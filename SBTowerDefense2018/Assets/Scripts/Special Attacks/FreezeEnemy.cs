using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEnemy : SpecialAttack
{
    public float timer = 0;
    public float abillityDuration = 5;

    public override float cooldown
    {
        get
        {
            return 30;
        }
    }

    public override void Do()
    {
        Freezeddd();
        timer = cooldown;
        isReady = false;
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
