using System;

using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace ProjectAlpha2
{
    public static class InputManager
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;

        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;

        public static KeyboardStateExtended GetKeyboardState()
        {
            return new KeyboardStateExtended(_currentKeyboardState, _previousKeyboardState);
        }

        public static MouseStateExtended GetMouseState()
        {
            return new MouseStateExtended(_currentMouseState, _previousMouseState);
        }
        public static void Refresh()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
    }
}