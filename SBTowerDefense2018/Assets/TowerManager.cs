using UnityEngine;
using System.Collections.Generic;
using System;

class TowerManager : Singleton<TowerManager>
{
    public MonsterSpawner spawner;
    public GameObject basePrefab = null;                // Our castle
    private Dictionary<HexTile, Tower> towers;          // Already built towers

    private void Start()
    {
        PlayerStats.Instance.Resources[0].Add(900);
        PlayerStats.Instance.Resources[1].Add(900);
        PlayerStats.Instance.Resources[2].Add(900);
        towers = new Dictionary<HexTile, Tower>();
        BuildBase();
    }

    private void BuildBase()
    {
        HexTile startTile = HexGrid.Instance.CenterTile;
        BuildTowerAt(startTile, basePrefab);
    }

    public Tower GetTowerAt(HexTile tile)
    {
        Tower tower;
        towers.TryGetValue(tile, out tower);
        if (tower == null)
            Debug.LogError("tower is null at: " + tile);
        return tower;
    }

    public void BuyTowerAt(HexTile tile, GameObject towerPrefab)
    {
        if (tile == null)
            throw new System.Exception("HexTile (key) is null when trying to add to the dictionary");
        Tower towercost = towerPrefab.GetComponent<Tower>();
        Debug.Log(towercost.WoodCost);
        if (towercost.WoodCost > PlayerStats.Instance.Resources[2].Amount)//checking if enough wood
            Debug.Log("Not Enough Wood");
        else if (towercost.IronCost > PlayerStats.Instance.Resources[0].Amount)
            Debug.Log("Not Enough Iron");
        else if (towercost.StoneCost > PlayerStats.Instance.Resources[1].Amount)
            Debug.Log("Not Enough Stone");
        else
        {
            PlayerStats.Instance.Resources[0].Spend(towercost.IronCost);
            PlayerStats.Instance.Resources[1].Spend(towercost.StoneCost);
            PlayerStats.Instance.Resources[2].Spend(towercost.WoodCost);
            Debug.Log(towercost.WoodCost);
            tile.SetType(TileType.Tower);                       //Set tile type
                                                                //Debug.Log(PlayerStats.Instance.Resources[2].ResourceName);
            Tower tower = Instantiate(towerPrefab, tile.worldPos, Quaternion.identity).GetComponent<Tower>();
            tower.OnDeath += DestroyTowerAt;                    //Setup tower
            tower.Setup(tile);
            towers.Add(tile, tower);                            //Add to the dictionary
        }
    }
    public void BuildTowerAt(HexTile tile, GameObject towerPrefab)
    {
        if (tile == null)
            throw new System.Exception("HexTile (key) is null when trying to add to the dictionary");
        ArcherTower towercost = towerPrefab.GetComponent<ArcherTower>();
       
        tile.SetType(TileType.Tower);                       //Set tile type
        //Debug.Log(PlayerStats.Instance.Resources[2].ResourceName);
        Tower tower = Instantiate(towerPrefab, tile.worldPos, Quaternion.identity).GetComponent<Tower>();
        tower.OnDeath += DestroyTowerAt;                    //Setup tower
        tower.Setup(tile);
        towers.Add(tile, tower);                            //Add to the dictionary
        
    }

    public void DestroyTowerAt(HexTile tile)
    {
        if (tile.type != TileType.Tower)
            Debug.LogError("There is no tower here");
        
        Tower tower;
        towers.TryGetValue(tile, out tower);
        if (tower == null)
            Debug.LogError("Tower is null");

        tower.OnDeath -= DestroyTowerAt;
        towers.Remove(tile);
        Destroy(tower.gameObject);
    }

    public bool CanBuildAt(HexTile atTile)
    {
        //Check if there are no enemies on this tile
        if (atTile.enemies.Count > 0)
            return false;

        //Check if towers are not close together
        List<HexTile> neighbours = HexGrid.Instance.GetNeighbours(atTile);
        for (int i = 0; i < neighbours.Count; i++)
            if (neighbours[i].type == TileType.Tower)
                return false;

        return atTile.type == TileType.Empty;
    }
}
