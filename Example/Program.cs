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
            SceneManager.AddScenes(new Scene[] {
                new MainScene()
            });

            SceneManager.Start();

            ApplicationConfig config = new();

            config.ProjectName = "Example Project";
            config.AuthorName = "Company";
            config.Version = "0.1.0";

            Clever2D.Engine.Application.Config = config;

            Eto.Forms.Application app = new Eto.Forms.Application(Eto.Platforms.Wpf);
            MainForm form = new MainForm(config.ProjectName, config.AuthorName, config.Version);

            app.Run(form);

            if (SceneManager.Started)
            {
                Console.WriteLine("Scene loaded.");
            }
            else
            {
                form.Close();
            }
        }
    }
}
