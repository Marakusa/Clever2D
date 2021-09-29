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

        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite.image;
        }

        public void Draw()
        {
            if (SceneManager.LoadedScene != null)
            {
                SceneManager.LoadedScene.Draw();
            }
            else
            {
                Console.WriteLine("No scene loaded.");
            }
        }
    }
}
