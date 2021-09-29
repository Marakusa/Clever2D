using System;
using System.Collections.Generic;
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
        public Transform transform = new();
        /// <summary>
        /// The components of this GameObject.
        /// </summary>
        public List<Component> components = new List<Component>();

        public T GetComponent<T>()
        {
            foreach (object component in components)
            {
                if (typeof(T) == component.GetType())
                {
                    return (T)component;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Base class for all entities in scenes.
        /// </summary>
        public GameObject(string name)
        {
            this.name = name;
            this.tag = "Untagged";
            this.transform = new();
        }

        /// <summary>
        /// Spawns the GameObject into the scene.
        /// </summary>
        public static void Spawn(GameObject gameObject)
        {
            Spawn_(gameObject, Vector2.zero, Vector2.zero);
        }
        public static void Spawn(GameObject gameObject, Vector2 position)
        {
            Spawn_(gameObject, position, Vector2.zero);
        }
        public static void Spawn(GameObject gameObject, Vector2 position, Vector2 rotation)
        {
            Spawn_(gameObject, position, rotation);
        }
        private static void Spawn_(GameObject gameObject, Vector2 position, Vector2 rotation)
        {
            try
            {
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;
                gameObject.transform.scale = Vector2.one;
                SceneManager.LoadedScene.SpawnGameObject(gameObject);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }

        public static void Destroy(GameObject gameObject)
        {
            try
            {
                SceneManager.LoadedScene.DestroyGameObject(gameObject);
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }
    }
}
