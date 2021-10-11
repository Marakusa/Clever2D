using System;
using System.IO;
using Clever2D.Core;
using Newtonsoft.Json;
using SDL2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
        internal string path;
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

        private float opacity = 1f;
        public float Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                Opacity = value;

                if (OnChanged != null)
                    OnChanged.Invoke(this);
            }
        }

        /// <summary>
        /// When this Sprite changes or is removed, this will get called.
        /// </summary>
        public delegate void SpriteChanged(object sender);
        /// <summary>
        /// When this Sprite changes or is removed, this will get called.
        /// </summary>
        public event SpriteChanged OnChanged;

        internal void InitializeSprite()
        {
            if (path != null && path.Trim() != "")
            {
                string resourceName = path;
                
                if (!path.StartsWith("/") && (path.Substring(1, 2) != ":/" || path.Substring(1, 2) != @":\"))
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

                    AssetLoader.AddAsset(resourceName, this);
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
        [JsonConstructor]
        public Sprite()
        {
            if (path != null && path.Trim() != "")
            {
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
            this.path = path;
            this.pivot = pivot;
            Image spriteImage = Image.Load(path);
            this.size = new(spriteImage.Width, spriteImage.Height);
            this.offset = Vector2.Zero;

            InitializeSprite();
        }
        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path, Vector2 pivot, Vector2Int size)
        {
            this.path = path;
            this.pivot = pivot;
            this.size = size;
            this.offset = Vector2.Zero;

            InitializeSprite();
        }
        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path, Vector2 pivot, Vector2Int size, Vector2 offset)
        {
            this.path = path;
            this.pivot = pivot;
            this.size = size;
            this.offset = offset;

            InitializeSprite();
        }

        private void LoadSprite(int sizeX, int sizeY, int offsetX, int offsetY)
        {
            if (path != null)
            {
                rect.x = offsetX;
                rect.y = offsetY;
                rect.w = sizeX;
                rect.h = sizeY;
                
                object asset = AssetLoader.GetAsset(path + "?color:" + Opacity.GetHashCode().ToString());

                if (asset == null || asset.GetType() != typeof(IntPtr))
                {
                    string tempFolder = $"{Application.TempDirectory}/image/";

                    if (!Directory.Exists(tempFolder))
                        Directory.CreateDirectory(tempFolder);

                    string cachePath = $"{tempFolder}{Cryptography.HashSHA1(System.IO.Path.GetFileNameWithoutExtension(path) + Opacity.GetHashCode().ToString())}{System.IO.Path.GetExtension(path)}";
                    OpacityFilter(AssetLoader.GetAsset($"{path}:Image") as Image, Opacity)
                        .Save(cachePath);

                    this.image = SDL_image.IMG_LoadTexture(Clever.Renderer, cachePath);

                    Clever.Destroying += () => SDL.SDL_DestroyTexture(this.image);

                    Player.Log($"Sprite {path} ==> Loaded.");

                    AssetLoader.AddAsset($"{path}?color:{Opacity.GetHashCode()}", this.image);
                }
                else
                {
                    this.image = (IntPtr)asset;
                }

                if (OnChanged != null)
                    OnChanged.Invoke(this);
            }
            else
            {
                Player.LogError("Sprites given path was not set.", new NullReferenceException());
            }
        }

        /// <summary>
        /// Sets opacity of the image.
        /// </summary>
        /// <param name="opacity">Opacity filter opacity.</param>
        public void SetOpacity(float opacity)
        {
            opacity = CMath.Clamp(opacity, 0f, 1f);
            Opacity = opacity;
            LoadSprite(rect.w, rect.h, rect.x, rect.y);
        }

        /// <summary>
        /// Sets opacity of the image.
        /// </summary>
        /// <param name="image">Image to add a filter into.</param>
        /// <param name="opacity">Opacity filter opacity.</param>
        private Image OpacityFilter(Image image, float opacity)
        {
            opacity = CMath.Clamp(opacity, 0f, 1f);
            image.Mutate(x => x.Opacity(opacity));
            return image;
        }
    }
}
