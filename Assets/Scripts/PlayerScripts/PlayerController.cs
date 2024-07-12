using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public class PlayerController : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        protected Transform playerTransform;
        [SerializeField]
        protected Rigidbody playerRigidBody;
        [SerializeField]
        protected Collider playerPhysicsCollider;
        [SerializeField]
        protected Transform cameraTransform;
        [SerializeField]
        protected PlayerVisual playerVisual;


        #endregion

        #region ReferenceGetter

        public Transform PlayerTransform
        {
            get 
            {
                return playerTransform;
            }
        }

        public Transform CameraTransform
        {
            get
            {
                return cameraTransform;
            }
        }

        public Collider PlayerPhysicsCollider
        {
            get
            {
                return playerPhysicsCollider;
            }
        }

        #endregion

        #region PrivateAttributes

        private PlayerAbilityBase[] abilities;

        #endregion

        #region AbilitiesParameters

        #region PlayerCollisionDetection

        public bool IsGrounded 
        { 
            get; 
            set; 
        }

        public Vector3 GroundImpactNormal
        {
            get;
            set;
        }

        public Collider LastGroundCollider
        {
            get;
            set;
        }

        public Action OnGroundLanded;
        public Action OnGroundReleased;

        #endregion

        #region PlayerMovement

        public Vector2 MovementDirection
        {
            get;
            set;
        }

        public Action OnWalkStarted;
        public Action OnWalkEnded;

        #endregion

        #region PlayerLook
        public float HorizontalRotation
        {
            get;
            set;
        }
        public float VerticalCameraRotation
        {
            get;
            set;
        }

        public Action OnCameraRotated;

        #endregion

        #region PlayerHook

        public bool IsHooking
        {
            get;
            set;
        }

        public Action OnHookLanded;
        public Action OnHookReleased;

        #endregion

        #region PlayerShoot

        public bool IsShooting
        {
            get;
            set;
        }

        #endregion

        #endregion



        #region Mono

        private void Awake()
        {

            abilities = GetComponentsInChildren<PlayerAbilityBase>();

            foreach(var ability in abilities)
            {
                ability.Init(this, playerVisual);
                ability.enabled = true;
            }
#if DEBUG
            InitDebugEvents();
#endif
        }

        private void Start()
        {
            
        }

        #endregion

        #region RigidBodyMethods

        //used to centralize rigidbody usage around the project

        public Vector3 GetVelocity()
        {
            return playerRigidBody.velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            playerRigidBody.velocity = velocity;
        }

        public void RemoveGravity()
        {
            playerRigidBody.useGravity = false;
        }

        public void RestoreGravity()
        {
            playerRigidBody.useGravity = true;
        }

        public void AddRigidBodyForce(Vector3 force, ForceMode forceMode)
        {
            playerRigidBody.AddForce(force, forceMode);
        }

        #endregion

        #region PrivateMethods

        private void DisableInput()
        {
            //disables input for all abilites
            foreach (var ability in abilities)
            {
                ability.OnInputDisabled();
            }
        }

        private void EnableInput()
        {
            //enables input for all abilites
            foreach (var ability in abilities)
            {
                ability.OnInputEnabled();
            }
        }

        #endregion

        #region HealthModule
        private bool isDead;
        public Action<DamageContainer> OnDamageTaken;
        public Action OnDeath;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set
            {
                isDead = value;
                //playerVisual.SetAnimatorParameter(isDeadAnimatorParameter, value); TO ADD ANIMATIONS
            }
        }
        #endregion

        #region DebugMethods

        //to implement
        private void InitDebugEvents()
        {

            //OnGroundLanded += () => { Debug.Log("OnGroundLanded"); };
            //OnGroundReleased += () => { Debug.Log("OnGroundReleased"); };
            //OnWalkStarted += () => { Debug.Log("OnWalkStarted"); };
            //OnWalkEnded += () => { Debug.Log("OnWalkEnded"); };
            //OnCameraRotated += () => { Debug.Log("OnCameraRotated (X " + HorizontalRotation + ": Y " + VerticalCameraRotation); };
        }

        #endregion
    }
}
