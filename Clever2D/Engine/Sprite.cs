using System;
using Gtk;

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
        public Image image;

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
            this.path = Environment.CurrentDirectory + @"\" + path;
            this.image = new Image(Environment.CurrentDirectory + @"\" + path);
        }
    }
}
