using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TileVisual : MonoBehaviour
{
    public HexTile tile { get; private set; }

    private MeshRenderer rend;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public void SetTile(HexTile tile)
    {
        this.tile = tile;
        ChangeTileType(this.tile.type);
    }

    public void ChangeTileType(TileType type)
    {
        tile.SetType(type);
        rend.material = EnvironmentSpawner.Instance.GetMaterial(type);
    }

    //TODO: change this
    public void InteractableClearTile(IInteractable i)
    {
        i.OnCompleted -= InteractableClearTile;
        ChangeTileType(TileType.Empty);
    }
}
