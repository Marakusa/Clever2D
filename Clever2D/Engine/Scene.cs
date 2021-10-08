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
        public string name;

        /// <summary>
        /// Get the ID of the scene.
        /// </summary>
        public int SceneId
        {
            get
            {
                if (SceneManager.includedScenes.Contains(this))
                    return SceneManager.includedScenes.IndexOf(this);
                
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
        public Dictionary<int, GameObject> SpawnedGameObjects => spawnedGameObjects;

        private bool isInitialized;
        /// <summary>
        /// Returns if the Scene is initialized.
        /// </summary>
        public bool IsInitialized => isInitialized;

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
        /// Destroy a GameObject in this Scene if loaded.
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
                isInitialized = false;

                bool added = false;
                
                while (!added)
                {
                    if (spawnedGameObjects.ContainsKey(id))
                    {
                        id++;
                        nextId = id + 1;
                    }
                    else
                    {
                        spawnedGameObjects.Add(id, gameObject);
                        added = true;
                    }
                }

                foreach (GameObject child in gameObject.children)
                {
                    child.parent = gameObject;
                    SpawnGameObject(child);
                }

                foreach (Component component in gameObject.components)
                {
                    component.gameObject = gameObject;
                    component.transform = gameObject.transform;
                    component.Initialize();
                    if (component.GetType() == typeof(SpriteRenderer))
                    {
                        OcclusionManager.AddRenderer((SpriteRenderer)component);
                    }
                }

                isInitialized = true;
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
                    if (component.GetType() == typeof(SpriteRenderer))
                    {
                        OcclusionManager.RemoveRenderer((SpriteRenderer)component);
                    }
                }

                spawnedGameObjects.Remove(gameObject.InstanceId);
                gameObject.Dispose();
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
            OcclusionManager.Clear();
            if (!SceneManager.LoadScene(this))
            {
                Player.LogError($"Loading \"{name}\" failed.");
            }
        }

        /// <summary>
        /// List of the GameObjects in this Scene.
        /// </summary>
        public List<GameObject> sceneGameObjects = new();
    }
}
