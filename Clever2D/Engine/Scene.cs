using System;
using System.Collections.Generic;
using System.Threading;
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

        /// <summary>
        /// The instance ID of the next instance.
        /// </summary>
        private int nextId = -2147483648;
        /// <summary>
        /// List of all IDs of destroyed spawned GameObjects. These will be used by the next spawned GameObjects that are going to be spawned.
        /// </summary>
        private readonly List<int> availableIds = new();

        /// <summary>
        /// List of all spawned GameObjects.
        /// </summary>
        private readonly Dictionary<int, GameObject> spawnedGameObjects = new();
        /// <summary>
        /// Returns the list of all spawned GameObjects.
        /// </summary>
        public Dictionary<int, GameObject> SpawnedGameObjects
        {
            get
            {
                return spawnedGameObjects;
            }
        }

        /// <summary>
        /// Spawn a GameObejct to this Scene if loaded.
        /// </summary>
        /// <param name="gameObject">The GameObject to spawn.</param>
        internal void SpawnGameObject(GameObject gameObject)
        {
            if (SceneManager.LoadedScene == this)
            {
                try
                {
                    gameObject.transform = gameObject.GetComponent<Transform>();

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
                            }
                            else
                            {
                                gameObject.instanceId = nextId;
                                AddGameObject(nextId, gameObject);

                                nextId++;
                            }
                        }
                        else
                        {
                            Player.LogError("GameObject could not be instantiated. Given GameObject was null.", new NullReferenceException());
                        }
                    }
                    else
                    {
                        if (nextId > 2147483647)
                            Player.LogError("GameObject could not be instantiated. The next instance ID available was larger than the 32-bit limit (2147483647).");
                        else if (nextId < -2147483648)
                            Player.LogError("GameObject could not be instantiated. The next instance ID available was smaller than the 32-bit limit (-2147483648).");
                        else
                            Player.LogError("GameObject could not be instantiated.");
                    }
                }
                catch (Exception exception)
                {
                    Player.LogError(exception.Message, exception);
                }
            }
            else
            {
                Player.LogError("GameObject could not be instantiated. The Scene is not loaded.");
            }
        }
        /// <summary>
        /// Destroy a GameObejct in this Scene if loaded.
        /// </summary>
        /// <param name="gameObject">The GameObject to destroy.</param>
        internal void DestroyGameObject(GameObject gameObject)
        {
            if (SceneManager.LoadedScene == this)
            {
                try
                {
                    if (gameObject != null)
                    {
                        RemoveGameObject(gameObject);
                        availableIds.Add(gameObject.InstanceId);
                    }
                    else
                    {
                        Player.LogError("GameObject could not be destroyed. Given GameObject was null.", new NullReferenceException());
                    }
                }
                catch (Exception exception)
                {
                    Player.LogError(exception.Message, exception);
                }
            }
            else
            {
                Player.LogError("GameObject could not be destroyed. The Scene is not loaded.");
            }
        }

        /// <summary>
        /// Add a GameObject being spawned into the spawned GameObjects list.
        /// </summary>
        /// <param name="id">The instance ID of the GameObject.</param>
        /// <param name="gameObject">The spawned GameObject.</param>
        private void AddGameObject(int id, GameObject gameObject)
        {
            if (SceneManager.LoadedScene == this)
            {
                spawnedGameObjects.Add(id, gameObject);

                foreach (Component component in gameObject.components)
                {
                    component.gameObject = gameObject;
                    component.transform = gameObject.transform;

                    if ((component as CleverScript) != null)
                    {
                        ((CleverScript)component).Start();

                        Thread thread = new(() =>
                        {
                            System.Timers.Timer tickTimer = new();

                            DateTime frameStart = DateTime.Now;
                            DateTime frameEnd = DateTime.Now;

                            tickTimer.Interval = 1f / 60f;
                            tickTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                            {
                                ((CleverScript)component).FixedUpdate();
                                //frameEnd = DateTime.Now;
                                //double fps = (60000f / (frameEnd - frameStart).TotalMilliseconds);
                                //frameStart = DateTime.Now;
                            };

                            tickTimer.Start();

                            ((CleverScript)component).timer = tickTimer;
                        });

                        thread.Start();
                    }
                }
            }
            else
            {
                Player.LogError("GameObject could not be instantiated. The Scene is not loaded.");
            }
        }
        /// <summary>
        /// Removes a GameObject added into the spawned GameObjects list.
        /// </summary>
        /// <param name="gameObject">The spawned GameObject to be removed.</param>
        private void RemoveGameObject(GameObject gameObject)
        {
            if (SceneManager.LoadedScene == this)
            {
                foreach (Component component in gameObject.components)
                {
                    if ((component as CleverScript) != null)
                    {
                        ((CleverScript)component).timer.Stop();
                        ((CleverScript)component).timer = null;
                    }
                }

                spawnedGameObjects.Remove(gameObject.InstanceId);
            }
            else
            {
                Player.LogError("GameObject could not be destroyed. The Scene is not loaded.");
            }
        }

        /// <summary>
        /// Loads this scene.
        /// </summary>
        public void LoadScene()
        {
            if (!SceneManager.LoadScene(this))
            {
                Player.LogError("Loading \"" + Name + "\" failed.");
            }
        }

        /// <summary>
        /// List of the GameObjects in this Scene.
        /// </summary>
        public abstract List<GameObject> SceneGameObjects
        {
            get;
        }
    }
}
