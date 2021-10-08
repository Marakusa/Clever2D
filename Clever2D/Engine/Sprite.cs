using System;
using System.IO;
using Clever2D.Core;
using Newtonsoft.Json;
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
        [JsonProperty]
        public SDL.SDL_Rect rect;
        /// <summary>
        /// Sprite pivot (scales from 0 to 1 per dimension).
        /// </summary>
        [JsonProperty]
        public Vector2 pivot = Vector2.Zero;

        /// <summary>
        /// Path to the source of the image of this Sprite.
        /// </summary>
        [JsonProperty]
        private readonly string path;
        /// <summary>
        /// Returns the path to the source of the image of this Sprite.
        /// </summary>
        public string Path => path;
        
        /// <summary>
        /// Size of this Sprite.
        /// </summary>
        [JsonProperty]
        public Vector2Int size;

        /// <summary>
        /// Offset of this Sprite.
        /// </summary>
        [JsonProperty]
        public Vector2 offset;

        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        [JsonConstructor]
        public Sprite()
        {
            if (path != null && path.Trim() != "")
            {
                path = $"{Application.ExecutableDirectory}/assets/{path}";

                object asset = AssetLoader.GetAsset(path + ":Image");

                if (!Directory.Exists(path) && (asset != null || File.Exists(path)))
                {
                    var width = 0;
                    var height = 0;

                    if (asset == null || asset.GetType() != typeof(Image))
                    {
                        Image spriteImage = Image.Load(path);
                        width = spriteImage.Width;
                        height = spriteImage.Height;
                        AssetLoader.AddAsset(path + ":Image", spriteImage);
                    }
                    else
                    {
                        width = ((Image)asset).Width;
                        height = ((Image)asset).Height;
                    }

                    var w = size.x < 0 ? width : size.x;
                    var h = size.y < 0 ? height : size.y;

                    LoadSprite(w, h, (int)Math.Round(offset.x), (int)Math.Round(offset.y));
                }
                else
                {
                    Player.LogError($"File doesn't exist in {path} or the path is a directory.");
                }
            }
        }
        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path, Vector2 pivot)
        {
            if (path != null && path.Trim() != "")
            {
                path = $"{Application.ExecutableDirectory}/assets/{path}";

                object asset = AssetLoader.GetAsset(path + ":Image");

                if (!Directory.Exists(path) && (asset != null || File.Exists(path)))
                {
                    this.path = path;
                    this.pivot = pivot;

                    var width = 0;
                    var height = 0;

                    if (asset == null || asset.GetType() != typeof(Image))
                    {
                        Image spriteImage = Image.Load(path);
                        width = spriteImage.Width;
                        height = spriteImage.Height;
                        AssetLoader.AddAsset(path + ":Image", spriteImage);
                    }
                    else
                    {
                        width = ((Image)asset).Width;
                        height = ((Image)asset).Height;
                    }

                    LoadSprite(width, height, 0, 0);
                }
                else
                {
                    Player.LogError($"File doesn't exist in {path} or the path is a directory.");
                }
            }
        }
        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path, Vector2 pivot, Vector2Int size)
        {
            if (path != null && path.Trim() != "")
            {
                path = $"{Application.ExecutableDirectory}/assets/{path}";

                object asset = AssetLoader.GetAsset(path + ":Image");

                if (!Directory.Exists(path) && (asset != null || File.Exists(path)))
                {
                    this.path = path;
                    this.pivot = pivot;

                    var width = 0;
                    var height = 0;

                    if (asset == null || asset.GetType() != typeof(Image))
                    {
                        Image spriteImage = Image.Load(path);
                        width = spriteImage.Width;
                        height = spriteImage.Height;
                        AssetLoader.AddAsset(path + ":Image", spriteImage);
                    }
                    else
                    {
                        width = ((Image)asset).Width;
                        height = ((Image)asset).Height;
                    }

                    var w = size.x < 0 ? width : size.x;
                    var h = size.y < 0 ? height : size.y;

                    LoadSprite(w, h, 0, 0);
                }
                else
                {
                    Player.LogError($"File doesn't exist in {path} or the path is a directory.");
                }
            }
        }
        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path, Vector2 pivot, Vector2Int size, Vector2 offset)
        {
            if (path != null && path.Trim() != "")
            {
                string resourceName = path;
                path = $"{Application.ExecutableDirectory}/assets/{path}";

                object asset = AssetLoader.GetAsset(path + ":Image");

                if (!Directory.Exists(path) && (asset != null || File.Exists(path)))
                {
                    this.path = path;
                    this.pivot = pivot;

                    var width = 0;
                    var height = 0;

                    if (asset == null || asset.GetType() != typeof(Image))
                    {
                        Image spriteImage = Image.Load(path);
                        width = spriteImage.Width;
                        height = spriteImage.Height;
                        AssetLoader.AddAsset(path + ":Image", spriteImage);
                    }
                    else
                    {
                        width = ((Image)asset).Width;
                        height = ((Image)asset).Height;
                    }

                    var w = size.x < 0 ? width : size.x;
                    var h = size.y < 0 ? height : size.y;

                    LoadSprite(w, h, (int)Math.Round(offset.x), (int)Math.Round(offset.y));

                    AssetLoader.AddAsset(resourceName, this);
                }
                else
                {
                    Player.LogError($"File doesn't exist in {path} or the path is a directory.");
                }
            }
        }

        private void LoadSprite(int sizeX, int sizeY, int offsetX, int offsetY)
        {
            rect.x = offsetX;
            rect.y = offsetY;
            rect.w = sizeX;
            rect.h = sizeY;

            object asset = AssetLoader.GetAsset(path);

            if (asset == null || asset.GetType() != typeof(IntPtr))
            {
                this.image = SDL_image.IMG_LoadTexture(Clever.Renderer, this.path);

                Clever.Destroying += () => SDL.SDL_DestroyTexture(this.image);

                Player.Log($"Sprite {path.Substring($"{Application.ExecutableDirectory}/".Length)} ==> Loaded.");

                AssetLoader.AddAsset(path, this.image);
            }
            else
            {
                this.image = ((IntPtr)asset);
            }
        }

        /// <summary>
        /// Returns a copy of this Sprite.
        /// </summary>
        public Sprite Copy()
        {
            return new Sprite(path, pivot, size, offset);
        }
    }
}
