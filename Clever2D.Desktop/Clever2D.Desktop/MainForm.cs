using Eto.Drawing;
using Eto.Forms;
using Clever2D.Engine;
using System;
using System.Collections.Generic;

namespace Clever2D.Desktop
{
    public partial class MainForm : Form, IKeyboardInputSource
    {
        private delegate List<GameObject> ListObjectsMethod getGameObjects;
        public static List<GameObject> ListObjectsMethod gameObjects;

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

        public List<GameObject> GetGameObjects()
        {
            return getGameObjects
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            if (SceneManager.LoadedScene != null && SceneManager.LoadedScene.ObjectCount > 0)
            {
                foreach (GameObject gameObject in GetGameObjects())
                {
                    SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        Console.WriteLine(gameObject.transform.position);
                        e.Graphics.DrawImage(renderer.Sprite, new PointF(gameObject.transform.position.x, gameObject.transform.position.y));
                    }
                }
            }
        }
        
        private void Draw()
        {
            Eto.Forms.Application.Instance.Invoke((Action)delegate {
                Drawable canvas = new Drawable(false);

                canvas.Paint += Paint;

                Content = canvas;

                UITimer drawTimer = new UITimer();

                drawTimer.Interval = 1f / 1f;
                drawTimer.Elapsed += (object sender, EventArgs e) =>
                {
                    Console.WriteLine("Draw");
                    canvas.Invalidate();
                };

                drawTimer.Start();
            });
        }
    }
}
