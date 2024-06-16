using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyWalker : EnemyFirst
{
    ///ENEMY-POOL///
    private IObjectPool<EnemyWalker> enemyPool;

    public void SetPool(IObjectPool<EnemyWalker> enemyPool)
    {
        this.enemyPool = enemyPool;
    }
    protected override void Die()
    {
        base.Die();
        enemyPool.Release(this);
    }
}
