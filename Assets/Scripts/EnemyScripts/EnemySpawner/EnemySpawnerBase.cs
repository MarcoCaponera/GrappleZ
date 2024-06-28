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

    #region OLD_SCRIPT
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
    #endregion

    #region OLD 2

    //[SerializeField]
    //private Transform spawnPoint;
    //[SerializeField]
    //private float timeBetweenSpawns;
    //private float timeSinceLastSpawn;

    //[SerializeField]
    //private int spawnCount;
    //protected static int totalEnemy = 0;

    //[SerializeField]
    //EnemyWalker enemyWalkerPrefab;

    //[SerializeField]
    //EnemySniper enemySniperPrefab;

    //[SerializeField]
    //EnemyRunner enemyRunnerPrefab;


    //private void Awake()
    //{
    //    walkerPool = CreatePool(enemyWalkerPrefab, 20);
    //    sniperPool = CreatePool(enemySniperPrefab, 10);
    //    runnerPool = CreatePool(enemyRunnerPrefab, 10);

    //    totalEnemy += spawnCount;
    //    GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());
    //}

    //private void Update()
    //{
    //    if (Time.time > timeSinceLastSpawn && spawnCount != 0)
    //    {
    //        //SPAWN
    //        SpawnEnemy(EnemyType.Walker);
    //        SpawnEnemy(EnemyType.Sniper);
    //        SpawnEnemy(EnemyType.Runner);
    //        timeSinceLastSpawn = Time.time + timeBetweenSpawns;
    //        spawnCount -= 1;
    //    }

    //}

    //#region EnemyPooling
    //IObjectPool<EnemyFirst> walkerPool;
    //IObjectPool<EnemyFirst> sniperPool;
    //IObjectPool<EnemyFirst> runnerPool;


    //private IObjectPool<EnemyFirst> CreatePool<T>(T prefab, int initialSize) where T : EnemyFirst
    //{
    //    var pool = new ObjectPool<T>(
    //        () => {
    //            var enemy = Instantiate(prefab);
    //            enemy.SetPool(null);
    //            return enemy;
    //        },
    //        enemy => {
    //            enemy.Activate();
    //            enemy.SetPool(GetPool(enemy));
    //        },
    //        enemy => enemy.Deactivate(),
    //        enemy => Destroy(enemy.gameObject),
    //        maxSize: 100
    //    );

    //    // Pre-popo
    //    List<T> initialEnemies = new List<T>(initialSize);
    //    for (int i = 0; i < initialSize; i++)
    //    {
    //        initialEnemies.Add(pool.Get());
    //    }
    //    foreach (var enemy in initialEnemies)
    //    {
    //        pool.Release(enemy);
    //    }

    //    return (IObjectPool<EnemyFirst>)pool;
    //}

    //private IObjectPool<EnemyFirst> GetPool(EnemyFirst enemy)
    //{
    //    if (enemy is EnemyWalker) return walkerPool as IObjectPool<EnemyFirst>;
    //    if (enemy is EnemySniper) return sniperPool as IObjectPool<EnemyFirst>;
    //    if (enemy is EnemyRunner) return runnerPool as IObjectPool<EnemyFirst>;
    //    return null;
    //}

    //public EnemyFirst GetEnemy(EnemyType enemyType)
    //{

    //    switch (enemyType)
    //    {
    //        case EnemyType.Runner:
    //            return runnerPool.Get();

    //        case EnemyType.Sniper:
    //            return sniperPool.Get();

    //        case EnemyType.Walker:
    //            return walkerPool.Get();

    //        default:
    //            return null;

    //    }
    //}

    //public void ReleaseEnemy(EnemyFirst enemy)
    //{
    //    enemy.ReleaseToPool();

    //}

    //#endregion




    //private void SpawnEnemy(EnemyType enemyType)
    //{
    //    totalEnemy--;
    //    EnemyFirst enemy = GetEnemy(enemyType);
    //    enemy.transform.position = spawnPoint.position;
    //}
    #endregion

    public class EnemySpawnerBase : MonoBehaviour
    {
        [Serializable]
        public class Pool
        {
            public EnemyType type;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> pools;
        private Dictionary<EnemyType, Queue<GameObject>> poolDictionary;

        [SerializeField]
        private Pool pool;

        [SerializeField]
        private int totalEnemiesToSpawn;
        [SerializeField]
        private float spawnInterval;
        [SerializeField]
        private Vector3 spawnPoint;

        private static int enemiesSpawned;
        private static int counter;

        void Start()
        {
            poolDictionary = new Dictionary<EnemyType, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.type, objectPool);
            }


            enemiesSpawned = 0;
            GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());
            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            while (enemiesSpawned < totalEnemiesToSpawn)
            {
                yield return new WaitForSeconds(spawnInterval);

                EnemyType randomType = GetRandomEnemyType();
                Vector3 spawnPosition = GetRandomSpawnPosition();
                SpawnFromPool(randomType, spawnPosition, Quaternion.identity);

                enemiesSpawned++;
            }
            

        }

        private EnemyType GetRandomEnemyType()
        {
            //int randomIndex = pools.Count;
            int randomIndex = UnityEngine.Random.Range(0, pools.Count);


            return pools[randomIndex].type;
        }

        public GameObject SpawnFromPool(EnemyType type, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(type))
            {
                Debug.LogWarning("Pool with type " + type + " doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[type].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            EnemyFirst enemy = objectToSpawn.GetComponent<EnemyFirst>();

            enemy.OnSpawn();

            poolDictionary[type].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void DespawnToPool(GameObject objectToDespawn)
        {
            objectToDespawn.SetActive(false);

            EnemyFirst enemy = objectToDespawn.GetComponent<EnemyFirst>();
            enemy.OnDespawn();
            counter++;
            CheckForWaveEnded();
            
        }

        private void CheckForWaveEnded()
        {
            if (counter >= totalEnemiesToSpawn)
            {
                GlobalEventManager.CastEvent(GlobalEventIndex.WaveEnded, GlobalEventArgsFactory.WaveEndedFactory());
            }
    }

        private Vector3 GetRandomSpawnPosition()
        {
            //int randomIndex = Random.Range(0, spawnPoints.Count);
            return spawnPoint;
        }
    }




