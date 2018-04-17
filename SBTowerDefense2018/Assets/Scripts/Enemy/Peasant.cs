using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : Enemy
{
    public int damage = 1;
    public int maxHealth = 1;
    public float maxSpeed = 0.1f;

    private void Start()
    {
        Damage = damage;
        Health = maxHealth;
        Speed = maxSpeed;
    }

    public override void TakeDamage(int amount)
    {
        Health -= amount;
        healthBar.UpdateHealthbar(Health, maxHealth);
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