using System;
using Clever2D;
using Clever2D.Desktop;
using Clever2D.Engine;
using Clever2D.Input;
using Eto.Forms;

namespace Example
{
    class Program
    {
        static MainForm form;

        [STAThread]
        static void Main(string[] args)
        {
            Player.Log("Starting...");

            ApplicationConfig config = new();

            Player.Log("Loading configurations...");

            config.ProjectName = "Example Project";
            config.AuthorName = "Company";
            config.Version = "0.1.0";

            Clever2D.Engine.Application.Config = config;

            Player.Log("Creating interface...");

            OperatingSystem os = System.Environment.OSVersion;

            if (os.Platform == PlatformID.Unix)
            {
                Eto.Forms.Application app = new(Eto.Platforms.Gtk);
                StartApp(app, config);
            }
            else
            {
                Player.LogError("Unsupported platform");
            }
        }

        private static void StartApp(Eto.Forms.Application app, ApplicationConfig config)
        {
            form = new MainForm(config.ProjectName, config.AuthorName, config.Version);

            //SceneManager.OnSceneDraw += SceneManager_OnSceneDraw;

            form.Shown += (object sender, EventArgs e) => {
                SceneManager.AddScenes(new Scene[] {
                    new MainScene()
                });

                Player.Log("Scenes loaded.");

                form.KeyDown += MainForm_KeyDown;
                form.KeyUp += MainForm_KeyUp;

                SceneManager.OnLoaded += (object sender, LoadedEventArgs e) =>
                {
                    if (SceneManager.Started)
                    {
                        Player.Log("\"" + SceneManager.LoadedScene.Name + "\" loaded.");
                        //SceneManager.LoadedScene.Draw();
                    }
                    else
                    {
                        Player.LogError("Scene loading failed.");
                        form.Close();
                        return;
                    }
                };

                SceneManager.Start();
            };

            app.Run(form);
        }

        //private static void SceneManager_OnSceneDraw(object sender, SceneDrawEventArgs e)
        //{
        //    form.Draw();
        //}

        private static void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Input.KeyPressed(e.Key.ToShortcutString());
        }
        private static void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            Input.KeyReleased(e.Key.ToShortcutString());
        }
    }
}
