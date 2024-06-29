using Codice.CM.WorkspaceServer.DataStore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Bullets
{
    public class BulletComponent : MonoBehaviour, IBulletInitializer, IDamager
    {
        #region SerializeField

        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private BulletTrailData trailData;
        [SerializeField]
        private Renderer tRenderer;

        #endregion

        #region PrivateAttributes

        private float lifeTime;
        private float lifeTimeCounter;
        private float damage;
        private TrailRenderer trailRenderer;

        #endregion

        #region Mono
        protected void Awake()
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        protected void OnEnable()
        {
            tRenderer.enabled = true;
            InitTrail();
            lifeTimeCounter = 0;
        }

        protected void OnDisable()
        {
            trailRenderer.Clear();
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
                IDamageble damageble = other.GetComponent<IDamageble>();
                if (damageble != null)
                {
                    DamageContainer container = new DamageContainer();
                    container.Damage = damage;
                    damageble.TakeDamage(container);
                }
            }
            gameObject.SetActive(false);
        }

        #endregion

        #region InternalMethods

        private void InitTrail()
        {
            trailData.SetupTrail(trailRenderer);
        }

        public void InitBullet(BulletInitArgs args)
        {
            rb.velocity = args.Velocity;
            transform.position = args.SpawnPoint;
            lifeTime = args.LifeTime;
            damage = args.Damage;
        }

        #endregion
    }

}
