using System;
using UnityEngine;

public class Maceman : Enemy
{
    public int damage = 4;
    public int maxHealth = 15;
    public float maxSpeed = 1f;
    int moneyReward = 15;

    private void Start()
    {
        Damage = damage;
        CurrentHealth = maxHealth;
        Speed = maxSpeed;
    }

    public override void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        healthBar.UpdateHealthbar(CurrentHealth, maxHealth);

        if (CurrentHealth > 0)
            soundEffectPlayer.Play(SoundType.EnemyPain);

        if (CurrentHealth <= 0)
        {
            GiveReward(moneyReward);
            Die();
        }
        GameObject blood = Instantiate(bloodPrefab, GetComponentInParent<Transform>().position, GetComponentInParent<Transform>().rotation);
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
