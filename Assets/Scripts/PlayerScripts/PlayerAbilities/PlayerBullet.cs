using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public class PlayerBullet : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Hitable"))
            {
                Debug.Log("Bullet Hitted " + collision.gameObject.name);
                Destroy(gameObject);
            }
        }

        /*[SerializeField]
        private DamageContainer damage;
        [SerializeField]
        private string damagebleTag;*/

        /*if (!other.CompareTag(damagebleTag)) return;
        IDamageble damageble = other.GetComponent<IDamageble>();
        if (damageble == null) return;
        damage.SetContactPoint(other.ClosestPoint(transform.position));
        damageble.TakeDamage(damage);*/
    }
}
