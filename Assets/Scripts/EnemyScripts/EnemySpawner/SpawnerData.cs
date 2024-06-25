using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SpawnerData", menuName = "Spawner/Data", order =0)]
public class SpawnerData : ScriptableObject
{
    [SerializeField]
    private EnemyFirst enemyPrefab;

    public EnemyFirst Prefab { get { return enemyPrefab; } }




}
