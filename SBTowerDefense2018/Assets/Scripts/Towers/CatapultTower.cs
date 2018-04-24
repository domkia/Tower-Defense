using UnityEngine;

public class CatapultTower : AOETower
{
    public Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void SetupTilesInRange()
    {
        TilesInRange = HexGrid.Instance.GetTilesRing(BuiltOn, Range);
    }

    public override void Attack()
    {
        base.Attack();
        animator.SetTrigger("Shoot");
    }
}