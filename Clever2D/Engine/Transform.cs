using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    /// <summary>
    /// Component that handles the GameObjects position, rotation and scale in the Scene.
    /// </summary>
    public class Transform : Component
    {
        /// <summary>
        /// Position of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 position = Vector2.zero;
        /// <summary>
        /// Rotation of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 rotation = Vector2.zero;
        /// <summary>
        /// Scale of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 scale = Vector2.one;

        /// <summary>
        /// Component that handles the GameObjects position, rotation and scale in the Scene.
        /// </summary>
        public Transform()
        {
            position = Vector2.zero;
            rotation = Vector2.zero;
            scale = Vector2.one;
        }
    }
}
