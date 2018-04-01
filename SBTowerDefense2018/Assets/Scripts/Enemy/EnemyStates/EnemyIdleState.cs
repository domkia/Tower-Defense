using UnityEngine;

class EnemyIdleState : IEnemyState
{
    public void UpdateState(Enemy enemy)
    {
        //for example: play idle animation
        //Set to idle when the game is over to prevent enemies from moving / attacking etc.
        enemy.transform.Rotate(Vector3.up * 180f * Time.deltaTime);
    }
}
