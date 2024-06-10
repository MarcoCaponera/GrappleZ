using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySniperSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private float timeBetweenSpawns;
    [SerializeField]
    private int spawnCount;

    private float timeSinceLastSpawn;

    [SerializeField]
    private EnemySniper enemyPrefab;


    private IObjectPool<EnemySniper> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<EnemySniper>(CreateEnemy, OnGet, OnRelease);
    }

    private EnemySniper CreateEnemy()
    {
        EnemySniper enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        return enemy;
    }

    private void OnGet(EnemySniper enemy)
    {
        enemy.gameObject.SetActive(true);
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;
    }

    private void OnRelease(EnemySniper enemy)
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
