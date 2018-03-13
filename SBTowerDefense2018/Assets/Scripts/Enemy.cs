using System;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Base Enemy class
/// </summary>
abstract class Enemy
{
    public abstract event Action OnEnemyDied;
    public abstract void TakeDamage();


    public float health = 100;
    [Header("HealthBar")]
    public Image healthBar;

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / 100f;
        //if(health <= 0)
        
    }
}
