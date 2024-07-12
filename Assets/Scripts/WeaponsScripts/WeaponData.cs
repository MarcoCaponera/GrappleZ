using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Bullets;

namespace GrappleZ_Weapons
{
    public enum ShootMode
    {
        SingleBullet,
        Shotgun
    }

    public enum ShootType
    {
        SingleShot
    }

    public enum WeaponType
    {
        Pistol,
        Shotgun
    }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "Custom/Weapons/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        #region SerializeFields

        [SerializeField]
        private WeaponType weaponType;
        [SerializeField]
        private ShootType shootType;
        [SerializeField]
        private int magAmmo;
        [SerializeField]
        private float damage;
        [SerializeField]
        private float bulletTime;
        [SerializeField]
        private float bulletSpeed;
        [SerializeField]
        private float shootDelay;
        [SerializeField]
        private float reloadTime;
        [SerializeField]
        private float recoilAmount;
        [SerializeField]
        private BulletData bulletData;
        [SerializeField]
        private ShootMode shootMode;
        [SerializeField]
        [Tooltip("This value only works if shootMode = Shotgun")]
        private int bulletsPerShot;
        [SerializeField]
        private float knockBackForce;

        #endregion

        #region PublicProperties

        public WeaponType WeaponType
        {
            get { return weaponType; }
        }

        public ShootType ShootType
        {
            get { return shootType; }
        }

        public float RecoilAmount
        {
            get { return recoilAmount; }
        }

        public float BulletSpeed
        {
            get { return bulletSpeed; }
        }

        public float Damage
        {
            get { return damage; }
        }

        public float BulletTime
        {
            get { return bulletTime; }
        }

        public float ShootDelay
        {
            get { return shootDelay; }
        }

        public float ReloadTime
        {
            get { return reloadTime; }
        }

        public BulletData BulletData
        {
            get { return bulletData; }
        }

        public ShootMode ShootMode
        {
            get { return shootMode; }
        }

        public int MagAmmo
        {
            get { return magAmmo; }
        }

        public float KnockBackForce
        {
            get { return knockBackForce; }
        }

        public int BulletsPerShot
        {
            get
            {
                if (shootMode != ShootMode.Shotgun) return 1;
                return bulletsPerShot;
            }
        }

        #endregion
    }
}
