using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

    //public float x { get; set; } //x coordinates on the world
    //public float z { get; set; } //y coordinates on the world

    public Transform enemyPrefab;

    public Transform spawnPoint;

    //public GameObject spawnPoint;
    public static System.Action onFinishedSpawning;

    public float countdown = 3f; // time between waves
    private float t = 0;

    public int waves = 3; //how many waves should spawn
    public int monsterCount = 3; // how many monsters to spawn
    public int waveCount = 0; // which wave is now going
    public bool isGameWon = false;

    private void Start()
    {
        spawnPoint = this.transform;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(waveCount < waves)
        {
            yield return SpawnWave();
            yield return new WaitForSeconds(0.5f);
            waveCount++;
        }

        Debug.Log("Finished");

        while(transform.childCount > 0)
        {
            yield return null;
        }
        isGameWon = true;
        if (onFinishedSpawning != null)
            onFinishedSpawning();
        yield return null;
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    /*
        if (waveCount >= waves)// && transform.childCount == 0)
            {
                Debug.Log(transform.childCount);
                if (isGameWon)
                    return;
                isGameWon = true;
                if (onFinishedSpawning != null)
                    onFinishedSpawning();
            }
        if (t <= 0f && waveCount < waves)
        {
            StartCoroutine(SpawnWave());
            
            t = countdown;
        }
        t -= Time.deltaTime;*/

    void SpawnEnemy()
    {
        Transform e = (Transform)Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        e.parent = this.transform;

    }


}
