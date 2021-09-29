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
            ApplicationConfig config = new();

            config.ProjectName = "Example Project";
            config.AuthorName = "Company";
            config.Version = "0.1.0";

            Clever2D.Engine.Application.Config = config;

            MainForm form = new MainForm(config.ProjectName, config.AuthorName, config.Version);

            new Eto.Forms.Application(Eto.Platforms.Wpf).Run(form);

            SceneManager.AddScenes(new Scene[] { 
                new MainScene()
            });

            SceneManager.Start();

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
