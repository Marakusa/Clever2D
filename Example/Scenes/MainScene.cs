using Clever2D.Engine;
using System.Collections.Generic;
using Clever2D.UI;

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
                    new PlayerController()
                    {
                        speed = 2f
                    },
                    new SpriteRenderer(new SpriteArray("resources/Skeleton.png", 4, 4)),
                    
                    new AnimatorController("animations/skeleton.anim")
                },
                tag = "Player"
            },
            new GameObject("FPS")
            {
                components = new List<Component>()
                {
                    new Transform(),
                    new Text(""),
                    new FPSCounter()
                },
                tag = "Player"
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
