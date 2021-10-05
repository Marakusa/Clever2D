using Clever2D.Engine;
using Clever2D.Input;
using System;
using SDL2;

namespace Example
{
    public class PlayerController : CleverScript
    {
        public AnimatorController animator;
        public float speed = 1f;

        public override void Start()
        {
            animator = gameObject.GetComponent<AnimatorController>();
        }

        public override void FixedUpdate()
        {
            Vector2 move = Vector2.zero;

            int d = 0;

            if (Input.GetKey(SDL.SDL_Keycode.SDLK_w))
            {
                move += Vector2.up;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_d))
            {
                move += Vector2.right;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_s))
            {
                move += Vector2.down;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_a))
            {
                move += Vector2.left;
            }
            
            Vector2 startPosition = transform.position;
            Vector2 movedPosition = transform.position + move.Normalized * speed * 50f * Time.DeltaTime;
            
            Vector2 direction = (movedPosition - startPosition).Normalized;
            Vector2Int animDirection = new Vector2Int((int)Math.Round(direction.x), (int)Math.Round(direction.y));

            if (animDirection.y != 0)
                d = animDirection.y > 0 ? 1 : 3;
            else if (animDirection.y == 0 && animDirection.x == 0)
                d = 0;
            else
                d = animDirection.x > 0 ? 2 : 4;
            
            animator.SetInt("Direction", d);

            transform.position = movedPosition;
        }
    }
}
