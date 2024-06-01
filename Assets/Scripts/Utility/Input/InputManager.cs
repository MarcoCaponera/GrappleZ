using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
