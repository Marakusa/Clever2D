using Eto.Drawing;
using Eto.Forms;
using System;

namespace Clever2D.Desktop
{
    public partial class MainForm : Form
    {
        private Drawable drawable;
        private RectangleF rect;

        public MainForm(string projectName = "Example", string authorName = "Example", string version = "0.1.0")
        {
            Title = projectName;
            MinimumSize = new Size(200, 200);
            Size = new Size(800, 600);

            BackgroundColor = new Color(0, 0, 0);

            drawable = new Drawable(false);

            drawable.Paint += Paint;
            drawable.MouseDown += MainForm_MouseDown;

            Content = drawable;
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("Paint: " + rect.IsEmpty);
            if (!rect.IsEmpty)
            {
                Pen pen = new Pen(new Color(1f, 0f, 0f, 1f), 10f);
                e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        public void Draw(float x, float y, float width, float height)
        {
            rect = new RectangleF(x, y, width, height);
            drawable.Invalidate();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Location.ToString());
            Draw(e.Location.X, e.Location.Y, 10f, 10f);
        }
    }
}
