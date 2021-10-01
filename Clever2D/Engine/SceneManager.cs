using System;
using System.Collections.Generic;

namespace Clever2D.Engine
{
    /// <summary>
    /// The base class which manages all the Scenes.
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// Is the SceneManager initialized.
        /// </summary>
        private static bool isInitialized = false;
        /// <summary>
        /// Returns is the SceneManager initialized.
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
        }

        /// <summary>
        /// Start the game and load the first Scene.
        /// </summary>
        public static void Initialize()
        {
            if (SceneList.Length > 0)
            {
                try
                {
                    if (!LoadScene(SceneList[0]))
                    {
                        Player.LogError("Failed to load the scene.");
                    }
                }
                catch (Exception exception)
                {
                    Player.LogError(exception.Message, exception);
                }
            }
            else
            {
                Player.LogError("Could not start the game. No Scenes added.");
            }
        }

        /// <summary>
        /// Returns the Scene list.
        /// </summary>
        public static Scene[] SceneList
        {
            get
            {
                return includedScenes.ToArray();
            }
        }
        /// <summary>
        /// The Scene list.
        /// </summary>
        internal static List<Scene> includedScenes = new();

        /// <summary>
        /// Add a Scene to the SceneManagers Scene list.
        /// </summary>
        public static void AddScene(Scene scene)
        {
            includedScenes.Add(scene);
        }
        /// <summary>
        /// Add multiple Scenes to the SceneManagers Scene list.
        /// </summary>
        public static void AddScenes(Scene[] scenes)
        {
            includedScenes.AddRange(scenes);
        }
        /// <summary>
        /// Add multiple Scenes to the SceneManagers Scene list.
        /// </summary>
        public static void AddScenes(List<Scene> scenes)
        {
            includedScenes.AddRange(scenes);
        }

        /// <summary>
        /// The loaded Scene.
        /// </summary>
        private static Scene loadedScene;
        /// <summary>
        /// Returns the loaded Scene.
        /// </summary>
        public static Scene LoadedScene
        {
            get
            {
                return loadedScene;
            }
        }

        /// <summary>
        /// EventHandler for the event OnLoaded.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        public delegate void LoadedEventHandler(object sender, SceneEventArgs e);
        /// <summary>
        /// This event gets called when the Scene is done loading.
        /// </summary>
        public static event LoadedEventHandler OnLoaded = delegate { };

        /// <summary>
        /// Load a scene.
        /// </summary>
        public static bool LoadScene(Scene scene)
        {
            Player.Log("Loading a Scene named \"" + scene.Name + "\"...");

            try
            {
                if (loadedScene != null)
                {
                    loadedScene.SpawnedGameObjects.Clear();
                }

                loadedScene = scene;

                foreach (GameObject obj in loadedScene.SceneGameObjects)
                {
                    loadedScene.SpawnGameObject(obj);
                }

                isInitialized = true;

                OnLoaded(null, new SceneEventArgs(loadedScene));

                return true;
            }
            catch (Exception exception)
            {
                Player.LogError(exception.Message, exception);
                return false;
            }
        }
    }

    /// <summary>
    /// Event arguments for Scene loading events.
    /// </summary>
    public class SceneEventArgs
    {
        /// <summary>
        /// Event arguments for Scene loading events.
        /// </summary>
        /// <param name="scene">The Scene this event call is for.</param>
        public SceneEventArgs(Scene scene) { Scene = scene; }
        /// <summary>
        /// The Scene this event call is for.
        /// </summary>
        public Scene Scene { get; }
    }
}
