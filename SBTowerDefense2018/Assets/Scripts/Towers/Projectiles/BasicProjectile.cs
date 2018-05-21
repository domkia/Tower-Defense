using UnityEngine;

/// <summary>
/// Implements a basic projectile (no area of effect damage, no lasting effects, etc.)
/// </summary>
public class BasicProjectile : Projectile
{
    // Reference to enemy that this projectile is targetting.
    private Enemy enemyTarget;

    // This stores the last known position of the enemy target.
    private Vector3 lastKnownPosition;

    /// <summary>
    /// Sets the projectile's target.
    /// </summary>
    /// <param name="enemy">Enemy that this projectile will be targetting.</param>
    public void Seek(Enemy enemy, float multiplier)
    {
        int newDamage = (int) (Damage * multiplier);
        Damage = newDamage;
        enemyTarget = enemy;

        soundPlayer = GetComponentInChildren<PlayProjectileSounds>();
        soundPlayer.PlaySound(SoundType.ProjectileSpawn);
    }

    protected override void Update()
    {
        if (enemyTarget != null)
            lastKnownPosition = enemyTarget.transform.position;

        // We calculate the unit vector, which points in the direction of the enemy target.
        // We also calculate the distance the projectile travelled this frame.
        Vector3 directionUnitVector = (lastKnownPosition - transform.position).normalized;
        float distanceTravelledThisFrame = Speed * Time.deltaTime;

        // Move the projectile ...
        transform.Translate(directionUnitVector * distanceTravelledThisFrame, Space.World);

        // We check if we are close enough to the enemy target. If so, we damage the enemy.
        float currentDistance = (transform.position - lastKnownPosition).magnitude;
        if(currentDistance <= MinimumDistanceToTarget)
        {
            if (enemyTarget != null)
                enemyTarget.TakeDamage(Damage);
            soundPlayer.PlaySound(SoundType.ProjectileDestroy);
            Destroy(gameObject);
        }
    }
}