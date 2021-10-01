using System;
using Clever2D.Desktop;
using Clever2D.Engine;
using Clever2D.Input;
using Eto.Forms;

namespace Example
{
    class Program
    {
        private static MainForm form;

        [STAThread]
        static void Main(string[] args)
        {
            Player.Log("Starting...");

            Time.Initialize();

            ApplicationConfig config = new()
            {
                ProductName = "Example Project",
                CompanyName = "Company",
                Version = "0.1.0"
            };

            Player.Log("Loading configurations...");

            Clever2D.Engine.Application.Config = config;

            Player.Log("Creating interface...");

            OperatingSystem os = System.Environment.OSVersion;

            if (os.Platform == PlatformID.Win32NT)
            {
                Eto.Forms.Application app = new(Eto.Platforms.Wpf);

                app.Initialized += (sender, e) => {
                    form = new MainForm(config.ProductName, config.CompanyName, config.Version);

                    form.Shown += (object sender, EventArgs e) => {
                        SceneManager.AddScenes(new Scene[] {
                            new MainScene()
                        });

                        Player.Log("Scenes loaded.");

                        form.KeyDown += MainForm_KeyDown;
                        form.KeyUp += MainForm_KeyUp;

                        form.Closing += MainForm_Closing;

                        SceneManager.OnLoaded += (object sender, SceneEventArgs e) =>
                        {
                            if (SceneManager.IsInitialized)
                            {
                                Player.Log("\"" + SceneManager.LoadedScene.Name + "\" loaded.");
                            }
                            else
                            {
                                Player.LogError("Scene loading failed.");
                                form.Close();
                                return;
                            }
                        };

                        SceneManager.Initialize();
                    };

                    form.Show();
                };

                app.Run();
            }
            else
            {
                Player.LogError("Unsupported platform");
            }
        }

        private static void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

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
