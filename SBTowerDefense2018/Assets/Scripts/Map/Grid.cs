using UnityEngine;
 
public class Grid : MonoBehaviour
{
    public Transform hexPrefab;
 
    public int gridWidth = 11;
    public int gridHeight = 11;
 

    //Prefab size
    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
 
    Vector3 startPos;
 
    void Start()
    {
        CalcStartPos();
        CreateGrid();
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

    void CreateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int col = Random.Range(0, 2);  
                Transform hex = Instantiate(hexPrefab) as Transform;
                Renderer thisRend;
                thisRend = hex.GetComponent<Renderer>(); //Gets hex renderer component
                if (col == 0)
                {
                    thisRend.material.SetColor("_Color", Color.black);
                }
                if (col == 1)
                {
                    thisRend.material.SetColor("_Color", Color.white);
                }
                Vector2 gridPos = new Vector2(x, y);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                hex.name = "Hexagon" + x + "|" + y;
            }
        }
    }
}