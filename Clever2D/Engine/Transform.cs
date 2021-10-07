using System;

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
        public Vector2 position;
        /// <summary>
        /// Rotation of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 rotation;
        /// <summary>
        /// Scale of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 scale;
        
        // TODO: localPosition
        
        /// <summary>
        /// Component that handles the GameObjects position, rotation and scale in the Scene.
        /// </summary>
        public Transform()
        {
            position = Vector2.Zero;
            rotation = Vector2.Zero;
            scale = Vector2.One;
        }

        internal override void Initialize() { }

        /// <summary>
        /// Disposes and destroys this Component.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
