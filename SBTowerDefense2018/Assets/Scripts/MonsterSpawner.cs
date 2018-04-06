using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public static event System.Action OnAllEnemiesKilled;

    public GameObject enemyPrefab;
    public float prepareTime = 10f;
    public int waves = 3;
    public int enemiesPerWave = 5;
    public float waitBetweenSpawns = 0.5f;

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
        int remaining = enemiesPerWave;

        //:: Every wave choses one random direction
        //int randomDirection = GetRandomOpenSpawnDirection();
        //if (randomDirection == -1)
        //    Debug.LogError("there are no open spawn directions");

        while (remaining > 0)
        {
            //OR: Every enemy chose random direction
            int randomDirection = GetRandomOpenSpawnDirection();
            if (randomDirection == -1)
                Debug.LogError("there are no open spawn directions");

            HexTile spawnTile = spawners[randomDirection].GetRandomOpenTile();
            Enemy enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
            enemy.transform.parent = this.transform;
            remaining--;
            //yield return new WaitForSeconds(1f);          //Idle for a second
            enemy.Move(spawnTile);
            yield return new WaitForSeconds(waitBetweenSpawns);
        }
    }

    private int GetRandomOpenSpawnDirection()
    {
        int rand = Random.Range(0, 5);
        for (int i = 0; i < spawners.Length; i++)
        {
            int index = (rand + i) % 6;
            if (spawners[index].IsOpen())
                return index;
        }
        return -1;
    }

    class SpawnDirection
    {
        public List<HexTile> spawnTiles { get; private set; }

        public SpawnDirection(int direction)
        {
            spawnTiles = HexGrid.Instance.GetEdgeTiles(direction);
        }

        public bool IsOpen()
        {
            for (int i = 0; i < spawnTiles.Count; i++)
                if (spawnTiles[i].type == TileType.Empty)
                    return true;
            return false;
        }

        public HexTile GetRandomOpenTile()
        {
            int randStart = Random.Range(0, spawnTiles.Count - 1);
            for (int i = 0; i < spawnTiles.Count; i++)
            {
                int index = (randStart + i) % spawnTiles.Count;     //Loop list from random start position
                if (spawnTiles[index].type == TileType.Empty)
                    return spawnTiles[index];
            }
            return null;            
        }
    }
}
