using PlasticGui.WorkspaceWindow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public enum EnemyType
{
    Walker,
    Sniper,
    Runner
}

public abstract class EnemySpawnerBase : MonoBehaviour
{
    //[SerializeField]
    //private Transform[] spawnPoints;

    //[SerializeField]
    //private float timeBetweenSpawns;
    //[SerializeField]
    //private int spawnCount;

    //private float timeSinceLastSpawn;

    //[SerializeField]
    //protected EnemyFirst enemyPrefab;

    //protected static int totalEnemy = 0;

    //protected IObjectPool<EnemyFirst> enemyPool;

    //private void Awake()
    //{
    //    GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());

    //    totalEnemy += spawnCount;

    //    enemyPool = new ObjectPool<EnemyFirst>(CreateEnemy, OnGet, OnRelease);
    //}

    //protected virtual EnemyFirst CreateEnemy()
    //{
    //    EnemyFirst enemy = Instantiate(enemyPrefab);
    //    enemy.SetPool(enemyPool);
    //    return enemy;
    //}

    //private void OnGet(EnemyFirst enemy)
    //{
    //    enemy.gameObject.SetActive(true);
    //    Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
    //    enemy.transform.position = randomSpawnPoint.position;
    //}

    //private void OnRelease(EnemyFirst enemy)
    //{
    //    enemy.gameObject.SetActive(false);
    //    totalEnemy--;
    //    if (totalEnemy<=0)
    //    {
    //        GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveEndedFactory());
    //    }
    //}


    //private void Update()
    //{
    //    if (Time.time > timeSinceLastSpawn && spawnCount != 0)
    //    {
    //        //SPAWN
    //        enemyPool.Get();
    //        timeSinceLastSpawn = Time.time + timeBetweenSpawns;
    //        spawnCount -= 1;
    //    }

    //}

    [SerializeField]
    private int spawnCount;

    [SerializeField]
    EnemyWalker enemyWalkerPrefab;

    [SerializeField]
    EnemySniper enemySniperPrefab;

    [SerializeField]
    EnemyRunner enemyRunnerPrefab;

    IObjectPool<EnemyFirst> walkerPool;
    IObjectPool<EnemyFirst> sniperPool;
    IObjectPool<EnemyFirst> runnerPool;


    private IObjectPool<EnemyFirst> CreatePool<T>(T prefab) where T : EnemyFirst
    {
        return new ObjectPool<EnemyFirst>(
             () => {
                 var enemy = Instantiate(prefab);
                 enemy.SetPool(null);  
                 return enemy;
             },
             enemy => {
                 enemy.Activate();
                 enemy.SetPool(GetPool(enemy));
             },
             enemy => enemy.Deactivate(),
             enemy => Destroy(enemy.gameObject),
             true,
             spawnCount
         );
    }

    private IObjectPool<EnemyFirst> GetPool(EnemyFirst enemy)
    {
        if (enemy is EnemyRunner) return runnerPool;
        if (enemy is EnemySniper) return sniperPool;
        if (enemy is EnemyWalker) return walkerPool;
        return null;
    }

    public EnemyFirst GetEnemy(EnemyType enemyType)
    {

        switch (enemyType)
        {
            case EnemyType.Runner:
                return runnerPool.Get();

            case EnemyType.Sniper:
                return sniperPool.Get();

            case EnemyType.Walker:
                return walkerPool.Get();

            default:
                return null;

        }
    }

    public void ReleaseEnemy(EnemyFirst enemy)
    {
        enemy.ReleaseToPool();
    }

}

/*
 
 
  EnemySpawnerbse spawner;

Start
{
    GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());
    SpawnEnemy(EnemyType.Walker);
    SpawnEnemy(EnemyType.Sniper);
    SpawnEnemy(EnemyType.Runner);
}
 
 SpawnEnemy(EnemyType enemyType)
{
    EnemyFirst enemy = spawner.GetEnemy(enemyType);
    passo posizione di spawn
}

RetrunEnemyToPool(Enemytype enemyType)
{
    spawner.ReleaseEnemy(enemy);
}
 
 
 
 
 
 */
