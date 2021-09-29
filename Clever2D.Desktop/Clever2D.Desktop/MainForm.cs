using Eto.Drawing;
using Eto.Forms;
using System;

namespace Clever2D.Desktop
{
    public partial class MainForm : Form
    {
        public Drawable drawable;

        public MainForm(string projectName, string authorName, string version)
        {
            Title = projectName;
            MinimumSize = new Size(200, 200);
            Size = new Size(800, 600);

            BackgroundColor = new Color(0, 0, 0);
            drawable = new Drawable(true);
            Content = drawable;

            Draw();
        }

        public void Draw()
        {
            Graphics graphics = drawable.CreateGraphics();
            graphics.DrawRectangle(new Color(1f, 0f, 0f), new RectangleF(0f, 0f, 100f, 50f));
        }
    }
}
