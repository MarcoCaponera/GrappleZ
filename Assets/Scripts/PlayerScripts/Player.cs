using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public class Player : MonoBehaviour
    {
        #region SerializeFields

        #region References

        [SerializeField]
        private PlayerController playerController;

        #endregion //References

        #endregion //SerializeFields

        #region StaticMembers
        //player instance
        public static Player instance; 

        //returns instance if instance has already been assigned
        public static Player Get()
        {
            if (instance != null) return instance;
            //finds player and returns it if instance is null
            instance = FindObjectOfType<Player>();
            return instance;
        }
        #endregion

        #region Mono

        private void Awake()
        {
            //singleton pattern, ensures that there will be only one instance of the player
            //if a player already exists, the gameObject will destroy itself
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}
