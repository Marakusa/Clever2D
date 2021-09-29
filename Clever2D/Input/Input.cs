using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Input
{
    public static class Input
    {
        internal static List<char> keysPressed = new();

        public static bool GetKey(char key)
        {
            return keysPressed.Contains(key);
        }

        public static void KeyPressed(char key)
        {
            if (!keysPressed.Contains(key))
                keysPressed.Add(key);
        }
        public static void KeyReleased(char key)
        {
            if (keysPressed.Contains(key))
                keysPressed.Remove(key);
        }
    }
}
