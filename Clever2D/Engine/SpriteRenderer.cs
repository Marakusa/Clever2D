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

        /// <summary>
        /// Send a request to draw this sprite.
        /// </summary>
        public void Draw()
        {
            if (SceneManager.Instance.LoadedScene != null)
            {
                SceneManager.Instance.LoadedScene.Draw();
            }
            else
            {
                Console.WriteLine("No scene loaded.");
            }
        }
    }
}
