using System;
using Clever2D.Core;
using SDL2;

namespace Clever2D.Engine
{
    /// <summary>
    /// Represents a Sprite object for use in 2D gameplay.
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// Image assigned to this Sprite.
        /// </summary>
        public IntPtr image;
        /// <summary>
        /// Rect of the assigned image to this object which contains the size of the texture.
        /// </summary>
        public SDL.SDL_Rect rect;

        /// <summary>
        /// Path to the source of the image of this Sprite.
        /// </summary>
        private readonly string path = "";
        /// <summary>
        /// Returns the path to the source of the image of this Sprite.
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
        }

        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path)
        {
            this.path = Environment.CurrentDirectory + "/Example/bin/Debug/net5.0-windows/assets/" + path;
            
            rect.x = 0;
            rect.y = 0;
            rect.w = 16;
            rect.h = 16;

            this.image = SDL_image.IMG_LoadTexture(Clever.Renderer, this.path);

            Clever.Destroying += () =>
            {
                SDL.SDL_DestroyTexture(this.image);
            };
        }
    }
}
