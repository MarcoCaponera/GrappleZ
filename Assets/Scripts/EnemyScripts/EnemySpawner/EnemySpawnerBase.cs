using GrappleZ_Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Enemy
{
    public enum EnemyType
    {
        Walker,
        Sniper,
        Runner,
        Jumper
    }

    public class EnemySpawnerBase : MonoBehaviour
    {

        #region Serializables

        #region Pool
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
            public WaveEnum WaveEnum;
        }
        #endregion

        [SerializeField]
        private Vector3 spawnPoint;
        #endregion

        #region Private Members
        private Dictionary<EnemyType, Queue<GameObject>> poolDictionary;
        private int currentWaveIndex = -1;
        private int enemiesSpawned;
        private int enemiesDefeated;
        #endregion

        #region Public Members
        public List<Pool> Pools;
        public List<Wave> Waves;
        #endregion

        #region Mono
        private void Start()
        {
            InitializePools();
            GlobalEventManager.AddListener(GlobalEventIndex.WaveStarted, StartNextWave);
            GlobalEventManager.CastEvent(GlobalEventIndex.WaveStarted, GlobalEventArgsFactory.WaveStartedFactory());
        }

        #endregion

        #region Pool
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
        private GameObject SpawnFromPool(EnemyType type, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(type))
            {
                Debug.LogWarning("Pool with type " + type + " no exist");
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
        #endregion

        #region Spawn Coroutine
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
        #endregion

        #region Internal Methods
        //pro
        private void StartNextWave(GlobalEventArgs message)
        {
            currentWaveIndex++;
            if (currentWaveIndex < Waves.Count)
            {
                enemiesSpawned = 0;
                enemiesDefeated = 0;
                Wave currentWave = Waves[currentWaveIndex];
                StartCoroutine(SpawnEnemiesForWave(currentWave));
            }
            else
            {
                GlobalEventManager.CastEvent(GlobalEventIndex.GameEnded, GlobalEventArgsFactory.GameEndedFactory());
            }
        }
        private void CheckForWaveEnded()
        {
            if (enemiesDefeated >= Waves[currentWaveIndex].TotalEnemiesInWave)
            {
                GlobalEventManager.CastEvent(GlobalEventIndex.WaveEnded, GlobalEventArgsFactory.WaveEndedFactory(Waves[currentWaveIndex].WaveEnum));

                //To fix, this should be called after the WaveEndedPopUp
                //StartNextWave();
            }
        }
        #endregion
    }
}
