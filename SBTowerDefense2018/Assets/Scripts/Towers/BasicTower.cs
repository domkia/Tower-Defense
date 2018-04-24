using UnityEngine;

/// <summary>
/// Implements a tower, which shoots a regular projectile (no area of effect, no lasting effects
/// such as poison, etc.)
/// </summary>
[RequireComponent(typeof(AmmoIndicator))]
public class BasicTower : Tower, IReloadable
{
    // Maximum amount of ammo this tower can hold.
    public int ammoCapacity;
    // What projectile this tower will shoot out.
    public GameObject projectilePrefab;
    // Ammo left in this tower.
    public int AmmoLeft { get; protected set; }
    // Reference to enemy target for this tower.
    protected Enemy currentTarget;

    // Time needed to reload the tower.
    public float ReloadTime;
    // Fire rate of the tower (projectiles per second)
    public float FireRate;
    // When this reaches zero, the tower will shoot a projectile.
    protected float fireCountdown;

    public override float InteractionDuration
    {
        get
        {
            return ReloadTime;
        }
    }

    float IReloadable.ReloadTime
    {
        get
        {
            return ReloadTime;
        }
    }

    // Reference to AmmoIndicator component.
    protected AmmoIndicator ammoIndicator;

    public override void Setup(HexTile builtOn)
    {
        base.Setup(builtOn);

        ammoIndicator = GetComponent<AmmoIndicator>();
        ammoIndicator.Setup(this);

        Interact(null);

        towerInteractable.SetParent(this);
        towerInteractable.OnCompleted += Interact;
    }

    protected override void SetupTilesInRange()
    {
        base.SetupTilesInRange();

        var additionalTiles = HexGrid.Instance.GetTilesInRange(BuiltOn, Range);
        TilesInRange.AddRange(additionalTiles);
    }

    private void Attack()
    {
        if (fireCountdown <= 0.0f && AmmoLeft > 0)
        {
            PreShoot();
        }
        fireCountdown -= Time.deltaTime;
    }

    protected virtual void Update()
    {
        // If there is no target, find another one. Otherwise, attack.
        if (currentTarget == null)
            UpdateTarget();
        else
            Attack();
    }

    /// <summary>
    /// Finds nearest enemy in range and updates target. If there is no viable enemy, target is set to null
    /// (which indicates no viable target)
    /// </summary>
    protected virtual void UpdateTarget()
    {
        int index = -1;
        float nearDist = float.MaxValue;
        for (int i = 0; i < TilesInRange.Count; i++)
            if (TilesInRange[i].Enemies.Count > 0)
            {
                float dist = (BuiltOn.worldPos - TilesInRange[i].Enemies[0].transform.position).sqrMagnitude;
                if (dist < nearDist)
                {
                    nearDist = dist;
                    index = i;
                }
            }
        if (index < 0)
            currentTarget = null;
        else
            currentTarget = TilesInRange[index].Enemies[0];
    }

    /// <summary>
    /// Shoots a projectile. at the target.
    /// </summary>
    protected virtual void SpawnProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectileGO.transform.parent = transform;
        BasicProjectile projectile = projectileGO.GetComponent<BasicProjectile>();
        projectile.Seek(currentTarget, multiplier);
    }

    private void Shoot()
    {
        // Child classes can override this
        SpawnProjectile();

        // Decrement the amount of bullets left
        AmmoLeft--;
        if (AmmoLeft == 0)
            towerInteractable.SetToInteractive();

        fireCountdown = 1.0f / FireRate;
        ammoIndicator.UpdateIndicator(AmmoLeft, ammoCapacity);
    }

    /// <summary>
    /// This methods allows you to wait or do some additional stuff before actually firing the projectile
    /// Ideal when using animations
    /// </summary>
    protected virtual void PreShoot()
    {
        //By default don't delay, just instantly shoot
        Shoot();
    }

    protected float multiplier = 1;

    public void MultiplyDamage(float multiplier, float duration)
    {
        this.multiplier = multiplier;
        Invoke("ResetDamage", duration);
    }

    private void ResetDamage()
    {
        multiplier = 1;
    }

    /// <summary>
    /// Interacts with the tower.
    /// </summary>
    public void Interact(IInteractable i)
    {
        Reload();
    }

    /// <summary>
    /// Reloads the tower, setting the amount of bullets left to the tower's ammo capacity.
    /// </summary>
    public void Reload()
    {
        AmmoLeft = ammoCapacity;
        ammoIndicator.UpdateIndicator(AmmoLeft, ammoCapacity);
    }

    //Clean up
    private void OnDestroy()
    {
        towerInteractable.OnCompleted -= Interact;
    }
}
