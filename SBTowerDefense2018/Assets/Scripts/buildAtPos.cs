using System.Collections.Generic;
using UnityEngine;

public class buildAtPos : MonoBehaviour
{
    public LayerMask layer;
    public bool isBuilding = false;
    public List<GameObject> towerPrefabs;
    public int index = 0;

    public Material canBuild;
    public Material cantBuild;
    public GameObject builParticle;

    /// <summary>
    /// Starts building tower of certain number in list
    /// </summary>
    /// <param name="num">Number of tower</param>
    public void startBuilding(int num)
    {
        isBuilding = true;
        index = num;
    }

    private void Update()
    {
        if (!isBuilding)
            return;
        TileVisual tile = GetTileAtMouse();
        if (tile == null)
            return;


        Material mat = TowerManager.Instance.CanBuildAt(tile.tile) ? canBuild : cantBuild;

        //Draw visual representation of tower
        Graphics.DrawMesh(towerPrefabs[index].GetComponentInChildren<MeshFilter>().sharedMesh,
            Matrix4x4.Translate(tile.transform.position), 
            mat, 0);

        if (Input.GetMouseButtonDown(0))
        {
            if (TowerManager.Instance.CanBuildAt(tile.tile) == false)
            {
                UISoundPlayer.Instance.PlayAlertSound();
                Debug.Log("Can't build here");
                return;
            }
            TowerManager.Instance.BuyTowerAt(tile.tile, towerPrefabs[index]);

            //Instantiate particle
            Instantiate(builParticle, tile.transform.position, Quaternion.identity);

            isBuilding = false;
        }
    }

    public TileVisual GetTileAtMouse()
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
