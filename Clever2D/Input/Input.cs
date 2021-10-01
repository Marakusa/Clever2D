using Clever2D.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Input
{
    /// <summary>
    /// The base class that handles all Input in the Clever game.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// List of all keys pressed at the moment.
        /// </summary>
        internal static List<string> keysPressed = new();

        /// <summary>
        /// Returns a bool value if the given key is pressed at the moment.
        /// </summary>
        /// <param name="key">Key to be checked for input.</param>
        public static bool GetKey(string key)
        {
            return keysPressed.Contains(key);
        }

        /// <summary>
        /// Sets the given key as pressed for input checks.
        /// </summary>
        /// <param name="key">Key that has been pressed.</param>
        public static void KeyPressed(string key)
        {
            if (!keysPressed.Contains(key))
            {
                keysPressed.Add(key);
                Player.Log("Key up: " + key);
                Player.LogWarn("Key up: " + key);
                Player.LogError("Null is yes");
                Player.LogError("Null is yes", new NullReferenceException());
            }
        }
        /// <summary>
        /// Sets the given key as released for input checks.
        /// </summary>
        /// <param name="key">Key that has been released.</param>
        public static void KeyReleased(string key)
        {
            if (keysPressed.Contains(key))
            {
                keysPressed.Remove(key);
            }
        }
    }
}
