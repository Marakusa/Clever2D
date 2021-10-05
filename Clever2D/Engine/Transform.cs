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
        private Vector2 position = Vector2.zero;
        private Vector2 rotation = Vector2.zero;
        private Vector2 scale = Vector2.one;

        /// <summary>
        /// Position of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        /// <summary>
        /// Rotation of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        /// <summary>
        /// Scale of the GameObject this Component is assigned to.
        /// </summary>
        public Vector2 Scale 
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }
        /*{
            get
            {
                if (gameObject != null)
                {
                    if (gameObject.parent == null)
                        return position;
                    else
                    {
                        return localPosition + gameObject.parent.transform.position;
                    }
                }
                else
                {
                    return Vector2.zero;
                }
            }
            set
            {
                if (gameObject != null)
                {
                    if (gameObject.parent == null)
                        localPosition = value;
                    else
                    {
                        localPosition = gameObject.parent.transform.position + value;
                    }
                }
            }
        }*/
        /*/// <summary>
        /// Local position of the GameObject relative to the parent GameObject.
        /// </summary>
        public Vector2 localPosition = Vector2.zero;*/

        /// <summary>
        /// Component that handles the GameObjects position, rotation and scale in the Scene.
        /// </summary>
        public Transform()
        {
            position = Vector2.zero;
            rotation = Vector2.zero;
            scale = Vector2.one;
        }

        internal override void Initialize()
        {
        }
    }
}
