using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public class PlayerVisual : MonoBehaviour
    {
        #region References

        [SerializeField]
        protected MeshFilter playerMeshFilter;
        [SerializeField]
        protected MeshRenderer playerMainRenderer;
        [SerializeField]
        protected Animator playerAnimator;

        #endregion

        #region AnimatorMethods

        //centralize animator management

        public void SetAnimatorParameter(string name)
        {
            playerAnimator.SetTrigger(Animator.StringToHash(name));
        }

        public void SetAnimatorParamerer(string name, bool value)
        {
            playerAnimator.SetBool(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter(string name, float value)
        {
            playerAnimator.SetFloat(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter(string name, int value)
        {
            playerAnimator.SetInteger(Animator.StringToHash(name), value);
        }

        #endregion

        #region VisualMethods

        public void SetMesh(Mesh mesh)
        {
            playerMeshFilter.mesh = mesh;
        }

        #endregion
    }

}
