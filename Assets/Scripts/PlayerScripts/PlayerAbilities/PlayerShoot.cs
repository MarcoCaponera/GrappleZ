using GrappleZ_Utility;
using GrappleZ_Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GrappleZ_Player
{

    public class PlayerShoot : PlayerAbilityBase
    {
        #region Mono

        protected void OnEnable()
        {
            InputManager.ManageShootSubscription(OnShootInputPerformed, true);
            InputManager.ManageWeaponSwapSubscription(OnWeaponSwapInputPerformed, true);
            InputManager.ManageReloadSubscription(OnReloadInputPerformed, true);
        }

        protected void OnDisable()
        {
            InputManager.ManageShootSubscription(OnShootInputPerformed, false);
            InputManager.ManageWeaponSwapSubscription(OnWeaponSwapInputPerformed, false);
            InputManager.ManageReloadSubscription(OnReloadInputPerformed, false);
        }

        #endregion

        #region VisualControl
        private void StartShootAnimation()
        {
            playerVisual.SetAnimatorParamerer("Shoot_b", true);
        }

        private void StopShootAnimation()
        {
            playerVisual.SetAnimatorParamerer("Shoot_b", true);
        }
        #endregion

        #region SerializeField

        [SerializeField]
        private WeaponInventory weaponInventory;

        #endregion

        #region Override
        public override void OnInputDisabled()
        {
            isPrevented = true;
        }

        public override void OnInputEnabled()
        {
            isPrevented = false;
        }

        public override void StopAbility()
        {
            StopShootAnimation();
        }
        #endregion

        #region Callbacks

        private void OnShootInputPerformed(InputAction.CallbackContext action)
        {
            if (!CanShoot()) return;
            playerController.AddRigidBodyForce(weaponInventory.ShootActiveWeapon(), ForceMode.Impulse);

            StartShootAnimation();
        }

        private void OnWeaponSwapInputPerformed(InputAction.CallbackContext action)
        {
            if (!CanShoot()) return;
            weaponInventory.ChangeWeapon();
        }

        private void OnReloadInputPerformed(InputAction.CallbackContext action)
        {
            if (!CanShoot()) return;
            weaponInventory.ReloadActiveWeapon();
        }

        #endregion

        #region InternalMethods

        protected bool CanShoot()
        {
            return !isPrevented;
        }

        #endregion
    }
}

