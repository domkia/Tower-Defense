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

    public int Health { get; protected set; }
    // Speed
    public float Speed { get; protected set; }

    [Header("HealthBar")]
    public Image healthBar;
}
