using System;
using Clever2D;
using Clever2D.Desktop;
using Clever2D.Engine;
using Eto.Forms;

namespace Example
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Clever2D.Engine.ApplicationConfig config = new Clever2D.Engine.ApplicationConfig();

            config.ProjectName = "Example Project";
            config.AuthorName = "Company";
            config.Version = "0.1.0";

            Clever2D.Engine.Application.Config = config;

            new Eto.Forms.Application(Eto.Platforms.Wpf).Run(new MainForm(config.ProjectName, config.AuthorName, config.Version));
        }
    }
}
