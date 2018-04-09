using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base Enemy class
/// </summary>
public abstract class Enemy : MonoBehaviour, IDamagable<Enemy>
{
    public HexTile currentlyOn { get; set; }                    //Tile this enemy is currently on
    public int Damage { get; set; }                             //Every enemy has damage?

    //IDamagable implementation
    public event Action<Enemy> OnDeath;
    public abstract void TakeDamage(int amount);
    public int Health { get; set; }

    //Enemy states
    public abstract void Idle();                                //Do nothing
    public abstract void Move(HexTile startTile);               //Just move along given path
    public abstract void Attack();

    protected IEnemyState currentState;
    public float Speed { get; protected set; }

    protected void Awake()
    {
        GameManager.OnGameOver += Idle;
        Idle();                                                 //Initial state is Idle
    }

    protected void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }

    protected void Die()
    {
        PlayerStats.Instance.EnemyKilled();
        if (OnDeath != null)
            OnDeath(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= Idle;
    }

    //TODO: decouple this
    [Header("HealthBar")]
    public Image healthBar;
}