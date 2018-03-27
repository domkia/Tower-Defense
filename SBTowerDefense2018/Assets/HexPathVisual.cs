using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class HexPathVisual : MonoBehaviour
{
    [SerializeField]
    private float yOffset = 0.1f;
    private LineRenderer lineRend;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.alignment = LineAlignment.Local;
        lineRend.startWidth = 0.25f;
        lineRend.endWidth = 0.25f;
        lineRend.numCornerVertices = 4;
        lineRend.textureMode = LineTextureMode.Tile;
    }

    public void Setup(List<HexTile> path)
    {
        if (path == null || path.Count <= 0)
            return;
        int count = path.Count;
        lineRend.positionCount = count;
        for(int i = 0; i < count; i++)
        {
            lineRend.SetPosition(i, HexGrid.TileCoordToWorldPosition(path[i].x, path[i].y) + Vector3.up * yOffset);
        }
    }
}
