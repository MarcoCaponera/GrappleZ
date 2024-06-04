using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GrappleZ_Utility
{
    public static class InputManager
    {
        private static Inputs input;

        static InputManager()
        {
            input = new Inputs();
            input.Player.Enable();
        }

        public static Vector2 PlayerMove()
        {
            return input.Player.Move.ReadValue<Vector2>();
        }

        public static Vector2 PlayerLook()
        {
            return input.Player.Look.ReadValue<Vector2>();
        }

        public static void ManageHookSubscription(Action<InputAction.CallbackContext> action, bool add)
        {
            if (add)
            {
                input.Player.Hook.performed += action;
                return;
            }
            input.Player.Hook.performed -= action;
        }
    }
}
