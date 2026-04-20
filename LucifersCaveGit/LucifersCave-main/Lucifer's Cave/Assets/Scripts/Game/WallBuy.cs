using UnityEngine;

public class WallBuy : MonoBehaviour, IInteractable
{
    public WeaponStats weaponStats;
    public RecoilProfiles weaponRecoil;
    private PlayerScore points;
    private PointSpawner pointSpawner;
    public Vector3 spawnedWeaponOffset = new Vector3(0.1f, 0, 0);
    private bool isPurchased = false;

    private WeaponManager inventory;
    public int cost;

    public void Initialize(WeaponManager inventory, PlayerScore playerScore, PointSpawner pointSpawner)
    {
        this.inventory = inventory;
        this.pointSpawner = pointSpawner;
        points = playerScore;
    }

    public void Interact()
    {
        if (isPurchased) return;

        if (points.points >= cost)
        {
            isPurchased = true;
            points.Purchasing(cost);
            pointSpawner.DeducePoints(cost);
            inventory.EquipWeapon(weaponStats, weaponRecoil, spawnedWeaponOffset);
            Debug.Log($"Purchased {weaponStats.weaponName} for {cost} points!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }
}

