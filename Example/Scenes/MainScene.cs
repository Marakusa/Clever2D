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
                        Position = new Vector2(0f, 0f),
                        Rotation = new Vector2(0f, 0f),
                        Scale = new Vector2(1f, 1f)
                    },
                    new SpriteRenderer(new Sprite("resources/Stars.gif", Vector2.zero))
                },
                tag = "Background"
            },
            new GameObject("Player")
            {
                components = new List<Component>()
                {
                    new Transform()
                    {
                        Position = new Vector2(50f, -50f),
                        Rotation = new Vector2(0f, 0f),
                        Scale = new Vector2(1f, 1f)
                    },
                    new PlayerController()
                    {
                        speed = 2f
                    },
                    new SpriteRenderer(new SpriteArray("resources/Skeleton.png", Vector2.one / 2f, 4, 4)),
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
                                Position = new Vector2(-16f, 20f),
                                Rotation = new Vector2(0f, 0f),
                                Scale = new Vector2(1f, 1f)
                            },
                            new Text("Skeleton", 12, new SDL.SDL_Color()
                            {
                                r = 255,
                                g = 155,
                                b = 155,
                                a = 255
                            })
                            {
                                worldSpace = true
                            }
                        }
                    }
                }
            },
            new GameObject("FPS")
            {
                components = new List<Component>()
                {
                    new Transform(),
                    new Text(28),
                    new FPSCounter()
                },
                tag = "Player"
            },
            new GameObject("Camera")
            {
                components = new List<Component>()
                {
                    new Transform(),
                    new Camera()
                },
                tag = "MainCamera"
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
