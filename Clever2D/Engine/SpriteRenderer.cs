using Eto.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public class SpriteRenderer : Component
    {
        private Sprite sprite;
        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
            }
        }
        public Image Image
        {
            get
            {
                return sprite.image;
            }
        }

        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}
