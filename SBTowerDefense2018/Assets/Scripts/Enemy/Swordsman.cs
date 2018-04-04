using System;

public class Swordsman : Enemy
{
    public override event Action<Enemy> OnDeath;

    public int damage = 2;
    public int maxHealth = 12;
    public float maxSpeed = 0.5f;

    private void Start()
    {
        Damage = damage;
        Health = maxHealth;
        Speed = maxSpeed;
    }

    public override void TakeDamage(int amount)
    {
        Health -= amount;
        healthBar.fillAmount = (float)Health / (float)maxHealth;
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
        currentState = new EnemyMovingState(path, this);
    }
    public override void Attack(HexTile tile)
    {
        Tower targetTower = TowerManager.Instance.GetTowerAt(tile);
        if (targetTower == null)
            UnityEngine.Debug.LogError("targetTower is null in Attack()");
        currentState = new EnemyAttackState(targetTower);
    }
}
