using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public static event System.Action OnAllEnemiesKilled;

    public GameObject enemyPrefab;
    public float prepareTime = 10f;
    public int waves = 3;

    private int currentWave = 0;
    private SpawnDirection[] spawners;

    private void Start()
    {
        SetupSpawnDirections();
        currentWave = 0;
        GameManager.OnGameOver += () => StopAllCoroutines();
        StartCoroutine(Spawn());
    }

    private void SetupSpawnDirections()
    {
        spawners = new SpawnDirection[6];
        for (int i = 0; i < 6; i++)
            spawners[i] = new SpawnDirection(i);
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

        //Finished spawning, now wait for all enemies to die
        while (transform.childCount > 0)
            yield return null;              //TODO: Maybe check every half a second or so, not every frame

        if (OnAllEnemiesKilled != null)
            OnAllEnemiesKilled();
    }

    IEnumerator SpawnWave()
    {
        //TESTING
        HexTile spawnTile = spawners[0].spawnTiles[0];
        Enemy enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.parent = this.transform;
        yield return new WaitForSeconds(1f);
        enemy.Move(spawnTile);
        yield return new WaitForSeconds(0.5f);
    }

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
