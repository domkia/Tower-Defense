using UnityEngine;
using System.Collections.Generic;

public class HexTile
{
    //More info about axial coordinates system: https://www.redblobgames.com/grids/hexagons/#coordinates-axial
    public readonly int x;              //x coordinate on axial grid
    public readonly int y;              //y coordinate on axial grid
    public TileType type { get; private set; }
    public Vector3 worldPos { get; private set; }

    //Called when type changes
    public static event System.Action<HexTile> OnTileTypeChanged = delegate { };

    //Enemies that are within this tile
    public List<Enemy> enemies { get; private set; }

    public HexTile(int xCord, int yCord, Vector3 worldPosition, TileType tileType = TileType.Empty)
    {
        this.x = xCord;
        this.y = yCord;
        this.worldPos = worldPosition;
        enemies = new List<Enemy>();
        SetType(tileType);
    }

    public void SetType(TileType tileType)
    {
        this.type = tileType;
        OnTileTypeChanged(this);
    }

    public Vector2Int GetAxialCoords()
    {
        return new Vector2Int(x, y);
    }

    public void EnemyExit(Enemy enemy)
    {
        enemy.OnDeath -= EnemyExit;
        enemies.Remove(enemy);
    }

    public void EnemyEnter(Enemy enemy)
    {
        enemies.Insert(enemies.Count, enemy);
        enemy.OnDeath += EnemyExit;
    }

    //TODO: Move this out of the way
    //Pathfinding
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public HexTile prev;            //Node we came from
    public void Reset()
    {
        gCost = hCost = 0;
        prev = null;
    }

    public override string ToString()
    {
        return string.Format("{0} {1}", x, y);
    }
}

public enum TileType
{
    Empty = 0,
    Resource = 1,
    Tower = 2,
    Blocked = 3
}
