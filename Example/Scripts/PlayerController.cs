using Clever2D.Engine;
using Clever2D.Input;
using System;
using SDL2;

namespace Example
{
    public class PlayerController : CleverScript
    {
        public float speed = 1f;

        public override void FixedUpdate()
        {
            Vector2 move = Vector2.zero;

            if (Input.GetKey(SDL.SDL_Keycode.SDLK_w))
            {
                move += Vector2.up;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_a))
            {
                move += Vector2.left;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_s))
            {
                move += Vector2.down;
            }
            if (Input.GetKey(SDL.SDL_Keycode.SDLK_d))
            {
                move += Vector2.right;
            }

            transform.position += move.Normalized * speed * 50f * Time.DeltaTime;
        }
    }
}
