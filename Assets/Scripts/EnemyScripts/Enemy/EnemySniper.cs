using GrappleZ_Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemySniper : EnemyFirst
{
    ///ENEMY-POOL///
    private IObjectPool<EnemySniper> enemyPool;

    public void SetPool(IObjectPool<EnemySniper> enemyPool)
    {
        this.enemyPool = enemyPool;
    }
    protected override void Die()
    {
        base.Die();
        enemyPool.Release(this);
    }

   
}
