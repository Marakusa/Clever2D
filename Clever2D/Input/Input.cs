using Clever2D.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Clever2D.Input
{
    /// <summary>
    /// The base class that handles all Input in the Clever game.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Returns a bool value if the given key is pressed at the moment.
        /// </summary>
        /// <param name="key">Key to be checked for input.</param>
        public static bool GetKey(SDL.SDL_Keycode key)
        {
            return InputManager.keysPressed.Find(k => k.key == key) != null;
        }
    }
}
