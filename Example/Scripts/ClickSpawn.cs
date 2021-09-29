using Clever2D.Engine;
using Clever2D.Input;
using System;

namespace Example
{
    public class ClickSpawn : CleverScript
    {
        public override void Start()
        {
            Console.WriteLine("Start");
        }
        public override void FixedUpdate()
        {
            if (Input.GetKey('d'))
            {
                transform.position += Vector2.right;
            }
        }
    }
}
