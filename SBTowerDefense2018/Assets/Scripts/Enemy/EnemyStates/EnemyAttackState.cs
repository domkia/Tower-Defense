using UnityEngine;

class EnemyAttackState : IEnemyState
{
    private Tower target = null;
    private float cooldown = 1f;
    private float t = 1f;

    public EnemyAttackState(Tower target)
    {
        this.target = target;
    }

    public void UpdateState(Enemy parent)
    {
        if (t >= cooldown)
        {
            target.TakeDamage(parent.Damage);   
            t = 0f;
        }
        t += Time.deltaTime;
    }
}