using Clever2D.Engine;
using Clever2D.Input;
using System;

namespace Example
{
    public class PlayerController : CleverScript
    {
        private AnimatorController animator;
        public float speed = 1f;

        protected override void Start()
        {
            animator = gameObject.GetComponent<AnimatorController>();
        }

        protected override void FixedUpdate()
        {
            Vector2 move = Vector2.Zero;

            if (Input.GetButton("up"))
            {
                move += Vector2.Up;
            }
            if (Input.GetButton("right"))
            {
                move += Vector2.Right;
            }
            if (Input.GetButton("down"))
            {
                move += Vector2.Down;
            }
            if (Input.GetButton("left"))
            {
                move += Vector2.Left;
            }
            
            Vector2 startPosition = transform.position;
            Vector2 movedPosition = transform.position + move.Normalized * speed * 50f * Time.DeltaTime;
            
            Vector2 direction = (movedPosition - startPosition).Normalized;
            Vector2Int animDirection = new((int)Math.Round(direction.x), (int)Math.Round(direction.y));

            int d;

            if (animDirection.y != 0)
                d = animDirection.y > 0 ? 1 : 3;
            else if (animDirection.y == 0 && animDirection.x == 0)
                d = 0;
            else
                d = animDirection.x > 0 ? 2 : 4;
            
            animator.SetInt("Direction", d);

            transform.position = movedPosition;

            Camera.MainCamera.gameObject.transform.position = gameObject.transform.position;
        }
    }
}
