using UnityEngine;

class EnemyAttackState : IEnemyState
{
    private float cooldown = 1f;
    private float t = 1f;

    public void UpdateState(Enemy enemy)
    {
        if (t >= cooldown)
        {
            Debug.Log("Attack!!");
            t = 0f;
        }
        t += Time.deltaTime;
    }
}