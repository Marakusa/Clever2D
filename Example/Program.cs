using System;
using Clever2D;
using Clever2D.Desktop;
using Clever2D.Engine;
using Eto.Forms;

namespace Example
{
    class Program
    {
        static MainForm form;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            ApplicationConfig config = new();

            Console.WriteLine("Loading configurations...");

            config.ProjectName = "Example Project";
            config.AuthorName = "Company";
            config.Version = "0.1.0";

            Clever2D.Engine.Application.Config = config;

            Console.WriteLine("Creating interface...");

            Eto.Forms.Application app = new Eto.Forms.Application(Eto.Platforms.Wpf);
            form = new MainForm(config.ProjectName, config.AuthorName, config.Version);

            SceneManager.OnSceneDraw += SceneManager_OnSceneDraw;

            form.Shown += (object sender, EventArgs e) => {
                SceneManager.AddScenes(new Scene[] {
                    new MainScene()
                });

                Console.WriteLine("Scenes loaded.");

                SceneManager.OnLoaded += (object sender, LoadedEventArgs e) =>
                {
                    if (SceneManager.Started)
                    {
                        Console.WriteLine("\"" + SceneManager.LoadedScene.Name + "\" loaded.");
                        SceneManager.LoadedScene.Draw();
                    }
                    else
                    {
                        Console.WriteLine("Scene loading failed.");
                        form.Close();
                        return;
                    }
                };

                SceneManager.Start();
            };

            app.Run(form);
        }

        private static void SceneManager_OnSceneDraw(object sender, SceneDrawEventArgs e)
        {
            form.Draw();
        }
    }
}
