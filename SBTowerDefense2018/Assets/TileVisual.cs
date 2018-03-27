using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TileVisual : MonoBehaviour
{
    public HexTile tile { get; private set; }

    private MeshRenderer rend;
    private MaterialPropertyBlock propBlock;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        propBlock = new MaterialPropertyBlock();
    }

    public void SetTile(HexTile tile)
    {
        this.tile = tile;
    }

    public void ToggleVisible()
    {
        //rend.enabled = !rend.enabled;
        int type = (int)tile.type;
        type = ++type % 4;
        ChangeTileType((TileType)type);
    }

    public void ChangeTileType(TileType type)
    {
        tile.SetType(type);
        Color tintColor = Color.white;
        switch (type)
        {
            case TileType.Empty:
                tintColor = Color.white;
                break;
            case TileType.Resource:
                tintColor = Color.yellow;
                break;
            case TileType.Tower:
                tintColor = Color.grey;
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
