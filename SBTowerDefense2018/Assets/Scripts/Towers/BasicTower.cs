using UnityEngine;

/// <summary>
/// Implements a tower, which shoots a regular projectile (no area of effect, no lasting effects
/// such as poison, etc.)
/// </summary>
public class BasicTower : Tower, IReloadable
{
    // Maximum amount of ammo this tower can hold.
    public int ammoCapacity;
    // Range of this tower (in tiles)
    public int Range;
    // What projectile this tower will shoot out.
    public GameObject projectilePrefab;
    // Ammo left in this tower.
    public int AmmoLeft { get; private set; }
    // Reference to enemy target for this tower.
    protected Enemy currentTarget;

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

    // Reference to AmmoIndicator component. (We may need to move this to the base class.)
    private AmmoIndicator ammoIndicator;

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

    public override void Attack()
    {
        if (fireCountdown <= 0.0f && AmmoLeft > 0)
        {
            Shoot();
            fireCountdown = 1.0f / FireRate;
            ammoIndicator.UpdateIndicator(AmmoLeft, ammoCapacity);
        }
        fireCountdown -= Time.deltaTime;
    }

    protected override void Update()
    {
        // If there is no target, find another one. Otherwise, attack.
        if (currentTarget == null)
            UpdateTarget();
        else
            Attack();
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
            currentTarget = TilesInRange[index].Enemies[0];
    }

    /// <summary>
    /// Shoots a projectile. at the target.
    /// </summary>
    private void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectileGO.transform.parent = transform;

        // Decrement the amount of bullets left
        AmmoLeft--;
        if (AmmoLeft == 0)
            towerInteractable.SetToInteractive();

        BasicProjectile projectile = projectileGO.GetComponent<BasicProjectile>();
        projectile.Seek(currentTarget, multiplier);
    }

    private float multiplier = 1;

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

    //protected override void Awake()
    //{
    //    base.Awake();
    //    GetComponentInChildren<TowerInfo>().Setup(new InfoField("Reload Time", reloadTime.ToString()),
    //                                              new InfoField("Attack Speed", (1.0f/fireRate).ToString()));
    //}
}
