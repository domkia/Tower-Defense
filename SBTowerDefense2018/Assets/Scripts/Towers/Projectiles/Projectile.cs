using UnityEngine;

/// <summary>
/// Abstract class for a projectile that are fired by towers.
/// </summary>
public abstract class Projectile : MonoBehaviour
{
    // Speed of the projectile (in map units)
    public float Speed;
    // Damage, which this projectile will give to enemies
    public int Damage;
    // Once the projectile gets close enough to its target, it will perform its function.
    public float MinimumDistanceToTarget;

    protected PlayProjectileSounds soundPlayer;

    protected abstract void Update();
}
