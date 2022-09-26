using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    /// <summary>
    /// Manages all the loaded assets and preloads them.
    /// </summary>
    public static class AssetLoader
    {
        private static Dictionary<string, object> resources = new();

        public static void LoadAssets()
        {
            if (resources.Count <= 0)
            {
                Player.Log("Loading assets...");
                LoadFolder(Application.ExecutableDirectory + "/assets/");
            }
            else
            {
                Player.LogError("Assets already loaded.");
            }
        }

        private static void LoadFolder(string path)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                string resourceName = file.Substring(Application.ExecutableDirectory.Length + "/assets/".Length);

                if (resources[resourceName] == null)
                {
                    switch (Path.GetExtension(file).Substring(1))
                    {
                        case "png":
                        case "jpg":
                        case "gif":
                            resources.Add(resourceName, new Sprite(file, Vector2.Zero, Vector2Int.Zero, Vector2.Zero));
                            break;
                    }
                }
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                LoadFolder(directory);
            }
        }

        /// <summary>
        /// Returns a loaded asset. If asset is not loaded, returns a null.
        /// </summary>
        /// <param name="resourceName">Assets resource name.</param>
        public static object GetAsset(string resourceName)
        {
            if (resources.ContainsKey(resourceName))
            {
                object asset = resources[resourceName];

                if (asset != null)
                    return asset;
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Adds an asset to resources list for optimization. If asset is already loaded, returns false.
        /// </summary>
        /// <param name="resourceName">Assets resource name.</param>
        public static bool AddAsset(string resourceName, object asset)
        {
            if (resources.ContainsKey(resourceName))
                return false;
            else
            {
                resources.Add(resourceName, asset);
                return true;
            }
        }
    }
}
