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
        internal static readonly List<KeyboardEvent> KeysPressed = new();

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
            if (KeysPressed.Find(k => k.keyEvent.keysym.sym == e.keysym.sym) == null)
            {
                KeysPressed.Add(new(e, Time.TotalTime));
            }
        }
        /// <summary>
        /// Player releases a key.
        /// </summary>
        private void KeyUp(SDL.SDL_KeyboardEvent e)
        {
            KeyboardEvent key = KeysPressed.Find(k => k.keyEvent.keysym.sym == e.keysym.sym);
            if (key != null)
            {
                KeysPressed.Remove(key);
            }
        }
    }

    /// <summary>
    /// Keyboard event for InputManager.
    /// </summary>
    public class KeyboardEvent
    {
        /// <summary>
        /// Event key.
        /// </summary>
        public readonly SDL.SDL_Keycode key;
        /// <summary>
        /// Event key event.
        /// </summary>
        public readonly SDL.SDL_KeyboardEvent keyEvent;
        /// <summary>
        /// Event time.
        /// </summary>
        public readonly float time;
        
        /// <summary>
        /// Keyboard event for InputManager.
        /// </summary>
        public KeyboardEvent(SDL.SDL_KeyboardEvent keyEvent, float time)
        {
            key = keyEvent.keysym.sym;
            this.keyEvent = keyEvent;
            this.time = time;
        }
    }
}
