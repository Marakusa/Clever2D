using System;

namespace Clever2D.Engine
{
    /// <summary>
    /// Base class for everything attached to GameObjects.
    /// </summary>
    public abstract class Component : IDisposable
    {
        /// <summary>
        /// The GameObject where this component is assigned.
        /// </summary>
        public GameObject gameObject;
        /// <summary>
        /// The Transform component attached to this GameObject.
        /// </summary>
        public Transform transform;

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
                Player.LogError("Cannot destroy null. There was no GameObject attached to the given component.", new NullReferenceException());
            }
        }

        internal bool isInitialized = false;
        internal abstract void Initialize();

        private bool disposed;

        /// <summary>
        /// Disposes and destroys this Component.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Disposes this Component.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
        }
    }
}
