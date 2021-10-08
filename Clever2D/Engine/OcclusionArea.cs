using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    /// <summary>
    /// Area used by OcclusionManager.
    /// </summary>
    public class OcclusionArea
    {
        /// <summary>
        /// SpriteRenderers contained in this area.
        /// </summary>
        public List<SpriteRenderer> renderers = new();
        internal Vector3Int occlusionPoint = Vector3Int.Zero;

        internal void AddRenderer(SpriteRenderer renderer)
        {
            if (!renderers.Contains(renderer))
                renderers.Add(renderer);
        }
        internal void RemoveRenderer(SpriteRenderer renderer)
        {
            if (renderers.Contains(renderer))
                this.renderers.Remove(renderer);
        }
    }
}
