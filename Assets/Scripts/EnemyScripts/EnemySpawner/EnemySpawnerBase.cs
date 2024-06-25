using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class EnemySpawnerBase : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private float timeBetweenSpawns;
    [SerializeField]
    private int spawnCount;

    private float timeSinceLastSpawn;

    [SerializeField]
    protected EnemyFirst enemyPrefab;


    protected IObjectPool<EnemyFirst> enemyPool;

    private void Awake()
    {
        //GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());

        enemyPool = new ObjectPool<EnemyFirst>(CreateEnemy, OnGet, OnRelease);
    }

    protected virtual EnemyFirst CreateEnemy()
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
