using UnityEngine;

public class buildAtPos : MonoBehaviour
{
    public LayerMask layer;
    public bool isBuilding = false;
    public GameObject towerPrefab;

    public void startBuilding()
    {
        isBuilding = !isBuilding;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isBuilding)
        {
            TileVisual tile = GetTileAtClick();
            if (tile != null)
            {
                if (TowerManager.Instance.CanBuildAt(tile.tile) == false)
                {
                    Debug.LogError("Can't build here");
                    return;
                }
                TowerManager.Instance.BuildTowerAt(tile.tile, towerPrefab);
                isBuilding = !isBuilding;
            }
        }
    }

    public TileVisual GetTileAtClick()
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
