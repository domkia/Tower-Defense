using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
    // Stores the current target enemy
    private Enemy target;
    // Range of tower (in map units). Any potential targets, further away than this range, cannot be fired at.
    [Header("Properties")]
    public float Range = 5.0f;
    // Fire rate of tower (times per second)
    public float FireRate = 0.5f;
    // Prefab of bullets to shoot.
    public GameObject BulletPrefab;
    // Ammo capacity
    public int AmmoCapacity;
    // When this countdown goes to zero, a bullet is fired.
    private float fireCountdown = 0.0f;
    // Current amount of bullets left in the tower.
    public int BulletsLeft { get; private set; }
    // Ammo indicator game object.
    private GameObject ammoIndicator;
    // Reference to sprite renderer for the ammo indicator.
    private SpriteRenderer spriteRenderer;
    // Sprite to be drawn when a tower has no ammo left.
    public Sprite NoAmmoIndicator;
    // Sprite to be drawn when a tower has low ammo.
    public Sprite LowAmmoIndicator;

    private LinkedList<Enemy> enemyList;

    private void Start()
    {
        BulletsLeft = AmmoCapacity;
        enemyList = new LinkedList<Enemy>();

        ammoIndicator = new GameObject("Ammo Indicator");

        ammoIndicator.transform.parent = this.transform;
        // We add an offset in the positive Y direction, so we can see the sprite clearly.
        // If we didn't add an offset, the sprite would be embedded in the tower.
        ammoIndicator.transform.position = this.transform.position + new Vector3(0, 2f, 0);
        // We add a SpriteRenderer component as we will need to render sprites.
        spriteRenderer = ammoIndicator.AddComponent<SpriteRenderer>();
        ammoIndicator.SetActive(false);

        GetComponent<SphereCollider>().radius = Range;
    }

    private void Update()
    {
        if (target != null)
        {
            if (fireCountdown <= 0.0f && BulletsLeft > 0)
            {
                Shoot();
                fireCountdown = 1.0f / FireRate;
                UpdateIndicator();
            }

            fireCountdown -= Time.deltaTime;
        }
        else
            UpdateTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e == null)
            return;
        AddEnemyToQueue(e);
        e.OnDeath += RemoveEnemyFromQueue;
        Debug.Log("Enemy added to list!");
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e == null)
            return;
        e.OnDeath -= RemoveEnemyFromQueue;
        RemoveEnemyFromQueue(e);
        Debug.Log("Enemy removed from list!");
    }

    /// <summary>
    /// Finds nearest enemy in range and updates target. If there is no viable enemy, target is set to null
    /// (indicating no target)
    /// </summary>
    private void UpdateTarget()
    {
        target = (enemyList.Count != 0) ? enemyList.First.Value : null;
    }

    /// <summary>
    /// Shoots a bullet at the target.
    /// </summary>
    private void Shoot()
    {
        GameObject bulletGO = Instantiate(BulletPrefab, transform.position, Quaternion.identity);

        // For organizational purposes
        bulletGO.transform.parent = this.transform;
        bulletGO.name = "Bullet";

        // Decrement the amount of bullets left
        BulletsLeft--;

        Debug.Log(string.Format("Fire! Number of bullets left: {0}", BulletsLeft));

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Seek(target);
    }

    /// <summary>
    /// Reloads the gun, setting the amount of bullets left to the tower's ammo capacity.
    /// </summary>
    public void Reload()
    {
        BulletsLeft = AmmoCapacity;
        UpdateIndicator();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    private void AddEnemyToQueue(Enemy enemy)
    {
        enemyList.AddLast(enemy);
    }

    private void RemoveEnemyFromQueue(Enemy enemy)
    {
        enemyList.Remove(enemy);
        UpdateTarget();
    }

    /// <summary>
    /// Updates the ammo warning indicator.
    /// </summary>
    private void UpdateIndicator()
    {
        // No ammo case.
        if (BulletsLeft == 0)
        {
            spriteRenderer.sprite = NoAmmoIndicator;
            ammoIndicator.SetActive(true);
        }
        // Low ammo case.
        else if (BulletsLeft <= AmmoCapacity / 4)
        {
            spriteRenderer.sprite = LowAmmoIndicator;
            ammoIndicator.SetActive(true);
        }
        // Otherwise, the tower has plenty of ammo and we can stop drawing the sprite.
        else
            ammoIndicator.SetActive(false);
    }
    [Header("Healthbar")]
    public Image healthBar;
    public float health = 100;
    public void takeTowerdmg()
    {
        healthBar.fillAmount = health / 100f;
        
    }

    
}
