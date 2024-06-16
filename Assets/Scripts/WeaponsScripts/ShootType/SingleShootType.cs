using GrappleZ_Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Bullets;

public class SingleShootType : IShootType
{
    public event Func<int, GameObject[]> BulletQuery;

    public void Shoot(Vector3 origin, Vector3 direction, WeaponData data)
    {
        GameObject[] bullet =  BulletQuery?.Invoke(data.BulletsPerShot);
        IBulletInitializer bulletInit = bullet[0].GetComponent<IBulletInitializer>();
        float xRecoil = UnityEngine.Random.Range(-data.RecoilAmount, data.RecoilAmount);
        float yRecoil = UnityEngine.Random.Range(-data.RecoilAmount, data.RecoilAmount);
        Vector3 recoil = new Vector3(xRecoil, yRecoil, 0);
        BulletInitArgs args = new BulletInitArgs();
        args.LifeTime = data.BulletTime;
        args.SpawnPoint = origin;
        args.Velocity = (direction + recoil) * data.BulletSpeed;
        bulletInit.InitBullet(args);
    }
}
