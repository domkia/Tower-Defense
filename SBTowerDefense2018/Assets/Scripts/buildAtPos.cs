using System.Collections.Generic;
using UnityEngine;

public class buildAtPos : MonoBehaviour
{
    public LayerMask layer;
    public bool isBuilding = false;
    public List<GameObject> towerPrefabs;
    public int index = 0;

    /// <summary>
    /// Starts building tower of certain number in list
    /// </summary>
    /// <param name="num">Number of tower</param>
    public void startBuilding(int num)
    {
        isBuilding = !isBuilding;
        index = num;
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
                    UISoundPlayer.Instance.PlayAlertSound();
                    Debug.Log("Can't build here");
                    return;
                }
                TowerManager.Instance.BuyTowerAt(tile.tile, towerPrefabs[index]);
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
