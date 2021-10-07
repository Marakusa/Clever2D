using SDL2;

namespace Clever2D.Input
{
    /// <summary>
    /// The base class that handles all Input in the Clever game.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Returns a bool value if the given key is pressed at the moment.
        /// </summary>
        /// <param name="key">Key to be checked for input.</param>
        public static bool GetKey(SDL.SDL_Keycode key)
        {
            return InputManager.KeysPressed.Find(k => k.key == key) != null;
        }
        
        /// <summary>
        /// Returns a bool value if the given key is pressed at the moment.
        /// </summary>
        /// <param name="button">Button name to be checked for input.</param>
        public static bool GetButton(string button)
        {
            switch (button)
            {
                case "up":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_UP) != null)
                        return true;
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_w) != null)
                        return true;
                    break;
                case "down":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_DOWN) != null)
                        return true;
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_s) != null)
                        return true;
                    break;
                case "right":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_RIGHT) != null)
                        return true;
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_d) != null)
                        return true;
                    break;
                case "left":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_LEFT) != null)
                        return true;
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_a) != null)
                        return true;
                    break;
                case "jump":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_SPACE) != null)
                        return true;
                    break;
                case "submit":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_RETURN) != null)
                        return true;
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_RETURN2) != null)
                        return true;
                    break;
                case "cancel":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_ESCAPE) != null)
                        return true;
                    break;
                case "action1":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_z) != null)
                        return true;
                    break;
                case "action2":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_x) != null)
                        return true;
                    break;
                case "action3":
                    if (InputManager.KeysPressed.Find(k => k.key == SDL.SDL_Keycode.SDLK_c) != null)
                        return true;
                    break;
            }
            
            return false;
        }
    }
}
