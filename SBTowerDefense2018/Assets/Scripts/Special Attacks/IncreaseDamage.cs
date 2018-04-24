using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamage : SpecialAttack
{
    public override float cooldown
    {
        get
        {
            return 30;
        }
    }

    public override void Do()
    {
        Increase();
        timer = cooldown;
        isReady = false;
    }

    public void Increase()
    {
        var towers = TowerManager.Instance.GetTowers();
        foreach (var tower in towers.Values)
        {
            BasicTower basicTower = tower as BasicTower;
            if(basicTower != null)
            {
                basicTower.MultiplyDamage(2, 10);
            }
            else
            {
                AOETower aoeTower = tower as AOETower;
                aoeTower.MultiplyDamage(2, 10);
            }
        }
    }


}
