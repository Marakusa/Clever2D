using Eto.Drawing;
using Eto.Forms;
using Clever2D.Engine;
using System;
using Eto.Threading;

namespace Clever2D.Desktop
{
    public partial class MainForm : Form, IKeyboardInputSource
    {
        public Drawable canvas;

        public delegate void CallDraw();

        public MainForm(string projectName = "Example", string authorName = "Example", string version = "0.1.0")
        {
            Console.WriteLine("Creating form...");

            Title = projectName;
            MinimumSize = new Size(200, 200);
            Size = new Size(800, 600);

            BackgroundColor = new Color(0, 0, 0);

            canvas = new Drawable(false);

            canvas.Paint += Paint;
            canvas.MouseDown += MainForm_MouseDown;

            Content = canvas;

            CallDraw del = new CallDraw(this.Draw);
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            Scene scene = SceneManager.Instance.LoadedScene;

            if (scene != null && scene.ObjectCount > 0)
            {
                foreach (GameObject gameObject in scene.GetSpawnedGameObjects())
                {
                    SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        e.Graphics.DrawImage(renderer.Sprite, new PointF(gameObject.transform.position.x, -gameObject.transform.position.y + Height));
                    }
                }
            }
        }

        public void Draw()
        {
            Action a = canvas.Invalidate;
            a.Invoke();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            GameObject.Spawn(new GameObject("Yes"), new Vector2(e.Location.X, e.Location.Y));
            Draw();
        }
    }
}
