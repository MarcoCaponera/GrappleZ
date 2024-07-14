using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
     private float damage;
     private IObjectPool<GameObject> pool;

     public void Initialize(float damageAmount, IObjectPool<GameObject> objectPool)
     {
         damage = damageAmount;
         pool = objectPool;
     }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageble damageble = other.GetComponent<IDamageble>();

            if (damageble != null)
            {
                DamageContainer damageContainer = new DamageContainer();
                damageContainer.Damage = damage;
                damageble.TakeDamage(damageContainer);
            }

            pool.Release(gameObject);
        }
    }    
}
