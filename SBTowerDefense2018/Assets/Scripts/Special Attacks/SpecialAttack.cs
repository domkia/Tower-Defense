using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAttack {

    public abstract string name { get; }
    public float timer = 0;
    public bool isReady = true;
    public abstract float cooldown { get; }

    public abstract void Do();

    public void UpdateCooldown()
    {
        if (isReady)
            return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isReady = true;
        }
    }

    
}
