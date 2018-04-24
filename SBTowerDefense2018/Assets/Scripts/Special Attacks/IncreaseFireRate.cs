using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireRate : SpecialAttack
{
    public override float cooldown
    {
        get
        {
            return 60;
        }
    }

    public override string name
    {
        get
        {
            return "Increase firerate";
        }
    }

    public override void Do()
    {
        timer = cooldown;
        isReady = false;
    }
}
