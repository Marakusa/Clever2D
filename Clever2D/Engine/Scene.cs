using System;
using System.Collections.Generic;
using System.Timers;

namespace Clever2D.Engine
{
    /// <summary>
    /// Collection of entities and components.
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        /// Name of the scene.
        /// </summary>
        public abstract string Name
        {
            get;
        }

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
        private List<GameObject> gameObjects = new();

        private int objectCount = 0;
        public int ObjectCount
        {
            get
            {
                return objectCount;
            }
        }

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
                            AddGameObject(id, gameObject);

                            objectCount++;
                        }
                        else
                        {
                            gameObject.instanceId = nextId;
                            AddGameObject(nextId, gameObject);

                            objectCount++;

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
                    RemoveGameObject(gameObject);
                    availableIds.Add(gameObject.InstanceId);

                    objectCount--;
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

        private void AddGameObject(int id, GameObject gameObject)
        {
            instances.Add(id, gameObject);
            gameObjects.Add(gameObject);

            foreach (Component component in gameObject.components)
            {
                component.gameObject = gameObject;
                component.transform = gameObject.transform;

                if ((component as CleverScript) != null)
                {
                    ((CleverScript)component).Start();
                    Timer tickTimer = new();

                    tickTimer.Interval = 1f / 30f;
                    tickTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                    {
                        ((CleverScript)component).FixedUpdate();
                    };

                    tickTimer.Start();

                    ((CleverScript)component).timer = tickTimer;
                }
            }
        }

        private void RemoveGameObject(GameObject gameObject)
        {
            foreach (Component component in gameObject.components)
            {
                if ((component as CleverScript) != null)
                {
                    ((CleverScript)component).timer.Stop();
                    ((CleverScript)component).timer = null;
                }
            }

            instances.Remove(gameObject.InstanceId);
            gameObjects.Remove(gameObject);
        }

        /// <summary>
        /// Loads this scene.
        /// </summary>
        public void LoadScene()
        {
            if (!SceneManager.LoadScene(this))
            {
                Console.WriteLine("Loading \"" + Name + "\" failed.");
            }
        }

        public List<GameObject> GetSpawnedGameObjects()
        {
            return gameObjects;
        }

        public abstract List<GameObject> SceneGameObjects
        {
            get;
        }

        /// <summary>
        /// Sends a redraw request to Canvas
        /// 
        /// TODO: Change this method
        /// 
        /// </summary>
        public void Draw()
        {
            SceneManager.DrawCalled(this);
        }
    }
}
