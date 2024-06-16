using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Bullets;
using PlasticGui;

namespace GrappleZ_ObjectPooling
{
    public class BulletPool : MonoBehaviour
    {
        #region SerializeField
        [SerializeField]
        private BulletData[] bulletData;

        [SerializeField]
        private int itemAmount;
        #endregion

        #region PrivateAttributes

        private Dictionary<string, GameObject[]> items = new Dictionary<string, GameObject[]>();

        #endregion

        #region Mono
        protected void Awake()
        {
            foreach (BulletData b in bulletData)
            {
                items[b.name] = new GameObject[itemAmount];
                for(int i = 0; i < itemAmount; i++)
                {
                    items[b.name][i] = Instantiate(b.BulletPrefab);
                    items[b.name][i].SetActive(false);
                }
            }

        }
        #endregion

        #region PublicMethods

        public GameObject GetItem(BulletData data)
        {
            GameObject[] pool = items[data.name];
            foreach(GameObject item in pool) 
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                    return item;
                }
            }
            return null;
        }

        #endregion
    }
}
