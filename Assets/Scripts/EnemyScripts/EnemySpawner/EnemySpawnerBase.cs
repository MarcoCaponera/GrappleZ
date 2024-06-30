<<<<<<< Updated upstream
using GrappleZ_Utility;
=======

#region OldScript
//using PlasticGui.WorkspaceWindow;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Pool;

//public enum EnemyType
//{
//    Walker,
//    Sniper,
//    Runner
//}


//    public class EnemySpawnerBase : MonoBehaviour
//    {
//        [Serializable]
//        public class Pool
//        {
//            public EnemyType Type;
//            public GameObject Prefab;
//            public int Size;
//        }

//        [Serializable]
//        public class Wave
//        {
//            public List<EnemyType> EnemyTypes;
//            public int TotalEnemiesInWave;
//            public float SpawnInterval;
//        }

//        public List<Pool> Pools;
//        public List<Wave> Waves;
//        private Dictionary<EnemyType, Queue<GameObject>> poolDictionary;

//        [SerializeField]
//        private Pool pool;

//        [SerializeField]
//        private int totalEnemiesToSpawn;
//        [SerializeField]
//        private float spawnInterval;
//        [SerializeField]
//        private Vector3 spawnPoint;

//        private static int enemiesSpawned;
//        private static int counter;

//        void Start()
//        {
//            poolDictionary = new Dictionary<EnemyType, Queue<GameObject>>();

//            foreach (Pool pool in Pools)
//            {
//                Queue<GameObject> objectPool = new Queue<GameObject>();

//                for (int i = 0; i < pool.Size; i++)
//                {
//                    GameObject obj = Instantiate(pool.Prefab);
//                    obj.SetActive(false);
//                    objectPool.Enqueue(obj);
//                }

//                poolDictionary.Add(pool.Type, objectPool);
//            }


//            enemiesSpawned = 0;
//            GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());
//            StartCoroutine(SpawnEnemies());
//        }

//        IEnumerator SpawnEnemies()
//        {
//            while (enemiesSpawned < totalEnemiesToSpawn)
//            {
//                yield return new WaitForSeconds(spawnInterval);

//                EnemyType randomType = GetRandomEnemyType();
//                Vector3 spawnPosition = GetRandomSpawnPosition();
//                SpawnFromPool(randomType, spawnPosition, Quaternion.identity);

//                enemiesSpawned++;
//            }


//        }

//        private EnemyType GetRandomEnemyType()
//        {
//            //int randomIndex = pools.Count;
//            int randomIndex = UnityEngine.Random.Range(0, Pools.Count);


//            return Pools[randomIndex].Type;
//        }

//        public GameObject SpawnFromPool(EnemyType type, Vector3 position, Quaternion rotation)
//        {
//            if (!poolDictionary.ContainsKey(type))
//            {
//                Debug.LogWarning("Pool with type " + type + " doesn't exist.");
//                return null;
//            }

//            GameObject objectToSpawn = poolDictionary[type].Dequeue();

//            objectToSpawn.SetActive(true);
//            objectToSpawn.transform.position = position;
//            objectToSpawn.transform.rotation = rotation;

//            EnemyFirst enemy = objectToSpawn.GetComponent<EnemyFirst>();

//            enemy.OnSpawn();

//            poolDictionary[type].Enqueue(objectToSpawn);

//            return objectToSpawn;
//        }

//        public void DespawnToPool(GameObject objectToDespawn)
//        {
//            objectToDespawn.SetActive(false);

//            EnemyFirst enemy = objectToDespawn.GetComponent<EnemyFirst>();
//            enemy.OnDespawn();
//            counter++;
//            CheckForWaveEnded();

//        }

//        private void CheckForWaveEnded()
//        {
//            if (counter >= totalEnemiesToSpawn)
//            {
//                GlobalEventManager.CastEvent(GlobalEventIndex.WaveEnded, GlobalEventArgsFactory.WaveEndedFactory());
//            }
//    }

//        private Vector3 GetRandomSpawnPosition()
//        {
//            //int randomIndex = Random.Range(0, spawnPoints.Count);
//            return spawnPoint;
//        }
//    }

#endregion

>>>>>>> Stashed changes
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

public class EnemySpawnerBase : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public EnemyType Type;
        public GameObject Prefab;
        public int Size;
    }

    [Serializable]
    public class Wave
    {
        public List<EnemyType> EnemyTypes;
        public int TotalEnemiesInWave;
        public float SpawnInterval;
    }

    public List<Pool> Pools;
    public List<Wave> Waves;
    private Dictionary<EnemyType, Queue<GameObject>> poolDictionary;

    [SerializeField]
    private Vector3 spawnPoint;

    private int currentWaveIndex = -1;
    private int enemiesSpawned;
    private int enemiesDefeated;

    private void Start()
    {
        InitializePools();
        StartNextWave();
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<EnemyType, Queue<GameObject>>();

        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.Type, objectPool);
        }
    }

    private void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < Waves.Count)
        {
            enemiesSpawned = 0;
<<<<<<< Updated upstream
            GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory(WaveEnum.First));
            StartCoroutine(SpawnEnemies());
=======
            enemiesDefeated = 0;
            Wave currentWave = Waves[currentWaveIndex];
            GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());
            StartCoroutine(SpawnEnemiesForWave(currentWave));
>>>>>>> Stashed changes
        }
        else
        {
<<<<<<< Updated upstream
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
                GlobalEventManager.CastEvent(GlobalEventIndex.WaveEnded, GlobalEventArgsFactory.WaveEndedFactory(WaveEnum.First));
            }
    }

        private Vector3 GetRandomSpawnPosition()
        {
            //int randomIndex = Random.Range(0, spawnPoints.Count);
            return spawnPoint;
=======
            // All waves completed
            Debug.Log("All waves completed!");
            // victory or defeat
>>>>>>> Stashed changes
        }
    }

    private IEnumerator SpawnEnemiesForWave(Wave wave)
    {
        while (enemiesSpawned < wave.TotalEnemiesInWave)
        {
            yield return new WaitForSeconds(wave.SpawnInterval);

            EnemyType enemyType = wave.EnemyTypes[UnityEngine.Random.Range(0, wave.EnemyTypes.Count)];
            SpawnFromPool(enemyType, spawnPoint, Quaternion.identity);

            enemiesSpawned++;
        }
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

        enemiesDefeated++;
        CheckForWaveEnded();
    }

    private void CheckForWaveEnded()
    {
        if (enemiesDefeated >= Waves[currentWaveIndex].TotalEnemiesInWave)
        {
            GlobalEventManager.CastEvent(GlobalEventIndex.WaveEnded, GlobalEventArgsFactory.WaveEndedFactory());
            StartNextWave();
        }
    }
}
