using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TileVisual : MonoBehaviour
{
    public HexTile tile { get; private set; }

    private MeshRenderer rend;
    private MaterialPropertyBlock propBlock;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        propBlock = new MaterialPropertyBlock();
    }

    /// <summary>
    /// Called from HexGrid when instantiated
    /// </summary>
    /// <param name="tile"></param>
    public void SetTile(HexTile tile)
    {
        this.tile = tile;
        ChangeTileType(this.tile.type);
    }

    /*
    public void ToggleVisible()
    {
        //rend.enabled = !rend.enabled;
        int type = (int)tile.type;
        type = ++type % 4;
        ChangeTileType((TileType)type);
    }
    */

    public void ChangeTileType(TileType type)
    {
        tile.SetType(type);
        Color tintColor = Color.white;
        switch (type)
        {
            case TileType.Empty:
                tintColor = Color.white;
                break;
            case TileType.Wood:
                tintColor = Color.green;
                break;
            case TileType.Stone:
                tintColor = Color.gray;
                break;
            case TileType.Tower:
                tintColor = Color.cyan;
                break;
            case TileType.Blocked:
                tintColor = Color.black;
                break;
        }
        //Debug.Log("Change color");
        propBlock.SetColor("_Color", tintColor);
        rend.SetPropertyBlock(propBlock);
    }
}
