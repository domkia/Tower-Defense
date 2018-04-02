using UnityEngine;

public class ArcherTower : Tower
{
    public int ammoCapacity;
    public float reloadTime;
    public int archersCount = 1;
    public float fireRate = 0.5f;
    public int range = 2;
    public GameObject arrowPrefab;

    public int AmmoLeft { get; private set; }
    public override float InteractionDuration
    {
        get { return reloadTime; }
    }

    // When this countdown goes to zero, a bullet is fired.
    private float fireCountdown = 0.0f;

    protected override void Start()
    {
        //TEMPORARY
        BuiltOn = HexGrid.Instance.CenterTile;

        base.Start();

        SetupIndicator();
        Reload(null);
        towerInteractable.SetParent(this);
        towerInteractable.OnCompleted += Reload;
    }

    public override void Attack()
    {
        if (enemyList.Count == 0)
            return;

        if (fireCountdown <= 0.0f && AmmoLeft > 0)
        {
            Shoot();
            fireCountdown = 1.0f / fireRate;
            UpdateIndicator();
        }
        fireCountdown -= Time.deltaTime;
    }

    /// <summary>
    /// Shoots a bullet at the target.
    /// </summary>
    private void Shoot()
    {
        GameObject bulletGO = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        // For organizational purposes
        bulletGO.transform.parent = this.transform;

        // Decrement the amount of bullets left
        AmmoLeft--;

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Seek(enemyList.First.Value);
    }

    /// <summary>
    /// Reloads the tower, setting the amount of bullets left to the tower's ammo capacity.
    /// </summary>
    public void Reload(IInteractable i)
    {
        AmmoLeft = ammoCapacity;
        UpdateIndicator();
    }

    protected override void GetRangeTiles()
    {
        rangeTiles = HexGrid.Instance.GetTilesInRange(BuiltOn, range);
        base.SetupEnemyCallbacks();
    }

    //Clean up
    private void OnDestroy()
    {
        towerInteractable.OnCompleted -= Reload;
    }

//--------- Move this to separate class -----------------

    public float lowAmmoPercentage = 0.25f;
    public Sprite LowAmmoIndicator;         // Sprite to be drawn when a tower has low ammo.
    public Sprite NoAmmoIndicator;          // Sprite to be drawn when a tower has no ammo left.
    private GameObject ammoIndicator;
    private SpriteRenderer spriteRenderer;  // Reference to sprite renderer for the ammo indicator.

    private void SetupIndicator()
    {
        ammoIndicator = new GameObject("Ammo Indicator");
        ammoIndicator.transform.parent = this.transform;
        // We add an offset in the positive Y direction, so we can see the sprite clearly.
        // If we didn't add an offset, the sprite would be embedded in the tower.
        ammoIndicator.transform.position = this.transform.position + new Vector3(0, 2f, 0);
        // We add a SpriteRenderer component as we will need to render sprites.
        spriteRenderer = ammoIndicator.AddComponent<SpriteRenderer>();
        ammoIndicator.SetActive(false);
    }

    /// <summary>
    /// Updates the ammo warning indicator.
    /// </summary>
    private void UpdateIndicator()
    {
        // No ammo case.
        if (AmmoLeft == 0)
        {
            spriteRenderer.sprite = NoAmmoIndicator;
            ammoIndicator.SetActive(true);
            // Since the tower has no bullets left, it can now be interacted with.
            towerInteractable.SetToInteractive();
        }
        // Low ammo case.
        else if (AmmoLeft <= ammoCapacity * lowAmmoPercentage)
        {
            spriteRenderer.sprite = LowAmmoIndicator;
            ammoIndicator.SetActive(true);
        }
        // Otherwise, the tower has plenty of ammo and we can stop drawing the sprite.
        else
            ammoIndicator.SetActive(false);
    }
}
