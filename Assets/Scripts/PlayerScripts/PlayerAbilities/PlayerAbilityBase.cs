using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrappleZ_Player
{
    public abstract class PlayerAbilityBase : MonoBehaviour
    {
        #region References

        protected PlayerController playerController;
        protected PlayerVisual playerVisual;

        #endregion

        #region ProtectedMembers

        //prevents ability usage if true
        protected bool isPrevented;

        #endregion

        #region AbstractMethods
        //triggered when input enabled
        public abstract void OnInputEnabled();

        //triggered when input disabled
        public abstract void OnInputDisabled();

        //triggered when ability has to stop 
        public abstract void StopAbility();

        #endregion

        #region VirtualMethods

        public virtual void Init(PlayerController playerController, PlayerVisual playerVisual)
        {
            this.playerController = playerController;
            this.playerVisual = playerVisual;
        }

        #endregion
    }
}
