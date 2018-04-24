using UnityEngine;

/// <summary>
/// Implements a tower which shoots a projectile that has some area of effect (like an explosion, for instance)
/// </summary>
public class AOETower : Tower, IReloadable
{
    // Maximum amount of ammo this tower can hold.
    public int ammoCapacity;
    // Range of this tower (in tiles)
    public int Range;
    // What projectile this tower will shoot out.
    public GameObject projectilePrefab;
    // Ammo left in this tower.
    public int AmmoLeft { get; private set; }
    // Reference to target tile for this tower.
    private HexTile currentTarget;

    // Reference to AmmoIndicator component. (We may need to move this to the base class.)
    private AmmoIndicator ammoIndicator;

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

    // We should probably have an abstract parent class, called RangedTower, so we wouldn't need to override
    // all of these.

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

    // =====================================================================================

    public override void Attack()
    {
        if (fireCountdown <= 0.0f && AmmoLeft > 0)
        {
            // Unlike the basic tower, we need to update our target every shot.
            // This is because our target is not an enemy, but a tile.
            // Maybe we should give an enemy a reference to the current tile it is on?
            // If we do, we wouldn't need to override the UpdateTarget method for this class.
            UpdateTarget();
            if(currentTarget != null)
            {
                Shoot();
                fireCountdown = 1.0f / FireRate;
                ammoIndicator.UpdateIndicator(AmmoLeft, ammoCapacity);
            }
        }
        fireCountdown -= Time.deltaTime;
    }

    protected override void UpdateTarget()
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
            currentTarget = TilesInRange[index];
    }

    protected override void Update()
    {
        if (currentTarget == null)
            UpdateTarget();
        else
            Attack();
    }

    /// <summary>
    /// Shoots a projectile at the target.
    /// </summary>
    private void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectileGO.transform.parent = transform;

        // Decrement the amount of bullets left
        AmmoLeft--;
        if (AmmoLeft == 0)
            towerInteractable.SetToInteractive();

        AOEProjectile projectile = projectileGO.GetComponent<AOEProjectile>();
        projectile.SetTargetTile(currentTarget);
    }
}