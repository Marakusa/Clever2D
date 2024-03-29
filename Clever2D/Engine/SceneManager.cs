﻿using System;
using System.Collections.Generic;

namespace Clever2D.Engine
{
    /// <summary>
    /// The base class which manages all the Scenes.
    /// </summary>
    public static class SceneManager
    {
        /// <summary>
        /// Returns if the SceneManager is initialized.
        /// </summary>
        public static bool IsInitialized
        {
            get;
            private set;
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
        /// Returns the loaded Scene.
        /// </summary>
        public static Scene LoadedScene
        {
            get;
            private set;
        }

        /// <summary>
        /// EventHandler for the event OnLoaded.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        public delegate void LoadedEventHandler(object sender, SceneEventArgs e);
        /// <summary>
        /// This event gets called when a Scene is done loading.
        /// </summary>
        public static event LoadedEventHandler OnLoaded = delegate { };

        /// <summary>
        /// EventHandler for the event OnLoad.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        public delegate void LoadEventHandler(object sender, SceneEventArgs e);
        /// <summary>
        /// This event gets called when a Scene starts to load.
        /// </summary>
        public static event LoadEventHandler OnLoad = delegate { };

        /// <summary>
        /// Load a scene.
        /// </summary>
        public static bool LoadScene(Scene scene)
        {
            Player.Log($"Loading a Scene named \"{scene.name}\"...");

            OnLoad?.Invoke(null, new(LoadedScene));

            try
            {
                if (LoadedScene != null)
                {
                    LoadedScene.SpawnedGameObjects.Clear();
                }

                LoadedScene = scene;

                int count = LoadedScene.sceneGameObjects.Count;
                int i = 0;
                
                foreach (GameObject obj in LoadedScene.sceneGameObjects)
                {
                    LoadedScene.SpawnGameObject(obj);
                    i++;
                    // TODO: Loading screen?
                    // Player.Log(i.ToString() + "/" + count.ToString());
                }

                IsInitialized = true;

                OcclusionManager.RendererAddingDone();

                OnLoaded?.Invoke(null, new(LoadedScene));

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
