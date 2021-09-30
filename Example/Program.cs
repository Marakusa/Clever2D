using System;
using System.Threading;
using Clever2D.Desktop;
using Clever2D.Engine;
using Clever2D.Input;
using Clever2D.Threading;
using Eto.Forms;

namespace Example
{
    class Program
    {
        private static MainForm form;

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

            OperatingSystem os = System.Environment.OSVersion;

            if (os.Platform == PlatformID.Win32NT)
            {
                Eto.Forms.Application app = new(Eto.Platforms.Wpf);

                app.Initialized += (sender, e) => {
                    form = new MainForm(config.ProjectName, config.AuthorName, config.Version);

                    form.Shown += (object sender, EventArgs e) => {
                        SceneManager.AddScenes(new Scene[] {
                            new MainScene()
                        });

                        Console.WriteLine("Scenes loaded.");

                        form.KeyDown += MainForm_KeyDown;
                        form.KeyUp += MainForm_KeyUp;

                        SceneManager.OnLoaded += (object sender, LoadedEventArgs e) =>
                        {
                            if (SceneManager.Started)
                            {
                                Console.WriteLine("\"" + SceneManager.LoadedScene.Name + "\" loaded.");
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

                    form.Show();
                };

                app.Run();
            }
            else
            {
                Console.WriteLine("Unsupported platform");
            }
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
