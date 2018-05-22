using UnityEngine;

public class CatapultTower : BasicTower
{
    public Transform catapultBase;
    public Transform projectileReleasePoint;

    private Animator animator;
    private float releaseWhenIntoAnimation = 0.8f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("animator is null");
    }

    protected override void SetupTilesInRange()
    {
        TilesInRange = HexGrid.Instance.GetTilesInRange(BuiltOn, Range);
    }

    protected override void PreShoot()
    {
        //TODO: for now animation state names are hardcoded...
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Shoot") == false)
        {
            animator.SetTrigger("Shoot");
            return;
        }
        if (info.normalizedTime >= releaseWhenIntoAnimation)
            base.PreShoot();
    }

    protected override void SpawnProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, projectileReleasePoint.position, Quaternion.identity);
        projectileGO.transform.parent = transform;
        AOEProjectile projectile = projectileGO.GetComponent<AOEProjectile>();
        projectile.SetTargetTile(currentTarget.currentlyOn, multiplier);
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
            return;

        float dx = currentTarget.transform.position.x - transform.position.x;
        float dz = currentTarget.transform.position.z - transform.position.z;
        float angle = Mathf.Atan2(dz, dx) * Mathf.Rad2Deg;
        catapultBase.rotation = Quaternion.AngleAxis(angle, Vector3.down);
    }
}