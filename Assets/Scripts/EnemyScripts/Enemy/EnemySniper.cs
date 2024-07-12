using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemySniper : EnemyFirst
{
    [SerializeField] 
    private GameObject bulletPrefab;
    [SerializeField] 
    private Transform firePoint;
    [SerializeField] 
    private float bulletSpeed = 20f;
    [SerializeField] 
    private int initialPoolSize = 10;
    [SerializeField] 
    private int maxPoolSize = 20;

    private ObjectPool<GameObject> bulletPool;

    #region Old
    //protected override void Attack()
    //{
    //    //base.Attack();
    //    transform.LookAt(lookPoint);

    //    if (!hasAttacked)
    //    {
    //        RaycastHit hitInfo;
    //        if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
    //        {
    //            Debug.Log("Attacking" + hitInfo.transform.name);
    //            if (!hitInfo.collider.CompareTag("Player")) return;

    //            IDamageble damageble = hitInfo.collider.gameObject.GetComponent<IDamageble>();
    //            if (damageble != null)
    //            {
    //                DamageContainer damageContainer = new DamageContainer();

    //                damageContainer.Damage = damage;

    //                damageble.TakeDamage(damageContainer);

    //            }


    //        }

    //        anim.SetBool("Walking", false);
    //        anim.SetBool("Idle", false);
    //        anim.SetBool("Attacking", false);
    //        anim.SetBool("Dead", false);

    //        hasAttacked = true;
    //        Invoke(nameof(ActiveAttacking), timeBetweenAttack);
    //    }
    //}

    //private void ActiveAttacking()
    //{
    //    hasAttacked = false;
    //    anim.SetBool("Attacking", false);


    //}
    #endregion

    private void Awake()
    {
        InitPool();
        
    }

    private void InitPool()
    {
        bulletPool = new ObjectPool<GameObject>(
            CreateProjectile,
            OnTakeProjectileFromPool,
            OnReturnProjectileToPool,
            OnDestroyPoolObject,
            true,
            initialPoolSize,
            maxPoolSize
        );
    }

    protected override void Attack()
    {
        transform.LookAt(lookPoint);
        if (!hasAttacked)
        {
            FireProjectile();

            anim.SetBool("Walking", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Dead", false);

            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    private void FireProjectile()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Projectile prefab or fire point is not set");
            return;
        }

        GameObject bullet = bulletPool.Get();
        if (bullet == null)
        {
            Debug.LogError("Failed to get projectile from pool");
            return;
        }

        bullet.transform.position = firePoint.position;

        // Calculate direction to the lookPoint
        Vector3 directionToTarget = (lookPoint.position - firePoint.position).normalized;
        bullet.transform.rotation = Quaternion.LookRotation(directionToTarget);

        Rigidbody projectileRb = bullet.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            // Set velocity towards the lookPoint
            projectileRb.velocity = directionToTarget * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("bullet is missing Rigidbody component");
        }

        StartCoroutine(ReturnProjectileToPool(bullet, 5f));
    }

    private GameObject CreateProjectile()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned");
            return null;
        }

        GameObject bullet = Instantiate(bulletPrefab);
        if (bullet == null)
        {
            Debug.LogError("Failed to instantiate projectile");
            return null;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript == null)
        {
            Debug.LogWarning("Projectile script not found");
            bulletScript = bullet.AddComponent<Bullet>();
        }

        bulletScript.Initialize(damage, bulletPool);
        return bullet;
    }

    private void OnTakeProjectileFromPool(GameObject bullet)
    {
        bullet.SetActive(true);
    }

    private void OnReturnProjectileToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject bullet)
    {
        Destroy(bullet);
    }

    private IEnumerator ReturnProjectileToPool(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet != null)
        {
            bulletPool.Release(bullet);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
        anim.SetBool("Attacking", false);
    }
}



