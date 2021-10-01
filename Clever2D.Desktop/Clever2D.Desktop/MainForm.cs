using Eto.Drawing;
using Eto.Forms;
using Clever2D.Engine;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Clever2D.Desktop
{
    public class MainForm : Form
    {
        public MainForm(string projectName = "Example", string authorName = "Example", string version = "0.1.0")
        {
            Player.Log("Creating form...");

            Title = $"{authorName} - {projectName} {version}";
            MinimumSize = new Size(200, 200);
            Size = new Size(800, 600);

            BackgroundColor = new Color(0, 0, 0);

            System.Threading.Thread graphicsThread = new(Draw);
            graphicsThread.Start();
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            Scene loadedScene = SceneManager.LoadedScene;
            if (loadedScene != null)
            {
                var instances = loadedScene.SpawnedGameObjects;

                if (instances.Count > 0)
                {
                    foreach (var instance in instances)
                    {
                        SpriteRenderer renderer = instance.Value.GetComponent<SpriteRenderer>();
                        if (renderer != null)
                        {
                            float scale = (Height / 600f) * 2f;
                            Bitmap bitmap = new(renderer.Sprite.Path);
                            Image image = new Bitmap(bitmap, (int)Math.Round(bitmap.Size.Width * scale), (int)Math.Round(bitmap.Size.Height * scale), ImageInterpolation.High);
                            e.Graphics.DrawImage(image, new PointF(instance.Value.transform.position.x * scale * instance.Value.transform.scale.x, -instance.Value.transform.position.y * scale * instance.Value.transform.scale.y));
                        }
                    }
                }
            }
        }
        
        private void Draw()
        {
            Thread thread = new(() =>
            {
                Eto.Forms.Application.Instance.Invoke((Action)delegate
                {
                    Drawable canvas = new(false);

                    canvas.Paint += Paint;

                    Content = canvas;

                    UITimer drawTimer = new();

                    DateTime frameStart = DateTime.Now;
                    DateTime frameEnd = DateTime.Now;

                    drawTimer.Interval = 1f / 60f;
                    drawTimer.Elapsed += (object sender, EventArgs e) =>
                    {
                        this.Closed += (object sender, EventArgs e) =>
                        {
                            drawTimer.Stop();
                            this.Unbind();
                        };
                        canvas.Invalidate();
                        //frameEnd = DateTime.Now;
                        //double fps = (60000f / (frameEnd - frameStart).TotalMilliseconds);
                        //Player.Log(fps.ToString("0") + " fps");
                        //frameStart = DateTime.Now;
                    };

                    drawTimer.Start();
                });
            });

            thread.Start();
        }
    }
}
