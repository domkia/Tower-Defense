using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildAtPos : MonoBehaviour {
    public bool isBuilding = false;
    public Transform towerPrefab;
    public GameManager gManager;

    public void startBuilding()
    {
        isBuilding = !isBuilding;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isBuilding)
        {
            TileVisual tile = gManager.GetTileAtClick();
            if (tile != null)
            {

                Transform tower = (Transform)Instantiate(towerPrefab, tile.transform.position, tile.transform.rotation);
                tile.ChangeTileType(TileType.Tower);
                isBuilding = !isBuilding;
            }
        }
    }
}
