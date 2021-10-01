using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Clever2D.Desktop
{
    public class MainForm : Form
    {
        public Drawable canvas;

        public void CreateForm(string projectName = "Example", string authorName = "Example", string version = "0.1.0")
        {
            Eto.Forms.Application.Instance.Invoke((Action)delegate
            {
                Title = $"{authorName} - {projectName} {version}";
                MinimumSize = new Size(200, 200);
                Size = new Size(800, 600);

                BackgroundColor = new Color(0, 0, 0);
                Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
            });
        }
    }
}
