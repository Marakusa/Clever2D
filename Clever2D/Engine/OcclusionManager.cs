﻿using System;
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
        internal static SpriteRenderer[] NonStaticRenderers
        {
            get
            {
                return nonStaticRenderers.ToArray();
            }
        }

        /// <summary>
        /// Area chunk sizes.
        /// </summary>
        internal const int AreaSize = 128;

        /// <summary>
        /// Adds renderer to occlusion manager.
        /// </summary>
        internal static void AddRenderer(SpriteRenderer renderer, bool initializing)
        {
            // TODO: Check if renderer gameobject static status has changed
            if (renderer.gameObject.isStatic)
            {
                Vector position = renderer.transform.position;

                int x = (int)Math.Floor(position.x / AreaSize) * AreaSize;
                int y = (int)Math.Floor(position.y / AreaSize) * AreaSize;
                int z = (int)Math.Floor(position.z / AreaSize) * AreaSize;

                Vector3Int areaPoint = new Vector3Int(x, y, z);

                if (areas.ContainsKey(areaPoint))
                {
                    areas[areaPoint].AddRenderer(renderer, initializing);
                }
                else
                {
                    areas.Add(areaPoint, new()
                    {
                        occlusionPoint = areaPoint
                    });
                    areas[areaPoint].AddRenderer(renderer, initializing);
                }

                int x2 = (int)Math.Floor((position.x + renderer.Sprite.rect.w) / AreaSize) * AreaSize;
                int y2 = (int)Math.Floor((position.y + renderer.Sprite.rect.h) / AreaSize) * AreaSize;

                Vector3Int areaPoint2 = new Vector3Int(x2, y2, z);

                if (areas.ContainsKey(areaPoint2))
                {
                    areas[areaPoint2].AddRenderer(renderer, initializing);
                }
                else
                {
                    areas.Add(areaPoint2, new()
                    {
                        occlusionPoint = areaPoint2
                    });
                    areas[areaPoint2].AddRenderer(renderer, initializing);
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
        internal static void RemoveRenderer(SpriteRenderer renderer)
        {
            if (renderer.gameObject.isStatic)
            {
                Vector position = renderer.transform.position;

                int x = (int)Math.Round(position.x / AreaSize) * AreaSize;
                int y = (int)Math.Round(position.y / AreaSize) * AreaSize;
                int z = (int)Math.Round(position.z / AreaSize) * AreaSize;

                Vector3Int areaPoint = new Vector3Int(x, y, z);

                if (areas.ContainsKey(areaPoint))
                {
                    areas[areaPoint].RemoveRenderer(renderer, true);
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
        /// <param name="radius">Rendering radius.</param>
        internal static OcclusionArea[] GetNearestAreas(Vector position, float radius)
        {
            List<OcclusionArea> list = new();

            int multiplier = (int)Math.Ceiling(radius / 2f);

            int x = (int)Math.Round(position.x / AreaSize) * AreaSize;
            int y = (int)Math.Round(position.y / AreaSize) * AreaSize;
            int z = 0;

            Vector3Int areaPoint = new Vector3Int(x, y, z);

            if (areas.ContainsKey(areaPoint))
            {
                list.Add(areas[areaPoint]);
            }
            
            return list.ToArray();
        }

        internal static void RendererAddingDone()
        {
            foreach (var area in areas)
            {
                area.Value.BatchDone();
            }
        }

        /// <summary>
        /// Clears the occlusion data created.
        /// </summary>
        internal static void Clear()
        {
            areas.Clear();
            nonStaticRenderers.Clear();
        }
    }
}
