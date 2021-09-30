using Clever2D.Engine;
using Clever2D.Input;

namespace Example
{
    public class ClickSpawn : CleverScript
    {
        public float speed = 1f;

        public override void FixedUpdate()
        {
            Vector2 move = Vector2.zero;

            if (Input.GetKey("W"))
            {
                move += Vector2.up;
            }
            if (Input.GetKey("A"))
            {
                move += Vector2.left;
            }
            if (Input.GetKey("S"))
            {
                move += Vector2.down;
            }
            if (Input.GetKey("D"))
            {
                move += Vector2.right;
            }

            transform.position += move.Normalized * speed;
        }
    }
}
