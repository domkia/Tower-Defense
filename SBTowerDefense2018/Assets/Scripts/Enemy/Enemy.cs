using System;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Base Enemy class
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    public abstract event Action<Enemy> OnDeath;

    public abstract void TakeDamage(int amount);
    public abstract void Idle();
    public abstract void Move(Path path);
    public abstract void Attack(Tower tower);

    public int Health { get; protected set; }
    public float Speed { get; protected set; }
    protected IEnemyState currentState;

    protected void Awake()
    {
        Idle();                 //Initial state is Idle
    }

    protected void Update()
    {
        if (currentState != null)
            currentState.UpdateState(this);
    }

    //TODO: decouple this
    [Header("HealthBar")]
    public Image healthBar;
}