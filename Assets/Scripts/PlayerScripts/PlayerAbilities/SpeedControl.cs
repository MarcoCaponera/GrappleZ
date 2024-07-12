using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public class SpeedControl : PlayerAbilityBase
    {
        #region SerializeFields

        [SerializeField]
        private float maxAerialSpeed;
        [SerializeField]
        private float maxGroundSpeed;
        [SerializeField]
        [Tooltip("Speed at which something could happen (visual effects like air)")]
        private float eventSpeed;

        #endregion

        #region PrivateAttributes

        private float sqMaxAerialSpeed;
        private float sqMaxGroundSpeed;

        #endregion

        #region Mono

        public void FixedUpdate()
        {
            if(playerController.IsGrounded)
            {
                ControlGroundSpeed();
            }
            else
            {
                ControlAerialSpeed();
            }
        }

        #endregion

        #region InternalMethods

        protected void CanControl()
        {
            
        }

        protected void ControlGroundSpeed()
        {
            if(playerController.GetVelocity().sqrMagnitude > sqMaxGroundSpeed)
            {
                playerController.SetVelocity(playerController.GetVelocity().normalized * maxGroundSpeed);
            }
        }

        protected void ControlAerialSpeed()
        {
            if (playerController.GetVelocity().sqrMagnitude > sqMaxAerialSpeed)
            {
                playerController.SetVelocity(playerController.GetVelocity().normalized * maxAerialSpeed);
            }
        }

        #endregion


        #region Override

        public override void Init(PlayerController playerController, PlayerVisual playerVisual)
        {
            base.Init(playerController, playerVisual);
            sqMaxAerialSpeed = maxAerialSpeed * maxAerialSpeed;
            sqMaxGroundSpeed = maxGroundSpeed * maxGroundSpeed;
        }

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
    }
}
