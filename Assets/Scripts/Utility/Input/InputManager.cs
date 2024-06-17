using System;
using System.Collections;
using System.Collections.Generic;
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

        public static void ManageShootSubscription(Action<InputAction.CallbackContext> action, bool add)
        {
            if (add)
            {
                input.Player.Shoot.performed += action;
                return;
            }
            input.Player.Shoot.performed -= action;
        }

        public static void ManageWeaponSwapSubscription(Action<InputAction.CallbackContext> action, bool add)
        {
            if (add)
            {
                input.Player.SwapWeapon.performed += action;
                return;
            }
            input.Player.SwapWeapon.performed -= action;
        }

        public static void ManageReloadSubscription(Action<InputAction.CallbackContext> action, bool add)
        {
            if (add)
            {
                input.Player.Reload.performed += action;
                return;
            }
            input.Player.Reload.performed -= action;
        }
    }
}
