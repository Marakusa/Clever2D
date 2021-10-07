using System;
using System.Collections.Generic;
using System.IO;
using Clever2D.Core;
using Clever2D.Engine;
using Newtonsoft.Json;
using SDL2;
using SixLabors.ImageSharp;

namespace Example
{
    class Program : Clever
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            ApplicationConfig config = new()
            {
                ProductName = "Example Project",
                CompanyName = "Company",
                Version = "0.1.0"
            };

            Application.Config = config;

            OperatingSystem os = System.Environment.OSVersion;

            Clever.OnInitialized += () =>
            {
                Scene[] scenes = new Scene[] {
                    new MainScene()
                };

                //var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                //string scene = JsonConvert.SerializeObject(new Sprite("resources/DungeonTile_1.png", new Point(0, 0)), jset);

                //File.WriteAllText(Application.ExecutableDirectory + @"\MainScene.scene", scene);
                

                //string a = File.ReadAllText(Application.ExecutableDirectory + @"\MainScene.scene");

                //TestObj t = JsonConvert.DeserializeObject<TestObj>(a);
                //Console.WriteLine(t.yesList[0].name);
                //Console.WriteLine(t.yesList[1].name);

                Clever.Start(scenes);
            };

            Clever.Initialize(config);
        }
    }

    /*public class TestObj
    {
        public Yes[] yesList = new Yes[] {
            new Yeser()
            {
                name = "test",
                testint = 1230
            },
            new No()
            {
                name = "test",
                why = true
            }
        };
    }
    public abstract class Yes
    {
        public string name;
    }
    public class Yeser : Yes
    {
        public int testint = 0;
    }
    public class No : Yes
    {
        public bool why = true;
    }

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
        public Point pivot = new(0, 0);

        /// <summary>
        /// Path to the source of the image of this Sprite.
        /// </summary>
        private readonly string path;
        /// <summary>
        /// Returns the path to the source of the image of this Sprite.
        /// </summary>
        public string Path => path;

        /// <summary>
        /// Represents a Sprite object for use in 2D gameplay.
        /// </summary>
        public Sprite(string path, Point pivot)
        {
            path = $"{Application.ExecutableDirectory}/assets/{path}";

            if (File.Exists(path))
            {
                this.path = path;
                this.pivot = pivot;

                var spriteImage = Image.Load(path);
                var width = spriteImage.Width;
                var height = spriteImage.Height;

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
        public Sprite(string path, Point pivot, Vector2Int size)
        {
            path = $"{Application.ExecutableDirectory}/assets/{path}";

            if (File.Exists(path))
            {
                this.path = path;
                this.pivot = pivot;

                var spriteImage = Image.Load(path);
                var width = spriteImage.Width;
                var height = spriteImage.Height;

                var w = size.x < 0 ? width : size.x;
                var h = size.y < 0 ? height : size.y;

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
        public Sprite(string path, Point pivot, Vector2Int size, Vector2 offset)
        {
            path = $"{Application.ExecutableDirectory}/assets/{path}";

            if (File.Exists(path))
            {
                this.path = path;
                this.pivot = pivot;

                Image spriteImage = Image.Load(path);
                var width = spriteImage.Width;
                var height = spriteImage.Height;

                var w = size.x < 0 ? width : size.x;
                var h = size.y < 0 ? height : size.y;

                LoadSprite(w, h, (int)Math.Round(offset.x), (int)Math.Round(offset.y));
            }
            else
            {
                Player.LogError($"File doesn't exist in {path}");
            }
        }

        private void LoadSprite(int sizeX, int sizeY, int offsetX, int offsetY)
        {
            rect.x = offsetX;
            rect.y = offsetY;
            rect.w = sizeX;
            rect.h = sizeY;

            //this.image = SDL_image.IMG_LoadTexture(Clever.Renderer, this.path);

            Clever.Destroying += () => SDL.SDL_DestroyTexture(this.image);
        }
    }*/
}
