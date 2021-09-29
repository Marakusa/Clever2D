using System.Timers;

namespace Clever2D.Engine
{
    public abstract class CleverScript : Component
    {
        internal Timer timer;

        public virtual void Start() { }
        public virtual void FixedUpdate() { }
    }
}
