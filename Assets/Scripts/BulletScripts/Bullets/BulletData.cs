using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Bullets
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "Custom/Bullets/BulletData", order = 1)]
    public class BulletData : ScriptableObject
    {
        [SerializeField]
        private GameObject bulletPrefab;
        [SerializeField]
        private string bulletName;

        public GameObject BulletPrefab
        {
            get { return bulletPrefab; }
        }

        public string BulletName
        {
            get { return bulletName; }
        }
    }

}
