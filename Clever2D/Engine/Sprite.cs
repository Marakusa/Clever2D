using System;
using Eto.Drawing;

namespace Clever2D.Engine
{
    public class Sprite
    {
        public Image image;

        private readonly string path = "";
        public string Path
        {
            get
            {
                return path;
            }
        }

        public Sprite(string path)
        {
            this.path = Environment.CurrentDirectory + @"\" + path;
            this.image = new Bitmap(Environment.CurrentDirectory + @"\" + path);
        }
    }
}
