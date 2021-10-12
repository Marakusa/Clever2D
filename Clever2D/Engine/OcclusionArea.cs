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

        /// <summary>
        /// Returns this sprite batches Sprite.
        /// </summary>
        public Sprite BatchSprite
        {
            get;
            private set;
        }

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

        private void UpdateBatch(SpriteRenderer r, bool initializing)
        {
            DrawBatch();
        }
        private void DrawBatch()
        {
            try
            {
                int size = OcclusionManager.AreaSize;

                Player.Log(size);

                // TODO: more optimized way to handle this
                using (var image = new Image<Rgba32>(size, size))
                {
                    foreach (var tile in renderers)
                    {
                        Vector3Int point = tile.transform.position.ToVector3Int();

                        point = point - occlusionPoint;

                        for (int y = 0; y < tile.Sprite.rect.h; y++)
                        {
                            int pixelY = point.y + y;

                            if (pixelY >= 0 && pixelY < size)
                            {
                                Span<Rgba32> pixelRowSpan = image.GetPixelRowSpan(pixelY);

                                for (int x = 0; x < tile.Sprite.rect.w; x++)
                                {
                                    int pixelX = point.x + x;

                                    if (pixelX >= 0 && pixelX < size)
                                    {
                                        int offsetX = tile.Sprite.rect.x;
                                        int offsetY = tile.Sprite.rect.y;

                                        try
                                        {
                                            Image asset = (Image)AssetLoader.GetAsset($"{tile.Sprite.Path}:Image");
                                            var color = ((Image<Rgba32>)asset)[x + offsetX, y + offsetY];
                                            pixelRowSpan[pixelX] = new Rgba32(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                                        }
                                        catch (Exception e)
                                        {
                                            Player.LogError(e.Message, e);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!Directory.Exists($"{Application.TempDirectory}batches/"))
                        Directory.CreateDirectory($"{Application.TempDirectory}batches/");

                    string saveLocation = $"{Application.TempDirectory}batches/{Cryptography.HashSHA1(occlusionPoint.GetHashCode().ToString())}.png";
                    image.Save(saveLocation);
                    Player.Log($"Tile batch saved ==> {saveLocation}");

                    BatchSprite = new Sprite(saveLocation, Vector2.Zero);
                }
            }
            catch (Exception e)
            {
                Player.LogError(e.Message, e);
            }
        }
    }
}
