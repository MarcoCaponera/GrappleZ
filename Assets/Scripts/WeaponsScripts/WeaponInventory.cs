using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_ObjectPooling;
using GrappleZ_Bullets;

namespace GrappleZ_Weapons
{
    public class WeaponInventory : MonoBehaviour
    {
        #region SerializeField

        [SerializeField]
        private BulletPool bulletPool;

        #endregion

        #region PrivateAttributes

        private WeaponComponent[] weapons;
        private int activeWeapon;

        #endregion

        #region Mono

        protected void Awake()
        {
            weapons = GetComponentsInChildren<WeaponComponent>();
            foreach (WeaponComponent w in weapons)
            {
                w.Init(this);
            }
            activeWeapon = 0;
        }

        #endregion

        #region PublicMethods

        public void AddWeapon(WeaponData data)
        {
            foreach(WeaponComponent weapon in weapons)
            {
                if (weapon.Type == data.WeaponType)
                {
                    weapon.SetActive(true);
                    return;
                }
            }
        }

        public void ChangeWeapon()
        {
            int newIndex = activeWeapon + 1;
            if (newIndex >= weapons.Length)
            {
                newIndex = 0;
            }
            else
            {
                for(int i = newIndex; i < weapons.Length; i++)
                {
                    if (weapons[i].enabled)
                    {
                        newIndex = i;
                        break;
                    }
                }
            }
            weapons[activeWeapon].SwapFrom();
            activeWeapon = newIndex;
            weapons[activeWeapon].SwapTo();
        }

        public Vector3 ShootActiveWeapon()
        {
            return weapons[activeWeapon].Shoot();
        }

        public void ReloadActiveWeapon()
        {
            weapons[activeWeapon].Reload();
        }

        #endregion

        #region WrapperMethods

        public GameObject[] GetBullets(BulletData data, int count)
        {
            GameObject[] pool = new GameObject[count];
            for(int i = 0; i < count; i++)
            {
                pool[i] = bulletPool.GetItem(data);
            }
            return pool;
        }

        #endregion
    }

}
