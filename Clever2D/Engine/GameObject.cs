using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    /// <summary>
    /// Base class for all entities in scenes.
    /// </summary>
    public class GameObject
    {
        internal int instanceId;
        private bool instanceIdSet;

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
        /// The transform component of this GameObject.
        /// </summary>
        public Transform transform = new Transform();

        /// <summary>
        /// Base class for all entities in scenes.
        /// </summary>
        public GameObject()
        {
            this.name = "New GameObject";
            this.transform = new Transform();
        }
        /// <summary>
        /// Base class for all entities in scenes.
        /// </summary>
        public GameObject(string name)
        {
            this.name = name;
            this.transform = new Transform();
        }

        /// <summary>
        /// Spawns the GameObject into the scene.
        /// </summary>
        public static void Spawn(GameObject gameObject)
        {
            _Spawn(gameObject, Vector2.zero, Vector2.zero);
        }
        public static void Spawn(GameObject gameObject, Vector2 position)
        {
            _Spawn(gameObject, position, Vector2.zero);
        }
        public static void Spawn(GameObject gameObject, Vector2 position, Vector2 rotation)
        {
            _Spawn(gameObject, position, rotation);
        }
        private static void _Spawn(GameObject gameObject, Vector2 position, Vector2 rotation)
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
                throw exception;
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
                throw exception;
            }
        }
    }
}
