using UnityEngine;

public class HexTile
{
    //More info about axial coordinates system: https://www.redblobgames.com/grids/hexagons/#coordinates-axial
    public readonly int x;              //x coordinate on axial grid
    public readonly int y;              //y coordinate on axial grid
    public TileType type { get; private set; }
    public Vector3 worldPos { get; private set; }

    public HexTile(int xCord, int yCord, Vector3 worldPosition, TileType tileType = TileType.Empty)
    {
        this.x = xCord;
        this.y = yCord;
        this.worldPos = worldPosition;
        this.type = tileType;
    }

    public void SetType(TileType tileType)
    {
        this.type = tileType;
    }

    public Vector2Int GetAxialCoords()
    {
        return new Vector2Int(x, y);
    }

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
