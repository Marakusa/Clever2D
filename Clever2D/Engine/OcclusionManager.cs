using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    /// <summary>
    /// Manages all the occlusion data.
    /// </summary>
    public class OcclusionManager
    {
        /// <summary>
        /// Split chunk for rendering.
        /// </summary>
        private static Dictionary<Vector3Int, OcclusionArea> areas = new();

        /// <summary>
        /// List of static members tried to occlude.
        /// </summary>
        private static List<SpriteRenderer> nonStaticRenderers = new();
        /// <summary>
        /// Returns an array of static members tried to occlude.
        /// </summary>
        public static SpriteRenderer[] NonStaticRenderers
        {
            get
            {
                return nonStaticRenderers.ToArray();
            }
        }

        /// <summary>
        /// Area chunk sizes.
        /// </summary>
        public static int areaSize = 100;

        /// <summary>
        /// Adds renderer to occlusion manager.
        /// </summary>
        public static void AddRenderer(SpriteRenderer renderer)
        {
            // TODO: Check if renderer gameobject static status has changed
            if (renderer.gameObject.isStatic)
            {
                Vector position = renderer.transform.position;

                int x = (int)Math.Round(position.x / areaSize / 2f);
                int y = (int)Math.Round(position.y / areaSize / 2f);
                int z = (int)Math.Round(position.z / areaSize / 2f);

                Vector3Int areaPoint = new Vector3Int(x, y, z);

                if (areas.ContainsKey(areaPoint))
                {
                    areas[areaPoint].AddRenderer(renderer);
                }
                else
                {
                    areas.Add(areaPoint, new()
                    {
                        occlusionPoint = areaPoint
                    });
                    areas[areaPoint].AddRenderer(renderer);
                }
            }
            else
            {
                nonStaticRenderers.Add(renderer);
            }
        }

        /// <summary>
        /// Adds renderer to occlusion manager.
        /// </summary>
        public static void RemoveRenderer(SpriteRenderer renderer)
        {
            if (renderer.gameObject.isStatic)
            {
                Vector position = renderer.transform.position;

                int x = (int)Math.Round(position.x / areaSize / 2f);
                int y = (int)Math.Round(position.y / areaSize / 2f);
                int z = (int)Math.Round(position.z / areaSize / 2f);

                Vector3Int areaPoint = new Vector3Int(x, y, z);

                if (areas.ContainsKey(areaPoint))
                {
                    areas[areaPoint].RemoveRenderer(renderer);
                }
            }
            else
            {
                nonStaticRenderers.Remove(renderer);
            }
        }

        /// <summary>
        /// Returns the nearest area to a point.
        /// </summary>
        /// <param name="position">Checking origin point.</param>
        public static OcclusionArea GetNearestArea(Vector position)
        {
            int x = (int)Math.Round(position.x / areaSize / 2f);
            int y = (int)Math.Round(position.y / areaSize / 2f);
            int z = (int)Math.Round(position.z / areaSize / 2f);

            Vector3Int areaPoint = new Vector3Int(x, y, z);

            if (areas.ContainsKey(areaPoint))
            {
                return areas[areaPoint];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Clears the occlusion data created.
        /// </summary>
        public static void Clear()
        {
            areas.Clear();
            nonStaticRenderers.Clear();
        }
    }
}
