using System.Timers;

namespace Clever2D.Engine
{
    /// <summary>
    /// CleverScript is the base class from which every Clever script delivers.
    /// </summary>
    public abstract class CleverScript : Component
    {
        /// <summary>
        /// A timer for Update functions.
        /// </summary>
        internal Timer timer;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any Update methods are called.
        /// </summary>
        public virtual void Start() { }
        /// <summary>
        /// Frame-rate independ Update method.
        /// </summary>
        public virtual void FixedUpdate() { }
        // TODO: Normal Update method
    }
}
