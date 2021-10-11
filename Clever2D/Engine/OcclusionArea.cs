using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;

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

        internal void AddRenderer(SpriteRenderer renderer, bool initializing)
        {
            if (!renderers.Contains(renderer))
            {
                renderer.OnChanged += BatchChanged;
                renderers.Add(renderer);

                // TODO: Tell update batch its a new renderer for optimization
                if (!initializing)
                    UpdateBatch(null, false);
            }
        }
        internal void RemoveRenderer(SpriteRenderer renderer, bool initializing)
        {
            if (renderers.Contains(renderer))
            {
                renderer.OnChanged -= BatchChanged;
                this.renderers.Remove(renderer);

                if (!initializing)
                    UpdateBatch(null, false);
            }
        }

        internal void BatchDone()
        {
            UpdateBatch(null, true);
        }

        private void BatchChanged(object sender, SpriteRendererChangedEventArgs e)
        {
            Player.Log(e.renderer);
            UpdateBatch(e.renderer, false);
        }

        private int rightmost = 1;
        private int bottommost = 1;

        private void UpdateBatch(SpriteRenderer r, bool initializing)
        {
            if (r == null || initializing)
            {
                foreach (var renderer in renderers)
                {
                    Vector3Int point = renderer.transform.position.ToVector3Int() - occlusionPoint;

                    if (point.x + renderer.Sprite.rect.w > rightmost)
                    {
                        rightmost = point.x + renderer.Sprite.rect.w;
                    }

                    if (renderer.Sprite.rect.h - point.y > bottommost)
                    {
                        bottommost = renderer.Sprite.rect.h - point.y;
                    }
                }

                Player.Log(rightmost);
                Player.Log(bottommost);

                DrawBatch();
            }
            else
            {
                DrawBatch();
            }
        }
        private void DrawBatch()
        {
            try
            {
                rightmost = (int)CMath.Clamp(rightmost, 1, int.MaxValue);
                bottommost = (int)CMath.Clamp(bottommost, 1, int.MaxValue);

                // TODO: more optimized way to handle this
                using (var image = new Image<Rgba32>(rightmost, bottommost))
                {
                    foreach (var tile in renderers)
                    {
                        Vector3Int point = tile.transform.position.ToVector3Int() - occlusionPoint;

                        for (int y = 0; y < tile.Sprite.rect.h; y++)
                        {
                            for (int x = 0; x < tile.Sprite.rect.w; x++)
                            {
                                int pixelX = point.x + x;
                                int pixelY = point.y + y;

                                if (pixelX < image.Width && pixelY < image.Height && pixelX >= 0 && pixelY >= 0)
                                {
                                    var pixel = image[pixelX, pixelY];
                                    Image asset = (Image)AssetLoader.GetAsset(tile.Sprite.Path + ":Image");
                                    pixel.Rgba = ((Image<Rgba32>)asset)[x, y].Rgba;
                                }
                            }
                        }
                    }

                    if (!Directory.Exists($"{Application.TempDirectory}batches/"))
                        Directory.CreateDirectory($"{Application.TempDirectory}batches/");

                    string saveLocation = $"{Application.TempDirectory}batches/{Cryptography.HashSHA1(new Vector2Int(rightmost, bottommost).GetHashCode().ToString())}.png";
                    image.Save(saveLocation);
                    Player.Log($"Tile batch saved ==> {saveLocation}");
                }
            }
            catch (Exception e)
            {
                Player.LogError(e.Message, e);
            }
        }
    }
}
