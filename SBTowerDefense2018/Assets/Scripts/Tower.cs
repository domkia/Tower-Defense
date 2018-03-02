using UnityEngine;

public class Tower : MonoBehaviour {
    // Stores the current target's position
    private Transform target;
    // Range of tower (in map units). Any potential targets, further away than this range, cannot be fired at.
    [Header("Properties")]
    public float Range = 5.0f;
    // Fire rate of tower (times per second)
    public float FireRate = 0.5f;
    // All objects with this tag are considered enemies.
    public string EnemyTag = "Enemy";
    // Prefab of bullets to shoot.
    public GameObject BulletPrefab;
    // When this countdown goes to zero, a bullet is fired.
    private float fireCountdown = 0.0f;

    private void Start()
    {
        // Right now, the tower will update targets every half a second.
        InvokeRepeating("UpdateTarget", 0.0f, 0.5f);
    }

    private void Update()
    {
        if(target != null)
        {
            if(fireCountdown <= 0.0f)
            {
                Shoot();
                fireCountdown = 1.0f / FireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Finds nearest enemy in range and updates target. If there is no viable enemy, target is set to null
    /// (indicating no target)
    /// </summary>
    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance <= shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        target = ((nearestEnemy != null) && (shortestDistance <= Range)) ? nearestEnemy.transform : null;
    }

    /// <summary>
    /// Shoots a bullet at the target.
    /// </summary>
    private void Shoot()
    {
        GameObject bulletGO = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Seek(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
