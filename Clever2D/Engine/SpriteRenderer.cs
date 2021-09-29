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
        private Image sprite;
        public Image Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
                Draw();
            }
        }

        public SpriteRenderer(Image image)
        {
            Sprite = image;
        }

        public void Draw()
        {
            SceneManager.LoadedScene.Draw();
        }
    }
}
