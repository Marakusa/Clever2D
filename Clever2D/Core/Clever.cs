using Clever2D.Engine;
using Clever2D.Input;
using Clever2D.Threading;
using System;
using System.Drawing;
using System.Threading;
using SDL2;

namespace Clever2D.Core
{
    /// <summary>
    /// The base core script of Clever2D game engine.
    /// </summary>
    public class Clever
    {
        internal IntPtr WindowHandle { get; private set; } = IntPtr.Zero;
        private IntPtr renderer = IntPtr.Zero;
        
        private bool focused;
        private bool Focused
        {
            get { return focused; }
            set { focused = value; }
        }
        private bool Quit { get; set; } = false;
        private bool CurrentState { get; set; } = false;
        
        private Thread mainLoopThread = null;
        private Thread inputThread = null;

        private const int defaultWidth = 1366;
        private const int defaultHeight = 768;
        
        private Size size = new Size(defaultWidth, defaultHeight);
        /// <summary>
        /// Returns or sets the window's internal size, before scaling.
        /// </summary>
        public Size Size
        {
            get => size;
            private set
            {
                if (value.Equals(size)) return;

                size = value;
                ScheduleEvent(() => Resized?.Invoke());
            }
        }
        
        private Vector2Int position;
        /// <summary>
        /// Returns or sets the window's position in screen space.
        /// </summary>
        public Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                ScheduleCommand(() => SDL.SDL_SetWindowPosition(WindowHandle, value.x, value.y));
            }
        }
        
        private readonly Size sizeFullscreen = new Size(1920, 1080);
        private readonly Size sizeWindowed = new Size(defaultWidth, defaultHeight);
        
        private readonly Scheduler commandScheduler = new();
        private readonly Scheduler eventScheduler = new();
        
        protected void ScheduleCommand(Action action) => commandScheduler.Add(action, false);

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

            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Player.Log(string.Format("Unable to initialize SDL. Error: {0}", SDL.SDL_GetError()));
            }

            WindowHandle = SDL.SDL_CreateWindow(
                $"{Application.CompanyName} - {Application.ProductName} {Application.ProductVersion}",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                1280,
                720,
                SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
            );
            
            renderer = SDL.SDL_CreateRenderer(WindowHandle, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            
            IntPtr bitmapSurface = SDL.SDL_LoadBMP("img/hello.bmp");
            IntPtr bitmapTex = SDL.SDL_CreateTextureFromSurface(renderer, bitmapSurface);
            SDL.SDL_FreeSurface(bitmapSurface);

            SetScreenState += (bool state) =>
            {
                CurrentState = state;
                if (CurrentState)
                {
                    var closestMode = GetClosestDisplayMode(sizeFullscreen, 144, 0);

                    Size = new Size(closestMode.w, closestMode.h);

                    SDL.SDL_SetWindowDisplayMode(WindowHandle, ref closestMode);
                    SDL.SDL_SetWindowFullscreen(WindowHandle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);
                }
                else
                {
                    SDL.SDL_SetWindowBordered(WindowHandle, SDL.SDL_bool.SDL_TRUE);
                    SDL.SDL_SetWindowFullscreen(WindowHandle, (uint)SDL.SDL_bool.SDL_FALSE);
                }
            };
            //Resized += () =>
            //{
            //    
            //};

            if (WindowHandle == IntPtr.Zero)
            {
                Player.Log(string.Format("Unable to create a window. Error: {0}", SDL.SDL_GetError()));
            }
            else
            {
                SDL.SDL_Event e;
                
                while (!Quit)
                {
                    commandScheduler.Update();

                    while (SDL.SDL_PollEvent(out e) != 0)
                    {
                        switch (e.type)
                        {
                            case SDL.SDL_EventType.SDL_QUIT:
                            case SDL.SDL_EventType.SDL_APP_TERMINATING:
                                HandleQuitEvent(e.quit);
                                Quit = true;
                                break;

                            case SDL.SDL_EventType.SDL_WINDOWEVENT:
                                HandleWindowEvent(e.window);
                                break;

                            case SDL.SDL_EventType.SDL_KEYDOWN:
                            case SDL.SDL_EventType.SDL_KEYUP:
                                HandleKeyboardEvent(e.key);
                                break;

                            case SDL.SDL_EventType.SDL_TEXTEDITING:
                                HandleTextEditingEvent(e.edit);
                                break;

                            case SDL.SDL_EventType.SDL_TEXTINPUT:
                                HandleTextInputEvent(e.text);
                                break;

                            case SDL.SDL_EventType.SDL_MOUSEMOTION:
                                HandleMouseMotionEvent(e.motion);
                                break;

                            case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                            case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                                HandleMouseButtonEvent(e.button);
                                break;

                            case SDL.SDL_EventType.SDL_MOUSEWHEEL:
                                HandleMouseWheelEvent(e.wheel);
                                break;

                            case SDL.SDL_EventType.SDL_DROPFILE:
                            case SDL.SDL_EventType.SDL_DROPTEXT:
                            case SDL.SDL_EventType.SDL_DROPBEGIN:
                            case SDL.SDL_EventType.SDL_DROPCOMPLETE:
                                HandleDropEvent(e.drop);
                                break;
                        }
                    }
                    
                    eventScheduler.Update();

                    Update?.Invoke();

                    SDL.SDL_RenderClear(renderer);
                    SDL.SDL_RenderCopy(renderer, bitmapTex, IntPtr.Zero, IntPtr.Zero);
                    SDL.SDL_RenderPresent(renderer);
                }
                
                SDL.SDL_DestroyTexture(bitmapTex);
                SDL.SDL_DestroyRenderer(renderer);
                SDL.SDL_DestroyWindow(WindowHandle);
                
                SDL.SDL_Quit();
            }
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
                inputManager.Initialize();
            });
            inputThread.Start();

            Time.Initialize();
            
            SceneManager.OnLoaded += (object sender, SceneEventArgs e) =>
            {
                if (SceneManager.IsInitialized)
                {
                    Player.Log("\"" + SceneManager.LoadedScene.Name + "\" loaded.");
                }
                else
                {
                    Player.LogError("Scene loading failed.");
                    Quit = true;
                    return;
                }
            };

            SceneManager.Initialize();

            //mainLoopThread = new(MainLoop);
            //mainLoopThread.Start();
        }

        private SDL.SDL_DisplayMode GetClosestDisplayMode(Size size, int refreshRate, int displayIndex)
        {
            var targetMode = new SDL.SDL_DisplayMode { w = size.Width, h = size.Height, refresh_rate = refreshRate };

            if (SDL.SDL_GetClosestDisplayMode(displayIndex, ref targetMode, out var mode) != IntPtr.Zero)
                return mode;

            // fallback to current display's native bounds
            targetMode.w = 1920;
            targetMode.h = 1080;
            targetMode.refresh_rate = 0;

            if (SDL.SDL_GetClosestDisplayMode(displayIndex, ref targetMode, out mode) != IntPtr.Zero)
                return mode;

            // finally return the current mode if everything else fails.
            // not sure this is required.
            if (SDL.SDL_GetWindowDisplayMode(WindowHandle, out mode) >= 0)
                return mode;

            throw new InvalidOperationException("couldn't retrieve valid display mode");
        }

        /// <summary>
        /// Invoked once every window event loop.
        /// </summary>
        public event Action Update;

        /// <summary>
        /// Invoked after the window has resized.
        /// </summary>
        public event Action Resized;
        
        private void HandleQuitEvent(SDL.SDL_QuitEvent e)
        {
            Quit = true;
        }
        
        private void HandleDropEvent(SDL.SDL_DropEvent evtDrop)
        {
            // TODO: HandleDropEvent
            /*switch (evtDrop.type)
            {
                case SDL.SDL_EventType.SDL_DROPFILE:
                    var str = SDL.UTF8_ToManaged(evtDrop.file, true);
                    if (str != null)
                        ScheduleEvent(() => DragDrop?.Invoke(str));

                    break;
            }*/
        }

        private void HandleMouseWheelEvent(SDL.SDL_MouseWheelEvent evtWheel)
        {
            // TODO: HandleMouseWheelEvent
            //ScheduleEvent(() => TriggerMouseWheel(new Vector2(evtWheel.x, evtWheel.y), false));
        }
        
        private void HandleMouseButtonEvent(SDL.SDL_MouseButtonEvent evtButton)
        {
            // TODO: HandleMouseButtonEvent
            /*MouseButton button = mouseButtonFromEvent(evtButton.button);

            switch (evtButton.type)
            {
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    ScheduleEvent(() => MouseDown?.Invoke(button));
                    break;

                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    ScheduleEvent(() => MouseUp?.Invoke(button));
                    break;
            }*/
        }

        private void HandleMouseMotionEvent(SDL.SDL_MouseMotionEvent evtMotion)
        {
            // TODO: HandleMouseButtonEvent
            /*if (SDL.SDL_GetRelativeMouseMode() == SDL.SDL_bool.SDL_FALSE)
                ScheduleEvent(() => MouseMove?.Invoke(new Vector2(evtMotion.x * Scale, evtMotion.y * Scale)));
            else
                ScheduleEvent(() => MouseMoveRelative?.Invoke(new Vector2(evtMotion.xrel * Scale, evtMotion.yrel * Scale)));*/
        }

        private void HandleTextInputEvent(SDL.SDL_TextInputEvent evtText)
        {
            // TODO: HandleMouseButtonEvent
            /*var ptr = new IntPtr(evtText.text);
            if (ptr == IntPtr.Zero)
                return;

            string text = Marshal.PtrToStringUTF8(ptr) ?? "";

            foreach (char c in text)
                ScheduleEvent(() => KeyTyped?.Invoke(c));*/
        }

        private void HandleTextEditingEvent(SDL.SDL_TextEditingEvent evtEdit)
        {
        }

        private void HandleKeyboardEvent(SDL.SDL_KeyboardEvent evtKey)
        {
            switch (evtKey.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    switch (evtKey.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_F11:
                            ScheduleEvent(() => SetScreenState?.Invoke(!CurrentState));
                            break;
                    }
                    break;

                case SDL.SDL_EventType.SDL_KEYUP:
                    //ScheduleEvent(() => KeyUp?.Invoke(key));
                    break;
            }
            // TODO: HandleMouseButtonEvent
            /*Key key = evtKey.keysym.ToKey();

            if (key == Key.Unknown)
                return;

            switch (evtKey.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    ScheduleEvent(() => KeyDown?.Invoke(key));
                    break;

                case SDL.SDL_EventType.SDL_KEYUP:
                    ScheduleEvent(() => KeyUp?.Invoke(key));
                    break;
            }*/
        }
        private delegate void ScreenState(bool state);

        private event ScreenState SetScreenState;

        private void HandleWindowEvent(SDL.SDL_WindowEvent evtWindow)
        {
            // TODO: HandleMouseButtonEvent
            /*updateWindowSpecifics();

            switch (evtWindow.windowEvent)
            {
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
                    // explicitly requery as there are occasions where what SDL has provided us with is not up-to-date.
                    SDL.SDL_GetWindowPosition(SDLWindowHandle, out int x, out int y);
                    var newPosition = new Point(x, y);

                    if (!newPosition.Equals(Position))
                    {
                        position = newPosition;
                        ScheduleEvent(() => Moved?.Invoke(newPosition));

                        if (WindowMode.Value == Configuration.WindowMode.Windowed)
                            storeWindowPositionToConfig();
                    }

                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                    updateWindowSize();
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                    cursorInWindow.Value = true;
                    ScheduleEvent(() => MouseEntered?.Invoke());
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
                    cursorInWindow.Value = false;
                    ScheduleEvent(() => MouseLeft?.Invoke());
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                    ScheduleEvent(() => Focused = true);
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                    ScheduleEvent(() => Focused = false);
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                    break;
            }*/
        }

        /// <summary>
        /// Adds an Action to the Scheduler expected to handle event callbacks.
        /// </summary>
        /// <param name="action">The Action to execute.</param>
        private void ScheduleEvent(Action action) => eventScheduler.Add(action, false);

        /// <summary>
        /// Returns the main threads fps
        /// </summary>
        /// <param name="format">Format the fps output.</param>
        public static string GetFPS(string format = "0.00 fps")
        {
            return (1f / (Time.DeltaTime / 1000f)).ToString(format);
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
    }
}
