using System;
using System.IO;
using Clever2D.Core;
using SDL2;
using SixLabors.ImageSharp;

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
        /// Sprite pivot (scales from 0 to 1 per dimension).
        /// </summary>
        public Vector2 pivot = Vector2.zero;

        /// <summary>
        /// Path to the source of the image of this Sprite.
        /// </summary>
        private readonly string path;
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
        public Sprite(string path, Vector2 pivot)
        {
            path = Clever.executableDirectory + "/assets/" + path;

            if (File.Exists(path))
            {
                this.path = path;
                this.pivot = pivot;
                
                Image image = Image.Load(path);
                int width = image.Width;
                int height = image.Height;
                
                LoadSprite(width, height, 0, 0);
            }
            else
            {
                Player.LogError("File doesn't exist in " + path);
            }
        }
        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path, Vector2 pivot, Vector2Int size)
        {
            path = Clever.executableDirectory + "/assets/" + path;

            if (File.Exists(path))
            {
                this.path = path;
                this.pivot = pivot;

                Image image = Image.Load(path);
                int width = image.Width;
                int height = image.Height;

                int w = size.x < 0 ? width : size.x;
                int h = size.y < 0 ? height : size.y;
                
                LoadSprite(w, h, 0, 0);
            }
            else
            {
                Player.LogError("File doesn't exist in " + path);
            }
        }
        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        /// <param name="offset">Offset of the rendering area of the texture.</param>
        public Sprite(string path, Vector2 pivot, Vector2Int size, Vector2 offset)
        {
            path = Clever.executableDirectory + "/assets/" + path;

            if (File.Exists(path))
            {
                this.path = path;
                this.pivot = pivot;

                Image image = Image.Load(path);
                int width = image.Width;
                int height = image.Height;

                int w = size.x < 0 ? width : size.x;
                int h = size.y < 0 ? height : size.y;
                
                LoadSprite(w, h, (int) Math.Round(offset.x), (int) Math.Round(offset.y));
            }
            else
            {
                Player.LogError("File doesn't exist in " + path);
            }
        }

        private void LoadSprite(int sizeX, int sizeY, int offsetX, int offsetY)
        {
            rect.x = offsetX;
            rect.y = offsetY;
            rect.w = sizeX;
            rect.h = sizeY;

            this.image = SDL_image.IMG_LoadTexture(Clever.Renderer, this.path);

            Clever.Destroying += () => { SDL.SDL_DestroyTexture(this.image); };
        }
    }
}
