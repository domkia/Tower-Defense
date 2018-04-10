using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : Singleton<EnvironmentSpawner>
{
    public GameObject tileVisual;
    public List<Material> grassMaterials;
    public Material towerMaterial;

    public List<GameObject> woodResourcePrefabs;
    public List<GameObject> stoneResourcePrefabs;
    public GameObject blockedTile;

    private void Start()
    {
        CreateEnvironment();
    }

    public void CreateEnvironment()
    {
        foreach (HexTile tile in HexGrid.Instance)
        {
            TileVisual tv = Instantiate(tileVisual, tile.worldPos, Quaternion.identity).GetComponent<TileVisual>();
            tv.SetTile(tile);
            tv.transform.parent = this.transform;
            MeshRenderer rend = tv.GetComponent<MeshRenderer>();
            rend.material = GetMaterial(tile.type);
            GameObject randomPrefab = null;
            switch (tile.type)
            {
                case TileType.Wood:
                    randomPrefab = woodResourcePrefabs[Random.Range(0, woodResourcePrefabs.Count - 1)];
                    break;
                case TileType.Stone:
                    randomPrefab = stoneResourcePrefabs[Random.Range(0, stoneResourcePrefabs.Count - 1)];
                    break;
                case TileType.Blocked:
                    randomPrefab = blockedTile;
                    break;
            }
            if (randomPrefab != null)
            {
                GameObject go = Instantiate(randomPrefab, tv.transform, false) as GameObject;
                IInteractable interactable = go.GetComponent<IInteractable>();
                if (interactable != null && interactable is ResourceInteractable)
                    interactable.OnCompleted += tv.InteractableClearTile;
            }
        }
    }

    public Material GetMaterial(TileType type)
    {
        Material mat;
        switch (type)
        {
            case TileType.Tower:
                mat = towerMaterial;
                break;
            default:
                mat = grassMaterials[Random.Range(0, grassMaterials.Count - 1)];
                break;
        }
        return mat;
    }
}
