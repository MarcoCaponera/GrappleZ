using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Bullets
{
    [CreateAssetMenu(fileName = "BulletTrailData", menuName = "Custom/Bullets/BulletTrailData")]
    public class BulletTrailData : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve widthCurve;
        [SerializeField]
        private float time;
        [SerializeField]
        private float minVertexDistance = 0.1f;
        [SerializeField]
        private Gradient colorGradient;
        [SerializeField]
        private Material material;
        [SerializeField]
        private int cornerVertices;
        [SerializeField]
        private int endCapVertices;

        public void SetupTrail(TrailRenderer renderer)
        {
            renderer.widthCurve = widthCurve;
            renderer.time = time;
            renderer.colorGradient = colorGradient;
            renderer.sharedMaterial = material;
            renderer.numCapVertices = endCapVertices;
            renderer.numCornerVertices = cornerVertices;
            renderer.minVertexDistance = minVertexDistance;
        }
    }
}
