using System;
using Clever2D.Core;

namespace Clever2D.Engine
{
    /// <summary>
    /// CleverScript is the base class from which every Clever script delivers.
    /// </summary>
    public abstract class CleverScript : Component
    {
        internal override void Initialize()
        {
            Start();
            Clever.Update += Update;
            Clever.FixedUpdate += FixedUpdate;
        }
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any Update methods are called.
        /// </summary>
        protected virtual void Start() { }
        /// <summary>
        /// Update is called on every frame.
        /// </summary>
        protected virtual void Update() { }
        /// <summary>
        /// Frame-rate independ Update method.
        /// </summary>
        protected virtual void FixedUpdate() { }

        /// <summary>
        /// Disposes and destroys this Component.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            Clever.Update -= Update;
            Clever.FixedUpdate -= FixedUpdate;
            GC.SuppressFinalize(this);
        }
    }
}
