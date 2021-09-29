using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public class GameObject
    {
        private int instanceId;
        private bool instanceIdSet;
        public int InstanceId
        {
            get
            {
                return instanceId;
            }
            set
            {
                if (!instanceIdSet)
                {
                    instanceId = value;
                    instanceIdSet = true;
                }
            }
        }

        public string name = "New GameObject";
        public Transform transform = new Transform();

        public GameObject()
        {
            this.name = "New GameObject";
            this.transform = new Transform();
        }
        public GameObject(string name)
        {
            this.name = name;
            this.transform = new Transform();
        }

        public static void Instansiate(GameObject gameObject, Vector position = Vector.zero, Vector rotation = Vector.zero)
        {
            _Instantiate(gameObject, position, rotation);
        }
        private static void _Instantiate(GameObject gameObject, Vector position, Vector rotation)
        {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            gameObject.transform.scale = Vector.one;
            SceneManager.LoadedScene.InstantiateGameObject(gameObject);
        }

        public static void Destroy(GameObject gameObject)
        {
            gameObjects[gameObject.id] = null;
            availableIds.Add(gameObject.id);
        }
    }
}
