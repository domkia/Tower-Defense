using UnityEngine;
using System.Collections.Generic;

public class DisplayHexTiles : MonoBehaviour
{
    public Mesh hexMesh;
    public Material hexMaterial;

    private List<HexTile> activeTiles = null;
    private Matrix4x4[] matrices;

    public void ShowTiles(List<HexTile> tiles)
    {
        activeTiles = tiles;
        matrices = new Matrix4x4[activeTiles.Count];
        for(int i = 0; i < activeTiles.Count; i++)
            matrices[i] = Matrix4x4.Translate(activeTiles[i].worldPos);
    }

    public void HideTiles()
    {
        activeTiles = null;
    }

    public void LateUpdate()
    {
        if (activeTiles == null)
            return;

        Graphics.DrawMeshInstanced(hexMesh, 0, hexMaterial, matrices);
    }
}
