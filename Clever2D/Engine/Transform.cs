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
        public Vector2 Position
        {
            get;
            set;
        }
        /// <summary>
        /// Rotation of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 Rotation
        {
            get;
            set;
        }
        /// <summary>
        /// Scale of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 Scale
        {
            get;
            set;
        }
        
        // TODO: localPosition
        
        /// <summary>
        /// Component that handles the GameObjects position, rotation and scale in the Scene.
        /// </summary>
        public Transform()
        {
            Position = Vector2.Zero;
            Rotation = Vector2.Zero;
            Scale = Vector2.One;
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
