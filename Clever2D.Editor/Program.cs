using Eto.Drawing;
using Eto.Forms;
using System;

namespace Clever2D.Editor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Application(Eto.Platform.Detect).Run(new MainForm());
        }
    }
}
