using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Drawing;

namespace Clever2D.Engine
{
    public class Sprite
    {
        public Image image;

        public Sprite(string path)
        {
            this.image = new Bitmap(Environment.CurrentDirectory + @"\" + path);
        }
    }
}
