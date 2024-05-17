using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 

    private Unit unit;
    private float spawnTimer; 
    public float spawnRange = 10f;
    void Start()
    {
        unit = GetComponent<Unit>();

        spawnTimer = unit.trainTime;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();

            spawnTimer = unit.trainTime;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRange;
        spawnPosition.y = 0f;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
