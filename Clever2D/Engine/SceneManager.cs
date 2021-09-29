using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public sealed class SceneManager
    {
        private static SceneManager instance = null;
        private static readonly object padlock = new();

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new SceneManager();
                        }
                    }
                }
                return instance;
            }
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
