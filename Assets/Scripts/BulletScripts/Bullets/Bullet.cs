using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    //Bullet Script to move

     private float damage;
     private IObjectPool<GameObject> pool;

     public void Initialize(float damageAmount, IObjectPool<GameObject> objectPool)
     {
         damage = damageAmount;
         pool = objectPool;
     }

     private void OnCollisionEnter(Collision collision)
     {
         //if (collision.collider.CompareTag("Player"))
         //{

             IDamageble damageble = collision.gameObject.GetComponent<IDamageble>();

             if (damageble != null)
             {
                 DamageContainer damageContainer = new DamageContainer();
                 damageContainer.Damage = damage;
                 damageble.TakeDamage(damageContainer);
             }

             //Return the projectile to the pool instead of destroying it
             pool.Release(gameObject);
        // }
     }
    
}
