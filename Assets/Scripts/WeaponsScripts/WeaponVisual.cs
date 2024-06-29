using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Weapons
{
    public class WeaponVisual : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private MeshFilter mesh;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private MeshRenderer muzzleFlashRenderer;

        #endregion

        #region PrivateAtt

        private float muzzleFlashDuration = 0.1f;
        private Coroutine muzzleFlashCoroutine;

        #endregion

        #region AnimatorMethods

        public void SetAnimatorParameter(string name)
        {
            animator.SetTrigger(Animator.StringToHash(name));
        }

        public void SetAnimatorParamerer(string name, bool value)
        {
            animator.SetBool(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter(string name, float value)
        {
            animator.SetFloat(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter(string name, int value)
        {
            animator.SetInteger(Animator.StringToHash(name), value);
        }

        #endregion

        #region MeshRendererMethods

        public void SetRendererActive(bool value)
        {
            meshRenderer.enabled = value;
        }

        public void SetMuzzleFlashActive(bool value)
        {
            muzzleFlashRenderer.enabled = value;
            StartCoroutine(MuzzleFlashCoroutine());
        }

        #endregion

        #region Coroutine

        protected IEnumerator MuzzleFlashCoroutine()
        {
            yield return new WaitForSeconds(muzzleFlashDuration);
            SetMuzzleFlashActive(false);
            StopCoroutine(MuzzleFlashCoroutine());
        }

        #endregion
    }
}
