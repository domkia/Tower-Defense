using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpecialAttacks/IncreaseDamage")]
public class IncreaseDamage : SpecialAttack
{

    public override void Do()
    {
        if (!isReady)
        {
            Debug.Log("Skill is still on cooldown " + timer);
            return;
        }
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
            //else
            //{
            //    AOETower aoeTower = tower as AOETower;
            //    aoeTower.MultiplyDamage(2, 10);
            //}
        }
    }


}
