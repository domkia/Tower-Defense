using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements a projectile which deals splash damage when it reaches its target, thus affecting enemies in range.
/// </summary>
public class AOEProjectile : Projectile
{
    // Splash damage radius.
    public int ExplosionRadius;

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
    public void SetTargetTile(HexTile target, float multiplier)
    {
        int newDamage = (int)(Damage * multiplier);
        Damage = newDamage;
        targetTile = target;
        // We can precalculate the unit direction vector ahead of time, because tiles don't move.
        unitDirectionVector = (target.worldPos - transform.position).normalized;

        soundPlayer = GetComponentInChildren<PlayProjectileSounds>();
        soundPlayer.PlaySound(SoundType.ProjectileSpawn);
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

        // Next, get all tiles which will receive splash damage.
        List<HexTile> splashDamageTiles = HexGrid.Instance.GetTilesInRange(targetTile, ExplosionRadius);

        // We get all enemies which will receive splash damage.
        //List<Enemy> splashDamageEnemies = new List<Enemy>();
        //foreach (var tile in splashDamageTiles)
        //    splashDamageEnemies.AddRange(tile.Enemies);

        // First, deal direct damage to all enemies which are currently on the target tile.
        //foreach (var enemy in targetTile.Enemies)
        int count = targetTile.Enemies.Count;
        for(int i = 0; i < count; i++)
            targetTile.Enemies[i].TakeDamage(Damage);

        // Deal splash damage
        int splashDamage = (int) (Damage * SplashDamageMultiplier);
        for (int i = 0; i < splashDamageTiles.Count; i++)
        {
            foreach (var enemy in splashDamageTiles[i].Enemies)
                enemy.TakeDamage(splashDamage);
        }
        soundPlayer.PlaySound(SoundType.ProjectileDestroy);
        Destroy(gameObject);
    }
}