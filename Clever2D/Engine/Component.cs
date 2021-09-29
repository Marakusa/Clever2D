using System;

namespace Clever2D.Engine
{
    /// <summary>
    /// Base class for everything attached to GameObjects.
    /// </summary>
    public abstract class Component
    {
        public string name;
        public GameObject gameObject;
        public Transform transform;

        /// <summary>
        /// The name of the object.
        /// </summary>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Removes a GameObject, component or asset.
        /// </summary>
        public static void Destroy(Component component)
        {
            if (component.gameObject != null)
            {
                GameObject.Destroy(component.gameObject);
            }
            else
            {
                throw new Exception("Cannot destroy null. There was no GameObject attached to the given component.");
            }
        }
    }
}
