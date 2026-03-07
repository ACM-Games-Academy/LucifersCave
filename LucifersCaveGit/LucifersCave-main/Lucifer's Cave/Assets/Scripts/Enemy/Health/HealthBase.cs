using UnityEngine;

public interface HealthBase 
{
    public void TakeDamage(int amount);

    public void Death();
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
}
