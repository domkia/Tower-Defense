using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="SpecialAttacks/ReloadTowers")]
public class ReloadSpecial : SpecialAttack
{

    public override void Do()
    {
        if (!isReady)
        {
            Debug.Log("Skill is still on cooldown " + timer);
            return;

        }
        Dictionary<HexTile, Tower> towers= TowerManager.Instance.GetTowers();
        //Debug.Log("Reloaded");
        foreach(var tower in towers.Values)
        {
            IReloadable reloadable = tower as IReloadable;
            if (reloadable != null)
            {
                reloadable.Reload();
            }
        }
        isReady = false;
        timer = cooldown;
    }    
}
