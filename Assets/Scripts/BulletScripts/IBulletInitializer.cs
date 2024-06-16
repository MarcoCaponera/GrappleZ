using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletInitArgs
{
    public float LifeTime;
    public Vector3 Velocity;
    public Vector3 SpawnPoint;
}

public interface IBulletInitializer 
{
    public void InitBullet(BulletInitArgs args);
}
