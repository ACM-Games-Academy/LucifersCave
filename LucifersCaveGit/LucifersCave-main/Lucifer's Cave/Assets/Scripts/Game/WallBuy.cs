using UnityEngine;

public class WallBuy : MonoBehaviour, IInteractable
{
    public WeaponStats weaponStats;
    public WeaponRecoil weaponRecoil;
    private PlayerScore points;

    private WeaponManager inventory;
    private int playerPoints;
    public int cost;

    public void Initialize(WeaponManager inventory, PlayerScore playerScore)
    {
        this.inventory = inventory;
        points = playerScore;
    }

    public void Interact()
    {
        if (points.points >= cost)
        {
            points.Purchasing(cost);
            inventory.EquipWeapon(weaponStats);
            Debug.Log($"Purchased {weaponStats.weaponName} for {cost} points!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }
}

