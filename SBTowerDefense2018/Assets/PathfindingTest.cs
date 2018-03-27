using UnityEngine;
using System.Collections.Generic;

public class PathfindingTest : MonoBehaviour
{
    public LayerMask layer;
    public HexPathVisual pathVisualPrefab;
    public HexGrid grid;

    private Pathfinding pathfinding;

    TileVisual startTile = null;
    TileVisual endTile = null;

    private void Start()
    {
        pathfinding = new Pathfinding(grid);
    }

    public void Update()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0.2)
        {
            TileVisual tile = GetTile();
            tile.ToggleVisible();
        }
        if (Input.GetMouseButtonDown(0))
        {
            startTile = GetTile();
            //Debug.Log("StartTile: " + startTile.tile);
            //List<HexTile> neighbours = grid.GetNeighbours(startTile.tile);
            //foreach (HexTile tile in neighbours)
            //    Debug.Log("N: " + tile);
        }
        if (Input.GetMouseButtonDown(1))
        {
            endTile = GetTile();
            //Debug.Log("EndTile: " + endTile.tile);

            //Debug.Log(string.Format("from {0} to {1} dist: {2}", startTile, endTile, grid.GetDistance(startTile.tile, endTile.tile)));

            if (startTile != endTile && startTile != null && endTile != null)
            {
                List<HexTile> path = pathfinding.GetPath(startTile.tile, endTile.tile);
                HexPathVisual pathVisual = Instantiate(pathVisualPrefab).GetComponent<HexPathVisual>();
                pathVisual.Setup(path);
                //Debug.Log(path.Count);
                //foreach (HexTile t in path)
                //    Debug.Log(t);
            }
            
        }
    }

    TileVisual GetTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            TileVisual tile = hit.collider.GetComponent<TileVisual>();
            return tile;
        }
        return null;
    }
}
