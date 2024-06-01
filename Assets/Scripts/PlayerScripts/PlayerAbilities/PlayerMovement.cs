using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Utility;
using Codice.Client.Commands.TransformerRule;

namespace GrappleZ_Player
{
    public class PlayerMovement : PlayerAbilityBase
    {
        #region SerializeFields

        [SerializeField]
        private float groundSpeed;
        [SerializeField]
        private float maxAerialSpeed;
        [SerializeField]
        private float aerialAcceleration;

        #endregion

        #region ProtectedAttributes

        protected float sqMaxAerialSpeed;
        protected bool wasWalking;

        #endregion

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

        public override void Init(PlayerController playerController, PlayerVisual playerVisual)
        {
            base.Init(playerController, playerVisual);
            sqMaxAerialSpeed = maxAerialSpeed * maxAerialSpeed;
        }

        #endregion

        #region Mono

        protected void OnEnable()
        {
            wasWalking = false;
        }

        protected void FixedUpdate()
        {
            if (isPrevented) return;
            FillDirectionFromInput();
            if(playerController.MovementDirection != Vector2.zero)
            {
                Move();
            }
            HandleEvents();
        }

        #endregion

        #region InternalMethods

        protected void FillDirectionFromInput()
        {
            playerController.MovementDirection = InputManager.PlayerMove();
        }

        protected void GroundMove()
        {
            Vector3 forwardToUse = -Vector3.Cross(playerController.GroundImpactNormal, transform.right);
            Vector3 rightToUse = Vector3.Cross(playerController.GroundImpactNormal, transform.forward);
            Vector3 inputDirection = rightToUse * playerController.MovementDirection.x + forwardToUse * playerController.MovementDirection.y;
            Vector3 velocityVector = inputDirection.normalized * groundSpeed;
            playerController.SetVelocity(velocityVector);
        }

        protected void AerialMove()
        {
            Vector3 velocityVector = transform.right * playerController.MovementDirection.x + transform.forward * playerController.MovementDirection.y;
            velocityVector = velocityVector.normalized * aerialAcceleration + Vector3.up * playerController.GetVelocity().z;
            playerController.AddRigidBodyForce(velocityVector, ForceMode.Acceleration);
        }

        protected void Move()
        {
            if (playerController.IsGrounded)
            {
                GroundMove();
            }
            else
            {
                AerialMove();
            }
        }

        protected void HandleEvents()
        {
            bool isWalking = playerController.MovementDirection != Vector2.zero;
            if (wasWalking && !isWalking) 
            {
                playerController.OnWalkEnded?.Invoke();
            }
            if (!wasWalking && isWalking)
            {
                playerController.OnWalkStarted?.Invoke();
            }
            wasWalking = isWalking;
        }
        #endregion
    }
}
