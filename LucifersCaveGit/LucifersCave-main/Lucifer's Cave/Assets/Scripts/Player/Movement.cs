using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public bool isSprinting;
    public bool canSprint;

    [Header("Crouching")]
    public float crouchSpeed;
    public Transform playerBody;
    public Animator animator;

    [Header("Ground Check")]
    float groundDrag = 5f;
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public float airMultiplier;

    [Header("Slope Management")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("KeyBinds")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    [Header("References")]
    public Transform orientation;
    public AudioSource walkingSound;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public bool canMove;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Air
    }

    private void StateHandler()
    {
        if (grounded && Input.GetKey(sprintKey) && canSprint)
        {
            state = MovementState.Sprinting;
            isSprinting = true;
            moveSpeed = sprintSpeed;
        }

        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.Crouching;
            moveSpeed = crouchSpeed;
            isSprinting = false;
        }

        else if (grounded)
        {
            state = MovementState.Walking;
            moveSpeed = walkSpeed;
            isSprinting = false;
        }

        else
        {
            state = MovementState.Air;
            isSprinting = false;
        }
    }

    private void Start()
    {
        canMove = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canSprint = true;

        animator = GetComponent<Animator>();
        walkingSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        if (canMove)
        {
            MyInput();
            SpeedControl();
            StateHandler();
        }
        rb.linearDamping = groundDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        if (canMove)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        else
        {
            return;
        }

        if (Input.GetKeyDown(crouchKey))
        {
            animator.SetBool("isCrouching", true);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            rb.useGravity = true;
            canSprint = false;
        }
        if (Input.GetKeyUp(crouchKey))
        {
            animator.SetBool("isCrouching", false);
            rb.useGravity = false;
            canSprint = true;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        rb.useGravity = !OnSlope();

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}