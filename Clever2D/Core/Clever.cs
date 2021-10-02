using Clever2D.Engine;
using Clever2D.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using Cairo;

namespace Clever2D.Core
{
    /// <summary>
    /// The main window which will be displayed for the player.
    /// </summary>
    public class MainWindow : Window
    {
        [UI] private Box _box = null;

        /// <summary>
        /// The main window which will be displayed for the player.
        /// </summary>
        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            this.Title = $"{Engine.Application.CompanyName} - {Engine.Application.ProductName} {Engine.Application.ProductVersion}";
            this.DefaultSize = new Gdk.Size(800, 600);

            /*DrawingArea drawingArea = new DrawingArea();
            drawingArea.ExposeEvent += new ExposeEventHandler(ExposeEventCallback);

            drawingArea.Expand = true;
            drawingArea.Margin = 0;
            drawingArea.Draw()
            using (Context cr = drawingArea.Window)
            {
                int top = Allocation.Top;
                int left = Allocation.Left;

                cr.Rectangle(left + 8, top + 8, 10, 10);
                cr.SetSourceRGB(255, 0, 0);
                cr.Fill();
            }*/

            builder.Autoconnect(this);
        }
    }

    /// <summary>
    /// The base core script of Clever2D game engine.
    /// </summary>
    public class Clever
    {
        private MainWindow mainWindow = null;
        private Thread mainLoopThread = null;
        private Thread inputThread = null;

        /// <summary>
        /// Delegate for Clever drawing.
        /// </summary>
        public delegate void CleverDraw();
        /// <summary>
        /// This event gets called when Clever starts to draw on the screen.
        /// </summary>
        public event CleverDraw OnDraw;

        /// <summary>
        /// Delegate for Clever frame update.
        /// </summary>
        public delegate void CleverUpdate();
        /// <summary>
        /// This event gets called after Clever updates a frame.
        /// </summary>
        public event CleverUpdate OnUpdate;

        /// <summary>
        /// Delegate for Clever loaded core.
        /// </summary>
        public delegate void CleverLoad();
        /// <summary>
        /// This event gets called when Clever finishes loading the core and the window.
        /// </summary>
        public event CleverLoad OnLoad;

        /// <summary>
        /// Delegate for Clever finishing initialization.
        /// </summary>
        public delegate void Initialized();
        /// <summary>
        /// This event gets called when Clever finishes initialization.
        /// </summary>
        public event Initialized OnInitialized;

        /// <summary>
        /// Initializes the engine and starts the game. Call Start() method after this to start the engine.
        /// </summary>
        public void Initialize(ApplicationConfig config)
        {
            Engine.Application.Config = config;

            Gtk.Application.Init();

            var app = new Gtk.Application("org.GtkNamespace.GtkNamespace", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            mainWindow = new MainWindow();
            mainWindow.DeleteEvent += (object sender, DeleteEventArgs a) => { Engine.Application.Exit(); };
            mainWindow.Shown += (object sender, EventArgs e) =>
            {
                Time.Initialize();
                OnInitialized.Invoke();
            };
            app.AddWindow(mainWindow);

            mainWindow.Show();
            Gtk.Application.Run();
        }
        /// <summary>
        /// Starts the engine and main loop.
        /// </summary>
        /// <param name="scenes">Scenes to load.</param>
        public void Start(Scene[] scenes)
        {
            SceneManager.AddScenes(scenes);
            Player.Log("Scenes loaded.");

            inputThread = new(() =>
            {
                InputManager inputManager = new();
                inputManager.Initialize(mainWindow);
            });
            inputThread.Start();

            SceneManager.OnLoaded += (object sender, SceneEventArgs e) =>
            {
                if (SceneManager.IsInitialized)
                {
                    Player.Log("\"" + SceneManager.LoadedScene.Name + "\" loaded.");
                }
                else
                {
                    Player.LogError("Scene loading failed.");
                    mainWindow.Close();
                    return;
                }
            };

            SceneManager.Initialize();

            mainLoopThread = new(MainLoop);
            mainLoopThread.Start();
        }

        /// <summary>
        /// Core loop of Clever engine.
        /// </summary>
        private void MainLoop()
        {
            //OnLoad.Invoke();

            while (mainWindow.IsDrawable)
            {
                try
                {
                    //OnDraw.Invoke();

                    /*mainWindow.canvas.LineWidth = 0.5;

                    int width, height;
                    width = 30;
                    height = 30;

                    mainWindow.canvas.Translate(width / 2, height / 2);
                    mainWindow.canvas.Arc(0, 0, 120, 0, 2 * Math.PI);
                    mainWindow.canvas.Stroke();

                    mainWindow.canvas.Save();

                    for (int i = 0; i < 36; i++)
                    {
                        mainWindow.canvas.Rotate(i * Math.PI / 36);
                        mainWindow.canvas.Scale(0.3, 1);
                        mainWindow.canvas.Arc(0, 0, 120, 0, 2 * Math.PI);
                        mainWindow.canvas.Restore();
                        mainWindow.canvas.Stroke();
                        mainWindow.canvas.Save();
                    }

                    ((IDisposable)mainWindow.canvas.GetTarget()).Dispose();
                    ((IDisposable)mainWindow.canvas).Dispose();*/

                    //OnUpdate.Invoke();
                    //Player.Log(Time.TotalTime);
                    Player.Log((Time.DeltaTime * 60f) + " FPS");
                    Thread.Sleep(1);
                }
                catch
                {
                    Player.LogWarn("Game is loading...");
                }
            }
        }

        /*private void Paint()
        {
            Scene loadedScene = SceneManager.LoadedScene;
            if (loadedScene != null)
            {
                var instances = loadedScene.SpawnedGameObjects;

                if (instances.Count > 0)
                {
                    foreach (var instance in instances)
                    {
                        SpriteRenderer renderer = instance.Value.GetComponent<SpriteRenderer>();
                        if (renderer != null)
                        {
                            float scale = (main.Height / 600f) * 2f;
                            Bitmap bitmap = new(renderer.Sprite.Path);
                            Image image = new Bitmap(bitmap, (int)Math.Round(bitmap.Size.Width * scale), (int)Math.Round(bitmap.Size.Height * scale), ImageInterpolation.High);
                            e.Graphics.DrawImage(image, new PointF(instance.Value.transform.position.x * scale * instance.Value.transform.scale.x, -instance.Value.transform.position.y * scale * instance.Value.transform.scale.y));
                        }
                    }
                }
            }
        }*/

        /*private static Clever engineInstance;
        /// <summary>
        /// Returns the instnace of the engine.
        /// </summary>
        public static Clever EngineInstance
        {
            get
            {
                return engineInstance;
            }
        }

        /// <summary>
        /// Delegate for Clever finishing initialization.
        /// </summary>
        public delegate void Initialized(object sender);
        /// <summary>
        /// This event gets called when Clever finishes initialization.
        /// </summary>
        public event Initialized OnInitialized;

        /// <summary>
        /// Initializes the engine and starts the game. Call Start() method after this to start the engine.
        /// </summary>
        public void Initialize(ApplicationConfig config, Platform platform = Platform.Windows)
        {
            engineInstance = this;

            Engine.Application.Config = config;

            if (platform == Platform.Windows)
            {
                Eto.Forms.Application app = new(Eto.Platforms.Wpf);

                app.Initialized += (sender, e) =>
                {
                    OnInitialized.Invoke(this);
                };

                Player.Log(Thread.CurrentThread.ManagedThreadId);
                app.Run();
            }
        }
        /// <summary>
        /// Starts the engine and main loop.
        /// </summary>
        /// <param name="scenes">Scenes to load.</param>
        public static void Start(Scene[] scenes)
        {
            SceneManager.AddScenes(scenes);
            Player.Log("Scenes loaded.");

            Thread mainThread = new(MainLoop);
            mainThread.Start();
        }

        /// <summary>
        /// Core loop of Clever engine.
        /// </summary>
        private static void MainLoop()
        {
            MainForm main = new();

            main.CreateForm(Engine.Application.ProductName, Engine.Application.CompanyName, Engine.Application.ProductVersion);

            Eto.Forms.Application.Instance.Invoke((Action)delegate
            {
                Drawable canvas = new(false);

                main.canvas = canvas;
                main.Content = canvas;

                canvas.Paint += (object sender, PaintEventArgs e) =>
                {
                    Player.Log(Thread.CurrentThread.ManagedThreadId);
                    Scene loadedScene = SceneManager.LoadedScene;
                    if (loadedScene != null)
                    {
                        var instances = loadedScene.SpawnedGameObjects;

                        if (instances.Count > 0)
                        {
                            foreach (var instance in instances)
                            {
                                SpriteRenderer renderer = instance.Value.GetComponent<SpriteRenderer>();
                                if (renderer != null)
                                {
                                    float scale = (main.Height / 600f) * 2f;
                                    Bitmap bitmap = new(renderer.Sprite.Path);
                                    Image image = new Bitmap(bitmap, (int)Math.Round(bitmap.Size.Width * scale), (int)Math.Round(bitmap.Size.Height * scale), ImageInterpolation.High);
                                    e.Graphics.DrawImage(image, new PointF(instance.Value.transform.position.x * scale * instance.Value.transform.scale.x, -instance.Value.transform.position.y * scale * instance.Value.transform.scale.y));
                                }
                            }
                        }
                    }
                };

                main.Closed += (object sender, EventArgs e) =>
                {
                    main.Unbind();
                };

                System.Timers.Timer timer = new();
                timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    Eto.Forms.Application.Instance.Invoke((Action)delegate
                    {
                        main.canvas.Invalidate();
                    });
                };
                timer.Interval = 1f / 10f;
                timer.Start();
            });

            main.Shown += (object sender, EventArgs e) => {
                Thread inputThread = new(() =>
                {
                    InputManager inputManager = new();
                    inputManager.Initialize(main);
                });
                inputThread.Start();

                main.Closing += MainForm_Closing;

                SceneManager.OnLoaded += (object sender, SceneEventArgs e) =>
                {
                    if (SceneManager.IsInitialized)
                    {
                        Player.Log("\"" + SceneManager.LoadedScene.Name + "\" loaded.");
                    }
                    else
                    {
                        Player.LogError("Scene loading failed.");
                        main.Close();
                        return;
                    }
                };

                SceneManager.Initialize();
            };

            main.Show();
        }

        /// <summary>
        /// When the player stops the application, exit the process.
        /// </summary>
        private static void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }*/
    }

    /// <summary>
    /// Platform type.
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Windows based platform.
        /// </summary>
        Windows,
        /// <summary>
        /// Linux based platform (Partial support).
        /// </summary>
        Linux,
        /// <summary>
        /// MacOS based platform (Not supported yet).
        /// </summary>
        MacOS
    }
}
