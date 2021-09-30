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
            Console.WriteLine("Creating form...");

            Title = projectName;
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
                var instances = loadedScene.Instances;

                if (instances.Count > 0)
                {
                    foreach (var instance in instances)
                    {
                        SpriteRenderer renderer = instance.Value.GetComponent<SpriteRenderer>();
                        if (renderer != null)
                        {
                            e.Graphics.DrawImage(renderer.Sprite, new PointF(instance.Value.transform.position.x, instance.Value.transform.position.y));
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
                    Drawable canvas = new Drawable(false);

                    canvas.Paint += Paint;

                    Content = canvas;

                    UITimer drawTimer = new UITimer();

                    drawTimer.Interval = 1f / 30f;
                    drawTimer.Elapsed += (object sender, EventArgs e) =>
                    {
                        this.Closed += (object sender, EventArgs e) =>
                        {
                            drawTimer.Stop();
                            this.Unbind();
                        };
                        canvas.Invalidate();
                    };

                    drawTimer.Start();
                });
            });

            thread.Start();
        }
    }
}
