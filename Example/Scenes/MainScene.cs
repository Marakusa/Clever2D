using Clever2D.Engine;
using System.Collections.Generic;

namespace Example
{
    public class MainScene : Scene
    {
        private readonly string name = "MainScene";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        public readonly List<GameObject> sceneGameObjects = new()
        {
            new GameObject("Background")
            {
                components = new List<Component>()
                {
                    new Transform()
                    {
                        position = new Vector2(0f, 0f),
                        rotation = new Vector2(0f, 0f),
                        scale = new Vector2(1f, 1f)
                    },
                    new SpriteRenderer(new Sprite("resources/Stars.gif"))
                },
                tag = "Background"
            },
            new GameObject("Player")
            {
                components = new List<Component>()
                {
                    new Transform()
                    {
                        position = new Vector2(10f, 10f),
                        rotation = new Vector2(0f, 0f),
                        scale = new Vector2(1f, 1f)
                    },
                    new ClickSpawn()
                    {
                        speed = 2f
                    },
                    new SpriteRenderer(new Sprite("resources/Sword.png"))
                },
                tag = "Player"
            }/*,
            new GameObject("Sword2")
            {
                components = new List<Component>()
                {
                    new Transform()
                    {
                        position = new Vector2(50f, 10f),
                        rotation = new Vector2(0f, 0f),
                        scale = new Vector2(1f, 1f)
                    },
                    new ClickSpawn(),
                    new SpriteRenderer(new Sprite("assets/resources/Sword.png"))
                },
                tag = "Weapon"
            }*/
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
