using Eto.Drawing;
using Eto.Forms;
using System;
using Clever2D.Engine;

namespace Clever2D.Desktop
{
    public partial class MainForm : Form
    {
        private Drawable canvas;

        public MainForm(string projectName = "Example", string authorName = "Example", string version = "0.1.0")
        {
            Title = projectName;
            MinimumSize = new Size(200, 200);
            Size = new Size(800, 600);

            BackgroundColor = new Color(0, 0, 0);

            canvas = new Drawable(false);

            canvas.Paint += Paint;
            canvas.MouseDown += MainForm_MouseDown;

            Content = canvas;
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            Scene scene = SceneManager.LoadedScene;
            if (scene != null && scene.ObjectCount > 0)
            {
                foreach (GameObject gameObject in scene.GetSpawnedGameObjects())
                {
                    Pen pen = new Pen(new Color(1f, 0f, 0f, 1f), 10f);
                    e.Graphics.DrawRectangle(pen, gameObject.transform.position.x, gameObject.transform.position.y, 10f, 10f);
                }
            }
        }

        public void Draw()
        {
            canvas.Invalidate();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            GameObject.Spawn(new GameObject("Yes"), new Vector2(e.Location.X, e.Location.Y));
            Draw();
        }
    }
}
