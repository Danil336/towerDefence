using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameValues GameValues;
    public GameObject enemyPrefab; 
    public Transform spawnPoint; 
    private float spawnInterval = 1.5f; 
    private float timer = 1.0f;

    public int maxEnemies = 5; 
    private int currentEnemies = 0; 

    private void Start() {
        
        spawnInterval = GameValues.enemySpawnInterval;

    }
    void Update()
    {
        // if (currentEnemies < maxEnemies)
        // {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0.0f;
            }
        // }
    }

    void SpawnEnemy()
    {

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        currentEnemies++;
        
    }
}