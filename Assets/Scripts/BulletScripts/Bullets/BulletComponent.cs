using Codice.CM.WorkspaceServer.DataStore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Bullets
{
    public class BulletComponent : MonoBehaviour, IBulletInitializer
    {
        #region SerializeField

        [SerializeField]
        private Rigidbody rb;

        #endregion

        #region PrivateAttributes

        private float lifeTime;
        private float lifeTimeCounter;

        #endregion


        #region Mono

        protected void OnEnable()
        {
            lifeTimeCounter = 0;
        }

        protected void Update()
        {
            lifeTimeCounter += Time.deltaTime;
            if (lifeTimeCounter >= lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                
            }
            gameObject.SetActive(false);
        }

        #endregion

        public void InitBullet(BulletInitArgs args)
        {
            rb.velocity = args.Velocity;
            transform.position = args.SpawnPoint;
            lifeTime = args.LifeTime;
        }
    }

}
