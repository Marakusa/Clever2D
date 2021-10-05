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
                d = 1;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_a))
            {
                move += Vector2.left;
                d = 4;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_s))
            {
                move += Vector2.down;
                d = 2;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_d))
            {
                move += Vector2.right;
                d = 1;
            }

            animator.SetInt("Direction", d);

            transform.position += move.Normalized * speed * 50f * Time.DeltaTime;
        }
    }
}
