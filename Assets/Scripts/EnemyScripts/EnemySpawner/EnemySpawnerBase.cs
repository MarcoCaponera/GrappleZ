
using GrappleZ_Utility;
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
            enemiesDefeated = 0;
            Wave currentWave = Waves[currentWaveIndex];
            //GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());
            StartCoroutine(SpawnEnemiesForWave(currentWave));
        }
        else
        {
            
            Debug.Log("All waves completed");
            
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

    private GameObject SpawnFromPool(EnemyType type, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogWarning("Pool with type " + type + " doesn't exist");
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
            //GlobalEventManager.CastEvent(GlobalEventIndex.WaveEnded, GlobalEventArgsFactory.WaveEndedFactory());
            StartNextWave();
        }
    }
}
