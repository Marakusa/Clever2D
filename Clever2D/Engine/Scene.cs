using System;
using System.Collections.Generic;

namespace Clever2D.Engine
{
    /// <summary>
    /// Collection of entities and components.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Name of the scene.
        /// </summary>
        public string name = "New Scene";

        /// <summary>
        /// Get the ID of the scene.
        /// </summary>
        public int SceneId
        {
            get
            {
                if (SceneManager.includedScenes.Contains(this))
                    return SceneManager.includedScenes.IndexOf(this);
                else
                    return -1;
            }
        }

        private int nextId = -2147483648;
        private readonly List<int> availableIds = new();

        internal Dictionary<int, GameObject> instances = new();

        internal void SpawnGameObject(GameObject gameObject)
        {
            try
            {
                if (nextId <= 2147483647 && nextId >= -2147483648)
                {
                    if (gameObject != null)
                    {
                        if (availableIds.Count > 0)
                        {
                            int id = availableIds[0];

                            availableIds.Remove(id);

                            gameObject.instanceId = id;
                            instances.Add(id, gameObject);
                        }
                        else
                        {
                            gameObject.instanceId = nextId;
                            instances.Add(nextId, gameObject);

                            nextId++;
                        }
                    }
                    else
                    {
                        throw new NullReferenceException("GameObject could not be instantiated. Given GameObject was null.");
                    }
                }
                else
                {
                    if (nextId > 2147483647)
                        throw new Exception("GameObject could not be instantiated. The next instance ID available was larger than the 32-bit limit (2147483647).");
                    else if (nextId < -2147483648)
                        throw new Exception("GameObject could not be instantiated. The next instance ID available was smaller than the 32-bit limit (-2147483648).");
                    else
                        throw new Exception("GameObject could not be instantiated.");
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }
        internal void DestroyGameObject(GameObject gameObject)
        {
            try
            {
                if (gameObject != null)
                {
                    instances.Remove(gameObject.InstanceId);
                    availableIds.Add(gameObject.InstanceId);
                }
                else
                {
                    throw new NullReferenceException("GameObject could not be destroyed. Given GameObject was null.");
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }

        public void LoadScene()
        {
            SceneManager.LoadScene(this);
        }
    }
}
