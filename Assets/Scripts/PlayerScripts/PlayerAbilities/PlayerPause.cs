
using GrappleZ_Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GrappleZ_Player.PlayerHook;

namespace GrappleZ_Player
{
    public class PlayerPause : PlayerAbilityBase
    {

        #region Override
        public override void OnInputDisabled()
        {
            isPrevented = true;
            StopAbility();
        }

        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility()
        {
        }
        #endregion

        #region Mono

        protected void OnEnable()
        {
            InputManager.ManagePauseSubscription(OnInputPerformed, true);
            GlobalEventManager.AddListener(GlobalEventIndex.GameResumed, OnGameResumed);
        }

        protected void OnDisable()
        {
            InputManager.ManagePauseSubscription(OnInputPerformed, false);
            GlobalEventManager.RemoveListener(GlobalEventIndex.GameResumed, OnGameResumed);
        }


        #endregion


        #region Callbacks

        protected void OnInputPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (isPrevented) return;
            Time.timeScale = 0f;
            GlobalEventManager.CastEvent(GlobalEventIndex.GamePaused,GlobalEventArgsFactory.GamePausedFactory());

        }
        protected void OnGameResumed(GlobalEventArgs message)
        {
            Time.timeScale = 1f;
        }
        #endregion
    }
}