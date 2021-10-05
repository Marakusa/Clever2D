using System;
using System.Collections.Generic;
using Clever2D.UI;

namespace Clever2D.Engine
{
    /// <summary>
    /// Base class for all entities in scenes.
    /// </summary>
    public class GameObject
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
        public string name = "New GameObject";
        /// <summary>
        /// The assigned tag to this GameObject.
        /// </summary>
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
            SpawnGameObject(gameObject, Vector2.zero, Vector2.zero);
        }
        /// <summary>
        /// Spawns the GameObject into the loaded scene.
        /// </summary>
        public static void Spawn(GameObject gameObject, Vector2 position)
        {
            SpawnGameObject(gameObject, position, Vector2.zero);
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
                gameObject.transform.Position = position;
                gameObject.transform.Position = rotation;
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
    }
}
