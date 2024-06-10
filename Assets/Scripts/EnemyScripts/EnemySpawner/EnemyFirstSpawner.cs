using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyFirstSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    
    [SerializeField]
    private float timeBetweenSpawns;
    [SerializeField]
    private int spawnCount;

    private float timeSinceLastSpawn;

    [SerializeField]
    private EnemyFirst enemyPrefab;


    private IObjectPool<EnemyFirst> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<EnemyFirst>(CreateEnemy, OnGet, OnRelease);
    }

    private EnemyFirst CreateEnemy()
    {
        EnemyFirst enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        return enemy;
    }

    private void OnGet(EnemyFirst enemy)
    {
        enemy.gameObject.SetActive(true);
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;
    }

    private void OnRelease(EnemyFirst enemy)
    {
        enemy.gameObject.SetActive(false);

    }


    private void Update()
    {
        if (Time.time > timeSinceLastSpawn && spawnCount != 0)
        {
            //SPAWN
            enemyPool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawns; 
            spawnCount -= 1;
        }
    }
}
