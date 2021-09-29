using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public abstract class CleverScript : Component
    {
        virtual public void Start() { }
        virtual public void Update() { }
    }
}
