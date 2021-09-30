using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public class Transform : Component
    {
        public Vector2 position = Vector2.zero;
        public Vector2 rotation = Vector2.zero;
        public Vector2 scale = Vector2.one;

        public Transform()
        {
            position = Vector2.zero;
            rotation = Vector2.zero;
            scale = Vector2.one;
        }
    }
}
