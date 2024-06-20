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

    protected override void Attack()
    {
        //base.Attack();
        transform.LookAt(lookPoint);

        if (!hasAttacked)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);
                if (!hitInfo.collider.CompareTag("Player")) return;

                IDamageble damageble = hitInfo.collider.gameObject.GetComponent<IDamageble>();
                if (damageble != null)
                {
                    DamageContainer damageContainer = new DamageContainer();

                    damageContainer.Damage = GetBodyDamage();

                    damageble.TakeDamage(damageContainer);

                }


            }

            anim.SetBool("Walking", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Dead", false);

            hasAttacked = true;
            Invoke(nameof(ActiveAttacking), timeBetweenAttack);
        }
    }

    private void ActiveAttacking()
    {
        hasAttacked = false;
        anim.SetBool("Attacking", false);


    }
}
