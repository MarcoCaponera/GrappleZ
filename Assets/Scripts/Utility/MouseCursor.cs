using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GrappleZ_Utility
{
    public class MouseCursor : MonoBehaviour
    {
        #region Mono
        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion

        #region PublicMethods

        public void SetVisibility(bool visible)
        {
            Cursor.visible = visible;
        }

        public void SetLockState(CursorLockMode cursorLockMode)
        {
            Cursor.lockState = cursorLockMode;
        }

        #endregion
    }
}
