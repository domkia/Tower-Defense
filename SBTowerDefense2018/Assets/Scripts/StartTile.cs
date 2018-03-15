using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : MonoBehaviour {

    //public float x { get; set; } //x coordinates on the world
    //public float z { get; set; } //y coordinates on the world

    public Transform enemyPrefab;

    public Transform spawnPoint;

    //public GameObject spawnPoint;
    

    public float countdown = 3f; // time between waves

    private int monsterCount = 3; // how many monsters to spawn

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = 3f;
        }

        countdown -= Time.deltaTime;
        
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }


}
