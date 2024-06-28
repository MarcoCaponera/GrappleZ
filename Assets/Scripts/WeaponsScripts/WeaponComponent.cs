using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Weapons
{
    public class WeaponComponent : MonoBehaviour
    {
        #region SerializeField

        [SerializeField]
        private WeaponData weaponData;
        [SerializeField]
        private WeaponVisual visual;
        [SerializeField]
        private Transform shootForward;
        [SerializeField]
        private Transform shootPoint;
        [SerializeField]
        private AudioSource weaponSoundSource;
        [SerializeField]
        private AudioClip[] weaponSounds;
        #endregion

        #region PrivateAttributes

        private int leftAmmo;
        private IShootType shootType;
        private float currentDelay;
        private float currentReloadTime;
        private WeaponInventory owner;
        private bool waitForDelay;
        private bool isReloading;

        #endregion

        #region Mono

        protected void Awake()
        {
            switch (weaponData.ShootMode)
            {
                case ShootMode.SingleBullet:
                    shootType = new SingleShootType();
                    break;
                case ShootMode.Shotgun:
                    shootType = new ShotgunShootType();
                    break;
                default:
                    shootType = new SingleShootType();
                    break;  
            }
            shootType.BulletQuery += OnBulletQuery;
            weaponSoundSource = GetComponent<AudioSource>();
            ResetAmmo();
        }

        protected void Update()
        {
            HandleDelay();
            HandleReload();
        }

        #endregion

        #region PublicProperties

        public WeaponType Type
        {
            get { return weaponData.WeaponType; }
        }

        #endregion

        #region InternalMethods

        public void Init(WeaponInventory owner)
        {
            this.owner = owner;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        /// <summary>
        /// shoot function but returns knockback
        /// </summary>
        /// <returns>knockback</returns>
        public Vector3 Shoot()
        {
            if (!CanShoot()) return Vector3.zero;
            InternalShoot();
            waitForDelay = true;
            return -shootForward.forward * weaponData.KnockBackForce;
        }

        private void InternalShoot()
        {
            shootType.Shoot(shootPoint.position, shootForward.forward, weaponData);
            UseAmmo();
        }

        private void HandleDelay()
        {
            if (waitForDelay)
            {
                currentDelay += Time.deltaTime;
                if (currentDelay >= weaponData.ShootDelay)
                {
                    currentDelay = 0;
                    waitForDelay = false;
                }
            }
        }

        private void HandleReload()
        {
            if (isReloading)
            {
                currentReloadTime += Time.deltaTime;
                if (currentReloadTime >= weaponData.ReloadTime)
                {
                    currentReloadTime = 0;
                    ResetAmmo();
                    isReloading = false;
                }
            }
        }

        private bool CanShoot()
        {
            return !waitForDelay && !isReloading
                    && leftAmmo > 0;
        }

        public void SwapFrom()
        {
            if (isReloading)
            {
                currentReloadTime = 0;
            }
            visual.SetRendererActive(false);
        }

        public void SwapTo()
        {
            visual.SetRendererActive(true);
        }

        private void UseAmmo()
        {

            weaponSoundSource.PlayOneShot(weaponSounds[0]);


            leftAmmo--;
            if (leftAmmo <= 0)
            {
                isReloading = true;
            }
        }

        public void Reload()
        {
            if (leftAmmo >= weaponData.MagAmmo) return;
            weaponSoundSource.PlayOneShot(weaponSounds[1]);

            isReloading = true;
        }

        private void ResetAmmo()
        {
            leftAmmo = weaponData.MagAmmo;
        }

        #endregion

        #region Callback

        private GameObject[] OnBulletQuery(int count)
        {
            GameObject[] bullets = owner.GetBullets(weaponData.BulletData, count);
            return bullets;
        }

        #endregion
    }

}
