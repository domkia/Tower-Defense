using System;

public class Swordsman : Enemy
{
    public override event Action<Enemy> OnDeath;

    public int MaxHealth = 30;
    public float MaxSpeed = 0.5f;

    private void Start()
    {
        Health = MaxHealth;
        Speed = MaxSpeed;
    }

    public override void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0 && OnDeath != null)
        {
            //Debug.Log("Died!");
            OnDeath(this);
            Destroy(gameObject);
        }
        
    }

    //Called once
    public override void Idle()
    {
        currentState = new EnemyIdleState();
    }
    public override void Move(Path path)
    {
        currentState = new EnemyMovingState(path);
    }
    public override void Attack(HexTile target)
    {
        currentState = new EnemyAttackState();
    }
}
