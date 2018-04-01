using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public static System.Action onAllEnemiesKilled;
    public static System.Action onWaveCompleted;

    public GameObject enemyPrefab;
    public float prepareTime = 10f;
    public int waves = 3;

    private int currentWave = 0;
    private SpawnDirection[] spawners;

    private void Start()
    {
        spawners = new SpawnDirection[6];
        for (int i = 0; i < 6; i++)
            spawners[i] = new SpawnDirection(i);
        currentWave = 0;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(prepareTime);
        while(currentWave < waves)
        {
            currentWave++;
            yield return SpawnWave();
            yield return new WaitForSeconds(prepareTime);
        }
    }

    IEnumerator SpawnWave()
    {
        //TESTING

        Vector3 worldPosition = spawners[0].spawnTiles[0].worldPos;
        Enemy enemy = Instantiate(enemyPrefab, worldPosition, Quaternion.identity).GetComponent<Enemy>();
        Path path = Pathfinding.GetPath(spawners[0].spawnTiles[0], HexGrid.Instance.CenterTile);
        if (path == null)
            Debug.LogError("path is null");
        yield return new WaitForSeconds(1f);
        enemy.Move(path);
        yield return new WaitForSeconds(0.5f);
    }

    /*
    void SpawnEnemy()
    {
        Transform e = (Transform)Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        e.parent = this.transform;
    }*/

    class SpawnDirection
    {
        public List<HexTile> spawnTiles;

        public SpawnDirection(int direction)
        {
            spawnTiles = HexGrid.Instance.GetEdgeTiles(direction);
        }

        public bool IsOpen
        {
            get
            {
                for (int i = 0; i < spawnTiles.Count; i++)
                    if (spawnTiles[i].type == TileType.Empty)
                        return true;
                return false;
            }
        }
    }
}
