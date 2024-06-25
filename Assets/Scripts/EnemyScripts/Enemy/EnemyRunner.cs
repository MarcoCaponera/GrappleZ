using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyRunner : EnemyFirst
{
    ///ENEMY-POOL///
    //private IObjectPool<EnemyRunner> enemyPool;

    //public void SetPool(IObjectPool<EnemyRunner> enemyPool)
    //{
    //    this.enemyPool = enemyPool;
    //}

    protected override void SetPool(IObjectPool<EnemyFirst> pool)
    {
        base.SetPool(pool);
    }

    protected override void Die()
    {
        base.Die();
        enemyPool.Release(this);
    }
}
