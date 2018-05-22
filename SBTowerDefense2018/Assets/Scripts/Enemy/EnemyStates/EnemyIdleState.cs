using UnityEngine;

class EnemyIdleState : IEnemyState
{
    private Enemy parent;

    public EnemyIdleState(Enemy parent)
    {
        this.parent = parent;
    }

    public void UpdateState()
    {
        parent.animator.Play("Idle");
        //for example: play idle animation
        //Set to idle when the game is over to prevent enemies from moving / attacking etc.
        //parent.transform.Rotate(Vector3.up * 180f * Time.deltaTime);
    }
}
