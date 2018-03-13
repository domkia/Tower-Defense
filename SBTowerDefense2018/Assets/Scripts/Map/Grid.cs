using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public Transform hexPrefab;
    public Transform forest;
    public Transform rock;

    //Prefab size
    float hexWidth = 0.866f;
    float hexHeight = 1.0f;

    public int map_radius = 3; //map radius

    public List<Tile> map = new List<Tile>(); //list of all tiles

    Vector3 startPos = new Vector3(0,0,0); //map center position
 
    void Start()
    {
        map = generateList(map_radius);
        generateMap(map);
    }

    /// <summary>
    /// Generates map of grid tile list
    /// </summary>
    /// <param name="map">List of tiles</param>
    void generateMap(List<Tile> map)
    {
        foreach(Tile tile in map)
        {
            Transform temp = hexPrefab;
            float rand = UnityEngine.Random.Range(0, 3);
            Vector2 gridPos = new Vector2(tile.x, tile.y);
            if (gridPos == Vector2.zero || rand == 0)
                temp = hexPrefab;
            if (rand == 1)
                temp = forest;
            if (rand <1)
                temp = rock;
            Transform hex = Instantiate(temp) as Transform;
            hex.position = tile.worldPos;
            hex.parent = this.transform;
            hex.name = "Hexagon" + tile.x + "|" + tile.y;
        }
    }

    /// <summary>
    /// Generates list of tiles
    /// </summary>
    /// <param name="map_radius">Radius of hexagon</param>
    /// <returns>return list</returns>
    List<Tile> generateList(int map_radius) { 
        List<Tile> map = new List<Tile>();
        for (int q = -map_radius; q <= map_radius; q++)
        {
            int r1 = Math.Max(-map_radius, -q - map_radius);
            int r2 = Math.Min(map_radius, -q + map_radius);
            for (int r = r1; r <= r2; r++)
            {
                Tile test = new Tile(q, r, CalcWorldPos(new Vector2(q, r)));
                map.Add(test);
            }
        }
        return map;
    }

    /// <summary>
    /// Calculates coordinates of hexagon on map, based of their coordinates in grid
    /// </summary>
    /// <param name="gridPos">Position in grid vector</param>
    /// <returns>return vector with coordinates on map</returns>
    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        offset = hexWidth / 2;
        float x = startPos.x + gridPos.x * hexWidth;
            x = x + offset * gridPos.y;
        float z = startPos.z + gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }
}