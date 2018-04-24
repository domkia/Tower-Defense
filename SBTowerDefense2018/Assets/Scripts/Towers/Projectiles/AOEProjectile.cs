using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements a projectile which deals splash damage when it reaches its target, thus affecting enemies in range.
/// </summary>
public class AOEProjectile : Projectile
{
    // Splash damage radius.
    private const int ExplosionRadius = 1;

    // All enemies receive damage, equal to direct damage * this multiplier.
    [Range(0.0f, 1.0f)]
    public float SplashDamageMultiplier;

    // Stores a reference to the tile that this projectile will eventually reach.
    private HexTile targetTile;
    // Unit direction vector to target tile.
    private Vector3 unitDirectionVector;

    /// <summary>
    /// Sets the tile that this projectile will target.
    /// </summary>
    /// <param name="target">Target tile</param>
    public void SetTargetTile(HexTile target)
    {
        targetTile = target;
        // We can precalculate the unit direction vector ahead of time, because tiles don't move.
        unitDirectionVector = (target.worldPos - transform.position).normalized;
    }

    protected override void Update()
    {
        // Calculate the distance this projectile travelled this frame.
        float distanceTravelledThisFrame = Speed * Time.deltaTime;

        // Move the projectile ...
        transform.Translate(unitDirectionVector * distanceTravelledThisFrame, Space.World);

        // We check if we are close enough to the target tile. If so, this projectile deals splash damage.
        float currentDistance = (transform.position - targetTile.worldPos).magnitude;
        if (currentDistance <= MinimumDistanceToTarget)
            Explode();
    }

    /// <summary>
    /// Deals splash damage to surrounding enemies.
    /// </summary>
    private void Explode()
    {
        // First, deal direct damage to all enemies which are currently on the target tile.
        foreach (var enemy in targetTile.Enemies)
            enemy.TakeDamage(Damage);

        // Next, get all tiles which will receive splash damage.
        List<HexTile> splashDamageTiles = HexGrid.Instance.GetTilesInRange(targetTile, ExplosionRadius);

        // We get all enemies which will receive splash damage.
        List<Enemy> splashDamageEnemies = new List<Enemy>();
        foreach (var tile in splashDamageTiles)
            splashDamageEnemies.AddRange(tile.Enemies);

        // Deal splash damage
        int splashDamage = (int) (Damage * SplashDamageMultiplier);
        foreach (var enemy in splashDamageEnemies)
            enemy.TakeDamage(splashDamage);

        Destroy(gameObject);
    }
}