using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GrappleZ_Player
{
    public class PlayerCollisionDetection : PlayerAbilityBase
    {
        #region Internal Classes
        public enum ColliderPointPosition
        {
            Center = 0,
            BottomCenter = 1,
            LeftCenter = 2,
            RightCenter = 3,
            ForwardCenter = 4,
            BackwardCenter = 5
        }

        [System.Serializable]
        protected class CollisionSphereData
        {
            [SerializeField]
            private ColliderPointPosition pointPosition;
            [SerializeField]
            private float sphereRadius;
            [SerializeField]
            private Vector3 offset;

            [Space]
            [Header("Editor only")]
            [SerializeField]
            private Color sphereColor;

            public ColliderPointPosition PointPosition
            {
                get { return pointPosition; }
            }
            public float SphereRadius
            {
                get { return sphereRadius; }
            }
            public Vector3 Offset
            {
                get { return offset; }
            }

            public Color SphereColor
            {
                get { return sphereColor; }
            }
        }
        #endregion

        #region SerializeFields

        [SerializeField]
        protected LayerMask groundLayer;
        [SerializeField]
        protected CollisionSphereData[] spheres;

        #endregion

        #region ProtectedProperties

        protected Collider LastGroundCollider
        {
            get
            {
                return playerController.LastGroundCollider;
            }
            set
            {
                playerController.LastGroundCollider = value;
            }
        }

        #endregion

        #region Override

        public override void OnInputDisabled()
        {
            throw new System.NotImplementedException();
        }

        public override void OnInputEnabled()
        {
            throw new System.NotImplementedException();
        }

        public override void StopAbility()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Mono

        protected void Update()
        {
            DetectGroundCollision();
        }

        protected void OnDrawGizmos()
        {
            if (playerController == null)
            {
                playerController = FindObjectOfType<PlayerController>();
            }
            DrawSphere();
        }

        #endregion

        #region InternalMethods

        protected Vector3 GetCenterPoint(ColliderPointPosition position)
        {
            CollisionSphereData data = System.Array.Find(spheres, spheres => spheres.PointPosition == position);
            Vector3 positionOffset = data == null ? Vector3.zero : data.Offset;
            Vector3 collisionPoint = Vector3.zero;

            if (playerController.PlayerPhysicsCollider == null) return collisionPoint + positionOffset;
            Vector3 playerCenter = playerController.PlayerPhysicsCollider.bounds.center;
            Vector3 playerExtents = playerController.PlayerPhysicsCollider.bounds.extents;

            switch (position)
            {
                case ColliderPointPosition.Center:
                    collisionPoint = playerCenter;
                    break;
                case ColliderPointPosition.BottomCenter:
                    collisionPoint = new Vector3(playerCenter.x, playerCenter.y - playerExtents.y, playerCenter.z);
                    break;
                case ColliderPointPosition.LeftCenter:
                    collisionPoint = new Vector3(playerCenter.x - playerExtents.x, playerCenter.y, playerCenter.z);
                    break;
                case ColliderPointPosition.RightCenter:
                    collisionPoint = new Vector3(playerCenter.x + playerExtents.x, playerCenter.y, playerCenter.z);
                    break;
                case ColliderPointPosition.ForwardCenter:
                    collisionPoint = new Vector3(playerCenter.x, playerCenter.y, playerCenter.z + playerExtents.z);
                    break;
                case ColliderPointPosition.BackwardCenter:
                    collisionPoint = new Vector3(playerCenter.x, playerCenter.y, playerCenter.z - playerExtents.z);
                    break;
            }
            return collisionPoint + positionOffset;
        }

        protected float GetSphereRadius(ColliderPointPosition position)
        {
            CollisionSphereData data = System.Array.Find(spheres, spheres => spheres.PointPosition == position);
            if (data == null) return -1;
            return data.SphereRadius;
        }

        protected void DrawSphere()
        {
            foreach (var sphere in spheres)
            {
                Gizmos.color = sphere.SphereColor;
                Vector3 point = GetCenterPoint(sphere.PointPosition);
                float radius = GetSphereRadius(sphere.PointPosition);
                Gizmos.DrawWireSphere(point, radius);
            }
        }

        private void DetectGroundCollision()
        {
            bool wasGrounded = playerController.IsGrounded;

            Vector3 collisionPoint = GetCenterPoint(ColliderPointPosition.BottomCenter);
            float sphereRadius = GetSphereRadius(ColliderPointPosition.BottomCenter);

            Collider[] colliders = Physics.OverlapSphere(collisionPoint, sphereRadius, groundLayer);
            if (colliders.Length > 0)
            {
                //temporary solution
                LastGroundCollider = colliders[0];
            }
            else
            {
                LastGroundCollider = null;
            }
            playerController.IsGrounded = LastGroundCollider != null;

            if (wasGrounded == playerController.IsGrounded) return;
            if (wasGrounded)
            {
                playerController.OnGroundReleased?.Invoke();
            }
            else
            {
                playerController.OnGroundLanded?.Invoke();
            }
        }

        #endregion
    }
}
