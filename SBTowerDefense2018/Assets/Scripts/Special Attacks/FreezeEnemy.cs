using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SpecialAttacks/FreezeEnemies")]
public class FreezeEnemy : SpecialAttack
{
    public float freezeDuration;
    public GameObject freezeParticle;

    public override void Do()
    {
        if (!isReady)
        {
            Debug.Log("Skill is still on cooldown " + timer);
            return;

        }
        UISoundPlayer.Instance.PlayCustomSound(AbilityActivatedSFX);
        Vector3 center = new Vector3(0f, 0.1f, 0f);
        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        GameObject freeze = Instantiate(freezeParticle, center, rotation);
        Freezeddd();
        timer = cooldown;
        isReady = false;
        Debug.Log("Freeze " + timer + " " + (isReady).ToString());
    }

    public void Freezeddd()
    {
        
        HexTile Center = HexGrid.Instance.CenterTile;
        List<HexTile> tiles = HexGrid.Instance.GetTilesInRange(Center, HexGrid.Instance.mapRadius);
        foreach (HexTile tile in tiles)
        {
            List<Enemy> enemies = tile.Enemies;
            foreach(Enemy enemy in enemies)
            {
                enemy.Freeze();
            }
        }
    }
}
