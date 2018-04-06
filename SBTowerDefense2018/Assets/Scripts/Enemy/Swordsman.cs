using System;

public class Swordsman : Enemy
{
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
        if (Health <= 0)
        {
            Die();
        }
    }

    //Called once
    public override void Idle()
    {
        currentState = new EnemyIdleState(this);
    }
    public override void Move(HexTile fromTile)
    {
        currentlyOn = fromTile;
        Path path = Pathfinding.GetPath(fromTile, HexGrid.Instance.CenterTile);
        currentState = new EnemyMovingState(path, this);
    }
    public override void Attack()
    {
        Tower targetTower = TowerManager.Instance.GetTowerAt(currentlyOn);
        if (targetTower == null)
            UnityEngine.Debug.LogError("targetTower is null in Attack()");

        //Inflict damage to tower and die
        targetTower.TakeDamage(Damage);
        Die();
    }
}
