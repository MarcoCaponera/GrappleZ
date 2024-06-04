using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    
    [SerializeField]
    private float timeBetweenSpawns;

    private float timeSinceLastSpawn;

    [SerializeField]
    private Enemy enemyPrefab;

    private IObjectPool<Enemy> enemyPool;

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(CreateEnemy);
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.SetPool(enemyPool);
        return enemy;
    }

    private void Update()
    {
        if (Time.time > timeSinceLastSpawn)
        {
            //SPAWN
            enemyPool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawns; 
        }
    }
}
