using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : Singleton<HexGrid>, IEnumerable
{
    //Six directions to neighbours in axial coordinate space
    static Vector2Int[] axialDirections =
    {
        new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
        new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1),
    };

    public int mapRadius = 3;
    public GameObject tileVisualPrefab;

    private HexTile[][] tiles;
    private static float hexWidth;
    private static float hexRadius;

    void Awake()
    {
        hexWidth = 1f;
        hexRadius = hexWidth / Mathf.Cos(30f * Mathf.Deg2Rad) / 2f; //2 x hexRadius === hexHeight

        int size = mapRadius * 2 + 1;                               //Array size
        this.tiles = new HexTile[size][];                           //Initialize two dimensional array
        for (int i = 0; i < size; i++)
            this.tiles[i] = new HexTile[size];

        InitializeTiles();                                          //Populate array with tiles
        SpawnPhysicalTiles();                                       //create TileVisual for every tile
    }

    public HexTile CenterTile
    {
        get{ return tiles[mapRadius][mapRadius]; }
    }

    /// <summary>
    /// Get Tile from the 2d array
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public HexTile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= tiles.Length || y >= tiles.Length)
            return null;
        return tiles[y][x];
    }

    /// <summary>
    /// Convert from axial to 2d array (remap negative indexes)
    /// </summary>
    /// <param name="axial"></param>
    /// <returns></returns>
    private HexTile GetTileAxial(Vector2Int axial)
    {
        int remapX = axial.x + mapRadius;
        int remapY = axial.y + mapRadius;
        return GetTile(remapX, remapY);
    }

    /// <summary>
    /// Get the surrounding neighbours based on axial directions
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public List<HexTile> GetNeighbours(HexTile tile)
    {
        if (tile == null)
            return null;
        Vector2Int tileCoord = tile.GetAxialCoords();
        List<HexTile> neighbours = new List<HexTile>();
        for (int i = 0; i < 6; i++)
        {
            HexTile neighbour = GetTileAxial(tileCoord + axialDirections[i]);       //In axial space, where center is 0,0
            if (neighbour != null)
                neighbours.Add(neighbour);
        }
        return neighbours;
    }

    /// <summary>
    /// Create all the tiles and calculate their axial and world coordinates
    /// </summary>
    private void InitializeTiles()
    {
        int diameter = tiles[0].Length;
        for (int y = 0; y < diameter; y++)
        {
            int count = diameter - Mathf.Abs(mapRadius - y);                        //How many tiles in a row
            int startIndex = Mathf.Max(mapRadius - y, 0);                           //Starting collumn in 2d array
            for (int x = startIndex; x < startIndex + count; x++)
            {
                int axialX = x - mapRadius;
                int axialY = y - mapRadius;
                Vector3 worldPosition = TileCoordToWorldPosition(axialX, axialY);
                tiles[y][x] = new HexTile(axialX, axialY, worldPosition, TileType.Empty);
            }
        }

        //Set corner / center tile types
        for (int i = 0; i < 6; i++)
            GetCornerTile(i).SetType(TileType.Blocked);

        DebugPrintGrid();
    }

    //TODO: move this to visual map spawner or something
    private void SpawnPhysicalTiles()
    {
        foreach (HexTile tile in this)
        {
            //Setup physical tile
            TileVisual tv = GameObject.Instantiate(tileVisualPrefab).GetComponent<TileVisual>();
            tv.SetTile(tile);
            tv.transform.position = tile.worldPos;
            tv.transform.parent = this.transform;
        }
    }

    /// <summary>
    /// From Axial coordinates to world position
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <returns></returns>
    private Vector3 TileCoordToWorldPosition(int x, int y)
    {
        Vector3 worldPos = new Vector3();
        worldPos.z = hexRadius * 3f / 2f * -y;
        worldPos.x = hexWidth * (x + y * 0.5f);
        return worldPos;
    }

    public int GetDistance(HexTile fromTile, HexTile toTile)
    {
        //Convert from axial to cube coordinates
        int x = Mathf.Abs(fromTile.x - toTile.x);
        int y = Mathf.Abs(fromTile.x + fromTile.y - toTile.x - toTile.y);
        int z = Mathf.Abs(fromTile.y - toTile.y);
        return (x + y + z) / 2;
    }

    public List<HexTile> GetTilesInRange(HexTile fromTile, int range)
    {
        //Debug.Log("Range: " + range);
        if (range <= 0 || range > mapRadius)
            throw new System.Exception("range is out of bounds. GetTilesInRange()");

        List<HexTile> tilesInRange = new List<HexTile>();
        for (int x = -range; x <= range; x++)
            for (int y = Mathf.Max(-range, -x-range); y <= Mathf.Min(range, -x+range); y++)
            {
                if (x == fromTile.x && y == fromTile.y)
                    continue;
                var z = -x - y;
                Vector3Int coords = AxialToCube(fromTile.GetAxialCoords()) + new Vector3Int(x, z, y);
                HexTile tileInRange = GetTileAxial(CubeToAxial(coords));
                if(tileInRange != null)
                    tilesInRange.Add(tileInRange);
                //Debug.Log("tile: " + neighbour);
            }
        return tilesInRange;
    }

    public List<HexTile> GetTilesRing(HexTile fromTile, int range)
    {
        if (range <= 0 || range > mapRadius)
            throw new System.Exception("range is out of bounds. GetTilesAtRing()");

        List<HexTile> ring = new List<HexTile>();
        Vector3Int coord = AxialToCube(fromTile.GetAxialCoords()) + AxialToCube(axialDirections[4]) * range;
        for (int i = 0; i < 6; i++)
            for (int j = 0; j < range; j++)
            {
                HexTile ringTile = GetTileAxial(CubeToAxial(coord));
                //Debug.Log("Tile: " + ringTile);
                //if(ringTile != null)
                ring.Add(ringTile);
                coord = AxialToCube(ringTile.GetAxialCoords()) + AxialToCube(axialDirections[i]);
            }
        return ring;
    }

    public List<HexTile> GetEdgeTiles(int direction)
    {
        if (direction < 0 || direction > 5)
            throw new System.Exception("direction is not valid. GetEdgeTiles()");

        List <HexTile> edgeTiles = new List<HexTile>();
        Vector2Int coord = axialDirections[direction] * mapRadius;       //Start coordinate
        //Debug.Log("start coord: " + coord);
        direction = (direction + 2) % 6;                                 //Direction in which we look for tiles
        for (int i = 0; i < mapRadius - 1; i++)
        {
            coord += axialDirections[direction];
            //Debug.Log("edge tile coord: " + coord);
            HexTile edgeTile = GetTileAxial(coord);
            //TODO: Check if its not null??
            edgeTiles.Add(edgeTile);
            //Debug.Log(coord);
        }
        return edgeTiles;
    }

    private HexTile GetCornerTile(int direction)
    {
        if (direction < 0 || direction > 5)
            throw new System.Exception("direction is not valid. GetCornerTile()");

        Vector2Int coord = axialDirections[direction] * mapRadius;
        return GetTileAxial(coord);
    }

    private Vector3Int AxialToCube(Vector2Int axial)
    {
        Vector3Int cube = new Vector3Int();
        cube.x = axial.x;
        cube.z = axial.y;
        cube.y = -cube.x - cube.z;
        return cube;
    }

    private Vector2Int CubeToAxial(Vector3Int cube)
    {
        Vector2Int axial = new Vector2Int();
        axial.x = cube.x;
        axial.y = cube.z;
        return axial;
    }

    public void DebugPrintGrid()
    {
        string s = "";
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                HexTile t = GetTile(j, i);
                s += (t == null ? "[-----]" : string.Format("[{0,2} {1,2}]", t.x, t.y));
            }
            s += "\n";
        }
        Debug.Log(s);
    }

    public IEnumerator GetEnumerator()
    {
        for (int y = 0; y < tiles.Length; y++)
            for (int x = 0; x < tiles[0].Length; x++)
            {
                if (tiles[y][x] == null)
                    continue;
                yield return tiles[y][x];
            }
    }
}