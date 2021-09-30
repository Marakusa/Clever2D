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

        public static void KeyPressed(string key, Action callback)
        {
            if (!keysPressed.Contains(key))
                keysPressed.Add(key);

            callback.Invoke();
        }
        public static void KeyReleased(string key, Action callback)
        {
            if (keysPressed.Contains(key))
                keysPressed.Remove(key);

            callback.Invoke();
        }
    }
}
