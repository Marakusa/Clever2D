using System;
using System.Collections.Generic;

namespace Clever2D.Engine
{
    public static class SceneManager
    {
        private static bool started = false;
        public static bool Started
        {
            get
            {
                return started;
            }
        }

        /// <summary>
        /// Start the game and load the first Scene.
        /// </summary>
        public static void Start()
        {
            if (SceneList.Length > 0)
            {
                try
                {
                    LoadScene(SceneList[0]);
                    started = true;
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
            else
            {
                throw new Exception("Could not start the game. No Scenes added.");
            }
        }

        /// <summary>
        /// Get the Scene list of the SceneManager.
        /// </summary>
        public static Scene[] SceneList
        {
            get
            {
                return includedScenes.ToArray();
            }
        }
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

        private static Scene loadedScene = null;
        public static Scene LoadedScene
        {
            get
            {
                return loadedScene;
            }
        }

        public static void LoadScene(Scene scene)
        {
            if (loadedScene != null)
            {
                loadedScene.instances.Clear();
            }

            loadedScene = scene;
        }
    }
}
