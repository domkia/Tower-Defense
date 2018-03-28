using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Enemy {

    public override event Action<Enemy> OnDeath;

    public int MaxHealth = 30;

    private void Start()
    {
        Health = MaxHealth;
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

}
