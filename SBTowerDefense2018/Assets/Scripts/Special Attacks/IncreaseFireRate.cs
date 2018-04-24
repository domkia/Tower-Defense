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

    public override void Do()
    {
        timer = cooldown;
        isReady = false;
    }
}
