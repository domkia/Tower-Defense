using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour, IEnumerable
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

    void Start()
    {
        hexRadius = 0.5f;      
        hexWidth = Mathf.Cos(30f * Mathf.Deg2Rad) * hexRadius * 2f; //Set constants

        int size = mapRadius * 2 + 1;                               //Array size
        this.tiles = new HexTile[size][];                           //Initialize two dimensional array
        for (int i = 0; i < size; i++)
            this.tiles[i] = new HexTile[size];

        InitializeTiles(tileVisualPrefab);                          //Populate array with tiles
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
    private void InitializeTiles(GameObject tileVisual)
    {
        int diameter = tiles[0].Length;
        for (int y = 0; y < diameter; y++)
        {
            int count = diameter - Mathf.Abs(mapRadius - y);                        //How many tiles in a row
            int startIndex = Mathf.Max(mapRadius - y, 0);                           //Starting collumn in 2d array
            for (int x = startIndex; x < startIndex + count; x++)
            {
                TileVisual tv = GameObject.Instantiate(tileVisual).GetComponent<TileVisual>();
                int axialX = x - mapRadius;
                int axialY = y - mapRadius;
                tiles[y][x] = new HexTile(axialX, axialY, TileType.Empty);//(x == 0 && y == 0)? TileType.Tower : TileType.Empty); //The very center tile is our base
                tv.SetTile(tiles[y][x]);                                            //Set tile 
                tv.transform.position = TileCoordToWorldPosition(axialX, axialY);   //Position physical tile in the world
                tv.transform.parent = this.transform;
            }
        }
        DebugPrintGrid();
    }

    /// <summary>
    /// From Axial coordinates to world position
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <returns></returns>
    public static Vector3 TileCoordToWorldPosition(int x, int y)
    {
        Vector3 worldPos = new Vector3();
        worldPos.z = hexRadius * 3f / 2f * -y;
        worldPos.x = hexWidth * (x + y * 0.5f);
        return worldPos;
    }

    public int GetDistance(HexTile fromTile, HexTile toTile)
    {
        //Conver from axial to cube coordinates
        int x = Mathf.Abs(fromTile.x - toTile.x);
        int y = Mathf.Abs(fromTile.x + fromTile.y - toTile.x - toTile.y);
        int z = Mathf.Abs(fromTile.y - toTile.y);
        return (x + y + z) / 2;
    }

    /*
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
    }*/

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
            for(int x = 0; x < tiles[0].Length; x++)
                yield return tiles[y][x];
    }
}