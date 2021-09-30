using Clever2D.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Input
{
    public static class Input
    {
        internal static List<string> keysPressed = new();

        public static bool GetKey(string key)
        {
            return keysPressed.Contains(key);
        }

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
        public static void KeyReleased(string key)
        {
            if (keysPressed.Contains(key))
            {
                keysPressed.Remove(key);
            }
        }
    }
}
