using System;
using UnityEngine;

public class Swordsman : Enemy
{
    public int damage = 2;
    public int maxHealth = 12;
    public float maxSpeed = 0.5f;

    private void Start()
    {
        Damage = damage;
        CurrentHealth = maxHealth;
        Speed = maxSpeed;
    }

    protected override void DamageEffect()
    {
        base.DamageEffect();

        healthBar.UpdateHealthbar(CurrentHealth, maxHealth);
        if (CurrentHealth > 0)
            soundEffectPlayer.Play(SoundType.EnemyPain);

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
