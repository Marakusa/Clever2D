using Clever2D.Engine;
using System.Collections.Generic;
using Clever2D.UI;
using SDL2;

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
                        position = new Vector2(50f, -50f),
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
                tag = "Player",
                children = new List<GameObject>()
                {
                    new GameObject("NameTag")
                    {
                        components = new List<Component>()
                        {
                            new Transform()
                            {
                                position = new Vector2(0f, 30f),
                                rotation = new Vector2(0f, 0f),
                                scale = new Vector2(1f, 1f)
                            },
                            new Text("Juan", new SDL.SDL_Color()
                            {
                                r = 255,
                                g = 155,
                                b = 155,
                                a = 255
                            })
                            {
                                worldSpace = true
                            },
                        }
                    }
                }
            },
            new GameObject("FPS")
            {
                components = new List<Component>()
                {
                    new Transform(),
                    new Text(),
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
