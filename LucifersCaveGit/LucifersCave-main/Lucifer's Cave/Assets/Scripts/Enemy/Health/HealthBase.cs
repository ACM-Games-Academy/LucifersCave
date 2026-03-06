using UnityEngine;

public abstract class HealthBase 
{
    public abstract void TakeDamage(int amount);

    public abstract void Death();
    public abstract int CurrentHealth { get; }
    public abstract int MaxHealth { get; }
}
