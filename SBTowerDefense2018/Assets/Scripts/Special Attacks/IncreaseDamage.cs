using UnityEngine;

[CreateAssetMenu(menuName = "SpecialAttacks/IncreaseDamage")]
public class IncreaseDamage : SpecialAttack
{
    public float DamageMultiplier;
    public float Duration;

    public override void Do()
    {
        if (!isReady)
        {
            Debug.Log("Skill is still on cooldown " + timer);
            return;
        }
        UISoundPlayer.Instance.PlayCustomSound(AbilityActivatedSFX);
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
                basicTower.MultiplyDamage(DamageMultiplier, Duration);
        }
    }


}
