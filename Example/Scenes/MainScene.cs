using Clever2D.Engine;
using Eto.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    public class MainScene : Scene
    {
        public override GameObject[] SceneGameObjects
        {
            get
            {
                return sceneGameObjects.ToArray();
            }
        }

        public readonly List<GameObject> sceneGameObjects = new List<GameObject>()
        {
            new GameObject("Sword"){
                components = new List<Component>()
                {
                    new ClickSpawn(),
                    new SpriteRenderer(new Sprite("Resources/Sword.png"))
                }
            }
        };
    }
}
