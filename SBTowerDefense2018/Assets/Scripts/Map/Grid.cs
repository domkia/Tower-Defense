using UnityEngine;
using System.IO;
 
public class Grid : MonoBehaviour
{
    public string path = "Assets/MapData/TestMap.txt";
    public Transform hexPrefab;
 
    public int gridWidth = 16;
    public int gridHeight = 9;
    

    //Prefab size
    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
 
    Vector3 startPos;
 
    void Start()
    {
        CalcStartPos();
        CreateFromFile(path);
    }
  
    void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;
 
        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * 0.75f * (gridHeight / 2);
 
        startPos = new Vector3(x, 0, z);
    }
 
    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;
 
        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.75f;
 
        return new Vector3(x, 0, z);
    }
    /// <summary>
    /// Read from file and generate grid
    /// </summary>
    /// <param name="filePath">File path</param>
    void CreateFromFile(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        Tile[,] grid = new Tile[gridHeight,gridWidth];
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split(';');
                int xCord = int.Parse(parts[0]);
                int yCord = int.Parse(parts[1]);
                char type = char.Parse(parts[2]);
                grid[y, x] = new Tile(x, y, type);
                Transform hex = Instantiate(hexPrefab) as Transform;
                Renderer thisRend;
                thisRend = hex.GetComponent<Renderer>(); //Gets hex renderer component
                Color tileColor = Color.white;
                Pick(type, ref tileColor);
                thisRend.material.SetColor("_Color", tileColor);
                Vector2 gridPos = new Vector2(x, y);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                hex.name = "Hexagon" + x + "|" + y;
            }
        }
     }
    /// <summary>
    /// Picks color for object depending on type from file
    /// </summary>
    /// <param name="type">Char that describes type of tile</param>
    /// <returns>Returns color, white by default</returns>
    void Pick(char type, ref Color tileColor)
    {
        switch (type)
        {
            case 'B': //Tile on which you can build towers
                {

                    tileColor = Color.green;
                    break;
                }
            case 'P': //Path tile
                {
                    tileColor = Color.yellow;
                    break;
                }
            case 'S': //Start tile
                {
                    tileColor = Color.magenta;
                    break;
                }
            case 'E': //End tile
                {
                    tileColor = Color.red;
                    break;
                }
            default:
                {
                    tileColor = Color.white;
                    break;
                }
        }
    }
}