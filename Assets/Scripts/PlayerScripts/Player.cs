using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public class Player : MonoBehaviour, IDamageble
    {
        #region Modules
        private HealthModule healthModule;
        #endregion //Modules

        #region SerializeFields

        #region References

        [SerializeField]
        private PlayerController playerController;

        #endregion //References


        [SerializeField]
        private float damageInvTime;

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

        #region PrivateMembers
        private Coroutine invCoroutine;
        #endregion //PrivateMembers

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

        #region PublicEvents
        public Action<DamageContainer> onDamageTaken;
        #endregion

        #region HealthModule
        public void ResetHealth()
        {
            healthModule.Reset();
            NotifyHealthUpdatedGlobal();
            playerController.IsDead = false;
        }

        public void TakeDamage(DamageContainer damage)
        {
            healthModule.TakeDamage(damage);
        }

        public void InternalOnDamageTaken(DamageContainer container)
        {
            //healthUpdate?.Invoke(MaxHP, CurrentHP);
            NotifyHealthUpdatedGlobal();
            onDamageTaken?.Invoke(container);
            playerController.OnDamageTaken?.Invoke(container);
            SetInvulnearble(damageInvTime);
        }

        public void InternalOnDeath()
        {
            playerController.IsDead = true;
            playerController.OnDeath?.Invoke();
        }
        #endregion //HealthModule

        #region PrivateMethods
        private void SetInvulnearble(float invTime)
        {
            if (invCoroutine != null)
            {
                StopCoroutine(invCoroutine);
            }
            invCoroutine = StartCoroutine(InvulnerabilityCoroutine(invTime));
        }

        private void NotifyHealthUpdatedGlobal()
        {
            //GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated,
            //    EventArgsFactory.PlayerHealthUpdatedFactory((int)healthModule.MaxHP, (int)healthModule.CurrentHP));
            GlobalEventManager.CastEvent(GlobalEventIndex.PlayerHealthUpdated,
                GlobalEventArgsFactory.PlayerHealthUpdatedFactory(healthModule.MaxHP, healthModule.CurrentHP));
        }
        #endregion

        #region Coroutine
        private IEnumerator InvulnerabilityCoroutine(float invTime)
        {
            healthModule.SetInvulnerable(true);
            yield return new WaitForSeconds(invTime);
            healthModule.SetInvulnerable(false);
        }
        #endregion
    }
}
