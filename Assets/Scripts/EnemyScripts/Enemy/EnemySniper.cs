using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace GrappleZ_Enemy
{
    public class EnemySniper : EnemyFirst
    {

        #region Serializables
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
        #endregion

        #region Mono
        private void Awake()
        {
            InitPool();
        }
        #endregion

        #region BulletPool
        private ObjectPool<GameObject> bulletPool;
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
        #endregion

        #region Override
        protected override void Attack()
        {
            transform.LookAt(lookPoint);

            if (!hasAttacked)
            {
                FireProjectile();

                hasAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttack);
            }
        }
        #endregion

        #region Internal Methods
        private void FireProjectile()
        {
            if (bulletPrefab == null || firePoint == null)
            {
                return;
            }

            GameObject bullet = bulletPool.Get();
            if (bullet == null)
            {
                return;
            }

            bullet.transform.position = firePoint.position;

            Vector3 directionToTarget = (lookPoint.position - firePoint.position).normalized;
            bullet.transform.rotation = Quaternion.LookRotation(directionToTarget);

            Rigidbody projectileRb = bullet.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.velocity = directionToTarget * bulletSpeed;
            }
            else
            {
                Debug.LogWarning("bullet is missing Rigidbody component");
            }

            StartCoroutine(ReturnProjectileToPool(bullet, 5f));
        }
        private void ResetAttack()
        {
            hasAttacked = false;
        }
        #endregion

    }
}


