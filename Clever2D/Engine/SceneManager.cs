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
                    if (!LoadScene(SceneList[0]))
                    {
                        Console.WriteLine("Failed to load the scene.");
                    }
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

        private static Scene loadedScene;
        public static Scene LoadedScene
        {
            get
            {
                return loadedScene;
            }
        }

        public delegate void LoadedEventHandler(object sender, LoadedEventArgs e);
        /// <summary>
        /// This event gets called when the Scene is done loading.
        /// </summary>
        public static event LoadedEventHandler OnLoaded = delegate { };

        public delegate void SceneDrawEventHandler(object sender, SceneDrawEventArgs e);
        /// <summary>
        /// This event gets called when the Scene is being requested to be drawn.
        /// </summary>
        public static event SceneDrawEventHandler OnSceneDraw = delegate { };

        /// <summary>
        /// This event gets called when the Scene is being requested to be drawn.
        /// </summary>
        public static void DrawCalled(Scene scene)
        {
            if (scene == LoadedScene)
            {
                OnSceneDraw(null, new SceneDrawEventArgs(scene));
            }
        }

        /// <summary>
        /// Load a scene.
        /// </summary>
        public static bool LoadScene(Scene scene)
        {
            Console.WriteLine("Loading a Scene named \"" + scene.Name + "\"...");

            try
            {
                if (loadedScene != null)
                {
                    loadedScene.instances.Clear();
                }

                loadedScene = scene;

                foreach (GameObject obj in loadedScene.SceneGameObjects)
                {
                    loadedScene.SpawnGameObject(obj);
                }

                started = true;

                OnLoaded(null, new LoadedEventArgs(scene));

                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }
    }

    public class LoadedEventArgs
    {
        public LoadedEventArgs(Scene scene) { Scene = scene; }
        public Scene Scene { get; }
    }
    public class SceneDrawEventArgs
    {
        public SceneDrawEventArgs(Scene scene) { Scene = scene; }
        public Scene Scene { get; }
    }
}
