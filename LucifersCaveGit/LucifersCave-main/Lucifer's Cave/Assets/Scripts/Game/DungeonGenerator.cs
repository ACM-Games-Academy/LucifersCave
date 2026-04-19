using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public bool visited = false;
        public bool[] walls = new bool[4]; 
    }

    public DoorLift doorLift;

    public Vector2 size;
    public int startPosition = 0;
    public GameObject roomPrefab;
    public Vector2 offset;

    private bool hasStarted = false;
    List<Cell> board;

    void Start()
    {
        MazeGenerator();
        hasStarted = true;
        if (hasStarted && doorLift != null)
            StartCoroutine(doorLift.DescendDoor());
        else
        {
            Debug.LogWarning("DoorLift reference is missing. Please assign it in the inspector.");
        }

        Debug.Log("DoorLift reference: " + (doorLift != null ? doorLift.name : "null"));
    }

    void Update()
    {
        
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];

                if (currentCell.visited)
                {
                    var roomInstance = Instantiate(roomPrefab, new Vector3(i * offset.x, 0, -j * offset.y),
                    Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    roomInstance.UpdateRoom(currentCell.walls);

                    roomInstance.name += " " + i + "-" + j;
                }
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x * size.y; i++)
        {
            board.Add(new Cell());
        }

        int currentCell = startPosition;
        int visitedCells = 1;
        board[currentCell].visited = true;

        Stack<int> cellStack = new Stack<int>();

        while (visitedCells < board.Count)
        {
            board[currentCell].visited = true;
            visitedCells++;

            if (currentCell == board.Count - 1)
            {
                break;
            }

            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (cellStack.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = cellStack.Pop();
                }
            }
            else
            {
                cellStack.Push(currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].walls[2] = true;
                        currentCell = newCell;
                        board[currentCell].walls[3] = true;
                    }
                    else
                    {
                        board[currentCell].walls[1] = true;
                        currentCell = newCell;
                        board[currentCell].walls[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].walls[3] = true;
                        currentCell = newCell;
                        board[currentCell].walls[2] = true;
                    }
                    else
                    {
                        board[currentCell].walls[0] = true;
                        currentCell = newCell;
                        board[currentCell].walls[1] = true;
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        // Check top neighbour
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }
        // Check down neighbour
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }
        // Check right neighbour
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }
        // Check left neighbour
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbours;
    }
}
