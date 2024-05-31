using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public class PlayerController : MonoBehaviour
    {
        #region References

        [SerializeField]
        protected Transform playerTransform;
        [SerializeField]
        protected Rigidbody playerRigidBody;
        [SerializeField]
        protected Collider playerPhysicsCollider;
        [SerializeField]
        protected Transform cameraPosition;

        #endregion

        #region ReferenceGetter

        public Transform PlayerTransform
        {
            get 
            {
                return playerTransform;
            }
        }

        public Transform CameraPosition
        {
            get
            {
                return cameraPosition;
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

        #region Abilities

        //to implement

        #endregion 

        #region Mono

        private void Awake()
        {
            //abilites setup
        }

        #endregion

        #region RigidBodyMethods

        //used to centralize rigidbody usage around the project

        public Vector2 GetVelocity()
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

        #region DebugMethods

        //to implement

        #endregion
    }
}
