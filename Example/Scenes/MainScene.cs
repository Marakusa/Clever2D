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
        private string name = "MainScene";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        public readonly List<GameObject> sceneGameObjects = new List<GameObject>()
        {
            new GameObject("Sword"){
                components = new List<Component>()
                {
                    new ClickSpawn(),
                    new SpriteRenderer(new Sprite("Resources/Sword.png"))
                },
                tag = "Weapon",
                transform = new Transform(new Vector2(10f, 30f), Vector2.zero)
            }
        };
        public override List<GameObject> SceneGameObjects
        {
            get
            {
                return sceneGameObjects;
            }
        }
    }
}
