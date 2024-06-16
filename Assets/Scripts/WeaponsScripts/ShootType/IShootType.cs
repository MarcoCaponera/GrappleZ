using GrappleZ_Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootType
{
    event Func<int, GameObject[]> BulletQuery;

    void Shoot(Vector3 origin, Vector3 direction, WeaponData data);
}
