using GrappleZ_Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrappleZ_Utility;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

namespace Grapple_Player
{
    public class PlayerHook : PlayerAbilityBase
    {
        private const float angleThreshold = 90;

        #region SerializeFields

        [SerializeField]
        protected float hookRange;
        [SerializeField]
        protected float hookLaunchSpeed;
        [SerializeField]
        protected float hookReturnSpeed;
        [SerializeField]
        protected float hookPullForce;
        [SerializeField]
        [Tooltip("The initial force applied to make sure that the player will be lifted from ground")]
        protected float initialLaunchForce;
        [SerializeField]
        protected float collisionRadius;
        [SerializeField]
        protected float detachRadius;
        [SerializeField]
        protected LayerMask wallMask;
        [SerializeField]
        private LineRenderer lineRenderer;

        #endregion

        #region PrivateAttributes

        private Coroutine hookCoroutine;
        private HookState hookState;
        private float sqDetachRadius;
        private float sqHookRange;

        #endregion

        #region InternalClasses

        public enum HookState
        {
            Launch,
            Return,
            Pull,
            None
        }

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
            if (hookCoroutine != null) StopCoroutine(hookCoroutine);
            lineRenderer.enabled = false;
            playerController.OnHookReleased?.Invoke();
            playerController.IsHooking = false;
            hookState = HookState.None;
        }

        public override void Init(PlayerController playerController, PlayerVisual playerVisual)
        {
            base.Init(playerController, playerVisual);
            sqHookRange = hookRange * hookRange;
            sqDetachRadius = detachRadius * detachRadius;
        }

        #endregion

        #region Mono

        protected void OnEnable()
        {
            InputManager.ManageHookSubscription(OnInputPerformed, true);
            hookState = HookState.None;
        }

        protected void OnDisable()
        {
            InputManager.ManageHookSubscription(OnInputPerformed, false);
            hookState = HookState.None;
        }

        #endregion

        #region InternalMethods

        protected bool CanHook()
        {
            return !isPrevented && !playerController.IsHooking;
        }

        protected void StartHook()
        {
            hookCoroutine = StartCoroutine(HookCoroutine());
        }

        protected void CalculateNewVelocity(Vector3 pullPoint)
        {
            Vector3 pullVector = pullPoint - playerController.PlayerTransform.position;
            Vector3 velocityVectorNeg = playerController.GetVelocity();
            if (pullVector.sqrMagnitude <= (pullVector + velocityVectorNeg).sqrMagnitude) return;
            Vector3 pullVectorNorm = pullVector.normalized;
            Vector3 reflectedVector = Vector3.Reflect(velocityVectorNeg, pullVectorNorm);
            float perc = 1f;
            Vector3 sumVector = velocityVectorNeg + reflectedVector;
            Vector3 maxVector = velocityVectorNeg * 2;
            float maxLen = maxVector.sqrMagnitude;
            float sumLen = sumVector.sqrMagnitude;
            perc = sumLen / maxLen; 
            playerController.SetVelocity((reflectedVector * perc) + pullVectorNorm * hookPullForce);
        }

        protected void ApplyPull(Vector3 pullPoint)
        {
            Vector3 pullVectorNorm = (pullPoint - playerController.PlayerTransform.position).normalized;
            playerController.SetVelocity(playerController.GetVelocity() + pullVectorNorm * hookPullForce);
        }

        #endregion

        #region Callbacks

        protected void OnInputPerformed(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (playerController.IsHooking && hookState == HookState.Pull)
            {
                StopAbility();
                return;
            }
            if (!CanHook()) return;
            StartHook();
        }
        #endregion

        #region Coroutine

        protected IEnumerator HookCoroutine()
        {
            playerController.IsHooking = true;
            lineRenderer.enabled = true;
            Vector3 direction = playerController.CameraTransform.forward;
            float currentLength = 0;
            bool hit = false;
            Vector3 hookPosition = playerController.CameraTransform.position;
            Vector3 hitPoint = Vector2.zero;
            WaitForFixedUpdate wfFixedUpdate = new WaitForFixedUpdate();
            hookState = HookState.Launch;
            while (currentLength < sqHookRange)
            {
                hookPosition = hookPosition + hookLaunchSpeed * direction.normalized * Time.deltaTime;
                Collider[] hitColliders = Physics.OverlapSphere(hookPosition, collisionRadius, wallMask);

                if (hitColliders.Length > 0 ) 
                {
                    hit = true;
                    hitPoint = hitColliders[0].ClosestPoint(hookPosition);
                    break;
                }

                currentLength = (hookPosition - gameObject.transform.position).sqrMagnitude;
                lineRenderer.SetPosition(0, gameObject.transform.position);
                lineRenderer.SetPosition(1, hookPosition);
                yield return wfFixedUpdate;
            }
            if (hit)
            {
                //pull logic
                hookState = HookState.Pull;
                lineRenderer.SetPosition(1, hitPoint);
                playerController.OnHookLanded?.Invoke();
                CalculateNewVelocity(hitPoint);
                if (playerController.IsGrounded)
                {
                    playerController.AddRigidBodyForce(Vector3.up * initialLaunchForce, ForceMode.Impulse);
                }
                float distance = float.MaxValue;
                while(distance > sqDetachRadius)
                {
                    //CalculateNewVelocity(hitPoint);
                    ApplyPull(hitPoint);
                    direction = hitPoint - gameObject.transform.position;
                    distance = direction.sqrMagnitude;
                    lineRenderer.SetPosition(0, gameObject.transform.position);
                    yield return wfFixedUpdate;
                }
            }
            else
            {
                //return logic
                float distance = float.MaxValue;
                Vector3 returnDirection = gameObject.transform.position - hookPosition;
                hookState = HookState.Return;
                while (distance >= 2f)
                {
                    hookPosition = hookPosition + hookReturnSpeed * returnDirection.normalized * Time.deltaTime;
                    returnDirection = gameObject.transform.position - hookPosition; 
                    distance = returnDirection.sqrMagnitude;
                    lineRenderer.SetPosition(0, gameObject.transform.position);
                    lineRenderer.SetPosition(1, hookPosition);
                    yield return wfFixedUpdate;
                }
            }
            StopAbility();
        }

        #endregion
    }
}

