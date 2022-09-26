using System;
using System.Collections.Generic;
using Clever2D.UI;
using Newtonsoft.Json;

namespace Clever2D.Engine
{
    /// <summary>
    /// Base class for all entities in scenes.
    /// </summary>
    public class GameObject : IDisposable
    {
        internal int instanceId;

        /// <summary>
        /// Gets the instance ID of this GameObject.
        /// </summary>
        public int InstanceId
        {
            get
            {
                return instanceId;
            }
        }

        /// <summary>
        /// The name of this GameObject.
        /// </summary>
        [JsonProperty]
        public readonly string name;
        /// <summary>
        /// Is object static (In example occlusion can be applied).
        /// </summary>
        [JsonProperty]
        public readonly bool isStatic;
        /// <summary>
        /// The assigned tag to this GameObject.
        /// </summary>
        [JsonProperty]
        public string tag = "Untagged";
        /// <summary>
        /// The transform component of this GameObject.
        /// </summary>
        public Transform transform;
        /// <summary>
        /// Components of this GameObject.
        /// </summary>
        public List<Component> components = new();
        /// <summary>
        /// Children GameObjects of this GameObject.
        /// </summary>
        public List<GameObject> children = new();
        /// <summary>
        /// Parent of this GameObject.
        /// </summary>
        public GameObject parent = null;

        /// <summary>
        /// Retrieves and returns a Component of type T.
        /// </summary>
        /// <typeparam name="T">The type of the Compnent to retrieve.</typeparam>
        public T GetComponent<T>()
        {
            foreach (object component in components)
            {
                if (typeof(T) == component.GetType() 
                    || (typeof(T) == typeof(UIElement) && component.GetType().ToString().StartsWith("Clever2D.UI")))
                {
                    return (T)component;
                }
            }

            return default;
        }

        /// <summary>
        /// Base class for all entities in scenes.
        /// </summary>
        public GameObject(string name, string tag = "Untagged")
        {
            this.name = name;
            this.tag = tag;
        }

        /// <summary>
        /// Spawns the GameObject into the loaded scene.
        /// </summary>
        public static void Spawn(GameObject gameObject)
        {
            SpawnGameObject(gameObject, Vector2.Zero, Vector2.Zero);
        }
        /// <summary>
        /// Spawns the GameObject into the loaded scene.
        /// </summary>
        public static void Spawn(GameObject gameObject, Vector2 position)
        {
            SpawnGameObject(gameObject, position, Vector2.Zero);
        }
        /// <summary>
        /// Spawns the GameObject into the loaded scene.
        /// </summary>
        public static void Spawn(GameObject gameObject, Vector2 position, Vector2 rotation)
        {
            SpawnGameObject(gameObject, position, rotation);
        }
        /// <summary>
        /// Sends the spawn request of the GameObject to the loaded scene.
        /// </summary>
        private static void SpawnGameObject(GameObject gameObject, Vector2 position, Vector2 rotation)
        {
            try
            {
                gameObject.transform.position = position;
                gameObject.transform.position = rotation;
                SceneManager.LoadedScene.SpawnGameObject(gameObject);
            }
            catch (Exception exception)
            {
                Player.LogError(exception.Message, exception);
            }
        }

        /// <summary>
        /// Removes a GameObject, component or asset.
        /// </summary>
        public static void Destroy(GameObject gameObject)
        {
            try
            {
                SceneManager.LoadedScene.DestroyGameObject(gameObject);
            }
            catch (Exception exception)
            {
                Player.LogError(exception.Message, exception);
            }
        }

        private bool disposed;

        /// <summary>
        /// Disposes and destroys this GameObject.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this GameObject and its components and children.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    foreach (Component component in components)
                    {
                        component.Dispose();
                    }

                    components.Clear();

                    foreach (GameObject child in children)
                    {
                        child.Dispose();
                    }

                    children.Clear();
                }

                disposed = true;
            }
        }
    }
}
