using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Weapons
{
    public class ShotgunShootType : IShootType
    {
        public event Func<int, GameObject[]> BulletQuery;

        public void Shoot(Vector3 origin, Vector3 direction, WeaponData data)
        {
            GameObject[] bullet = BulletQuery?.Invoke(data.BulletsPerShot);
            foreach(GameObject item in bullet)
            {
                IBulletInitializer bulletInit = item.GetComponent<IBulletInitializer>();
                float xRecoil = UnityEngine.Random.Range(-data.RecoilAmount, data.RecoilAmount);
                float yRecoil = UnityEngine.Random.Range(-data.RecoilAmount, data.RecoilAmount);
                float zRecoil = UnityEngine.Random.Range(-data.RecoilAmount, data.RecoilAmount);
                Vector3 recoil = new Vector3(xRecoil, yRecoil, zRecoil);
                BulletInitArgs args = new BulletInitArgs();
                args.LifeTime = data.BulletTime;
                args.SpawnPoint = origin;
                args.Velocity = (direction + recoil) * data.BulletSpeed;
                bulletInit.InitBullet(args);
            }
        }
    }

}
