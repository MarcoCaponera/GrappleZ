using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_ObjectPooling;
using GrappleZ_Bullets;
using System;

namespace GrappleZ_Weapons
{
    public class WeaponInventory : MonoBehaviour
    {
        #region events

        public Action OnReload;
        #endregion

        #region SerializeField

        [SerializeField]
        private BulletPool bulletPool;

        #endregion

        #region Wrap
        public bool IsRealoading
        {
            get { return weapons[activeWeapon].getIsReloading(); }
        }
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
            GlobalEventManager.AddListener(GlobalEventIndex.WaveEnded, OnWaveEnded);
            GlobalEventManager.AddListener(GlobalEventIndex.PlayerDeath, OnPlayerDeath);
        }

        #endregion

        #region PublicMethods

        public void AddWeapon(WeaponType type)
        {
            foreach (WeaponComponent weapon in weapons)
            {
                if (weapon.Type == type)
                {
                    weapon.enabled = true;
                    return;
                }
            }
        }

        public void ChangeWeapon()
        {
            int newIndex = activeWeapon;
            if (newIndex >= weapons.Length - 1)
            {
                newIndex = 0;
            }
            else
            {
                for(int i = newIndex + 1; i < weapons.Length; i++)
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

        #region Callbacks

        protected void OnWaveEnded(GlobalEventArgs args)
        {
            foreach(WeaponComponent item in weapons)
            {
                if(item.enabled == false)
                {
                    AddWeapon(item.Type);
                    return;
                }
            }
        }

        private void OnPlayerDeath(GlobalEventArgs arg0)
        {
            foreach (WeaponComponent weapon in weapons)
            {
                if (weapon.Type != WeaponType.Pistol)
                {
                    weapon.enabled = false;
                }
                if (weapons[activeWeapon].enabled == false)
                {
                    ChangeWeapon();
                }
            }
        }

        #endregion
    }

}
