using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements a projectile which deals splash damage when it reaches its target, thus affecting enemies in range.
/// </summary>
public class AOEProjectile : Projectile
{
    // Splash damage radius
    public int ExplosionRadius;

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
        // First, get a list of all tiles that will be affected by splash damage.
        List<HexTile> affectedTiles = new List<HexTile> { targetTile };
        var additionalTiles = HexGrid.Instance.GetTilesInRange(targetTile, ExplosionRadius);
        affectedTiles.AddRange(additionalTiles);

        // Next, we get a list of all enemies that will be affected.
        List<Enemy> affectedEnemies = new List<Enemy>();
        foreach (var tile in affectedTiles)
            affectedEnemies.AddRange(tile.Enemies);

        // Finally, deal damage.
        foreach (var enemy in affectedEnemies)
            enemy.TakeDamage(Damage);

        Destroy(gameObject);
    }
}