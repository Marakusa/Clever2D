using System;
using System.Collections.Generic;
using Clever2D.Core;
using Clever2D.Engine;
using SDL2;

namespace Clever2D.Input
{
    /// <summary>
    /// Manages all the input.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// List of all keys pressed at the moment.
        /// </summary>
        internal static List<KeyboardEvent> keysPressed = new();

        /// <summary>
        /// Initializes the InputManager.
        /// </summary>
        public void Initialize()
        {
            Clever.OnKeyPress += KeyDown;
            Clever.OnKeyRelease += KeyUp;
        }

        /// <summary>
        /// Player presses a key.
        /// </summary>
        private void KeyDown(SDL.SDL_KeyboardEvent e)
        {
            if (keysPressed.Find(k => k.keyEvent.keysym.sym == e.keysym.sym) == null)
            {
                keysPressed.Add(new KeyboardEvent(e, Time.TotalTime));
            }
        }
        /// <summary>
        /// Player releases a key.
        /// </summary>
        private void KeyUp(SDL.SDL_KeyboardEvent e)
        {
            KeyboardEvent key = keysPressed.Find(k => k.keyEvent.keysym.sym == e.keysym.sym);
            if (key != null)
            {
                keysPressed.Remove(key);
            }
        }
    }

    public class KeyboardEvent
    {
        public SDL.SDL_Keycode key;
        public SDL.SDL_KeyboardEvent keyEvent;
        public float time;
        
        public KeyboardEvent(SDL.SDL_KeyboardEvent keyEvent, float time)
        {
            key = keyEvent.keysym.sym;
            this.keyEvent = keyEvent;
            this.time = time;
        }
    }
}
