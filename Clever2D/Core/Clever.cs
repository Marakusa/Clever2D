using Clever2D.Engine;
using Clever2D.Input;
using Clever2D.Threading;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using SDL2;
using System.Timers;

namespace Clever2D.Core
{
    /// <summary>
    /// The base core script of Clever2D game engine.
    /// </summary>
    public class Clever
    {
        private static IntPtr WindowHandle { get; set; } = IntPtr.Zero;
        
        /// <summary>
        /// Returns the current screen renderer.
        /// </summary>
        internal static IntPtr Renderer
        {
            get;
            private set;
        } = IntPtr.Zero;

        /// <summary>
        /// Returns the window focus status.
        /// </summary>
        public static bool Focused
        {
            get;
            set;
        }
        
        /// <summary>
        /// Returns the applications quit status.
        /// </summary>
        internal static bool Quit { get; set; }
        
        /// <summary>
        /// Returns the current window state.
        /// </summary>
        public static WindowState CurrentState { get; set; } = WindowState.Windowed;
        
        private const int DefaultWidth = 1366;
        private const int DefaultHeight = 768;
        
        private static Size _size = new Size(DefaultWidth, DefaultHeight);
        /// <summary>
        /// Returns or sets the window's internal size, before scaling.
        /// </summary>
        public static Size Size
        {
            get => _size;
            private set
            {
                if (value.Equals(_size)) return;

                _size = value;
                ScheduleEvent(() => Resized?.Invoke(_size));
            }
        }
        
        private static Point _position;
        /// <summary>
        /// Returns or sets the window's position in screen space.
        /// </summary>
        public static Point Position
        {
            get => _position;
            set
            {
                _position = value;
                ScheduleCommand(() => SDL.SDL_SetWindowPosition(WindowHandle, value.X, value.Y));
            }
        }
        
        private static Size _sizeFullscreen = new Size(1920, 1080);
        private static Size _sizeWindowed = new Size(DefaultWidth, DefaultHeight);

        private static bool _cursorInWindow;
        
        private static readonly Scheduler CommandScheduler = new();
        private static readonly Scheduler EventScheduler = new();

        /// <summary>
        /// Returns the loaded fonts.
        /// </summary>
        public static IntPtr[] Fonts
        {
            get;
            private set;
        }

        /// <summary>
        /// Registers a new scheduled command.
        /// </summary>
        /// <param name="action">Scheduled action.</param>
        private static void ScheduleCommand(Action action) => CommandScheduler.Add(action, false);

        /// <summary>
        /// Delegate for Clever finishing initialization.
        /// </summary>
        public delegate void Initialized();
        /// <summary>
        /// This event gets called when Clever finishes initialization.
        /// </summary>
        public static event Initialized OnInitialized;

        /// <summary>
        /// Initializes the engine and starts the game. Call Start() method after this to start the engine.
        /// </summary>
        protected static void Initialize(ApplicationConfig config)
        {
            Application.Config = config;

            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Player.LogError(string.Format("Unable to initialize SDL. Error: {0}", SDL.SDL_GetError()));
            }
            
            if (SDL_ttf.TTF_Init() < 0)
            {
                Player.LogError("There was an error initializing SDL_ttf. " + SDL.SDL_GetError());
            }

            List<IntPtr> fonts = new();
            fonts.Add(SDL_ttf.TTF_OpenFont(Application.ExecutableDirectory + "/res/fonts/Pixel-UniCode.ttf", 72));
            fonts.Add(SDL_ttf.TTF_OpenFont(Application.ExecutableDirectory + "/res/fonts/Poppins-Regular.ttf", 72));

            Fonts = fonts.ToArray();
            
            WindowHandle = SDL.SDL_CreateWindow(
                $"{Application.CompanyName} - {Application.ProductName} {Application.ProductVersion}",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                1280,
                720,
                SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
            );
            
            Renderer = SDL.SDL_CreateRenderer(WindowHandle, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            // VSync
            SDL.SDL_GL_SetSwapInterval(1);
            
            // TODO: Fix fullscreen
            /*SetScreenState += (WindowState state) =>
            {
                CurrentState = state;

                SDL.SDL_DisplayMode closestMode = GetClosestDisplayMode(sizeFullscreen, 144, 0);

                switch (CurrentState)
                {
                    case WindowState.Fullscreen:
                        
                        Size = new Size(closestMode.w, closestMode.h);

                        SDL.SDL_SetWindowDisplayMode(WindowHandle, ref closestMode);
                        SDL.SDL_SetWindowFullscreen(WindowHandle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);
                        
                        break;
                    case WindowState.Windowed:
                        
                        Size = new Size(defaultWidth, defaultHeight);

                        SDL.SDL_SetWindowBordered(WindowHandle, SDL.SDL_bool.SDL_TRUE);
                        SDL.SDL_SetWindowFullscreen(WindowHandle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                        
                        break;
                }
            };*/
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
                SDL.SDL_GetWindowPosition(WindowHandle, out int x, out int y);
                var newPosition = new Point(x, y);
                Position = newPosition;
                ScheduleEvent(() => Moved?.Invoke(newPosition));

                SDL.SDL_GetWindowSize(WindowHandle, out int w, out int h);
                var newSize = new Size(w, h);
                Size = newSize;
                ScheduleEvent(() => Resized?.Invoke(newSize));
                
                SDL.SDL_Event e;

                OnInitialized?.Invoke();

                System.Timers.Timer tickTimer = new()
                {
                    Interval = 1f / 30f
                };
                tickTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    FixedUpdate?.Invoke();
                };
                tickTimer.Start();

                ulong lastFpsTick = 0;
                ulong framesInFrame = 0;

                while (!Quit)
                {
                    CommandScheduler.Update();

                    if (SDL.SDL_GetTicks() - lastFpsTick >= 1000)
                    {
                        Fps = framesInFrame.ToString();
                        lastFpsTick = SDL.SDL_GetTicks();
                        framesInFrame = 0;
                    }
                    else
                    {
                        framesInFrame++;
                    }

                    while (SDL.SDL_PollEvent(out e) != 0)
                    {
                        switch (e.type)
                        {
                            case SDL.SDL_EventType.SDL_QUIT:
                            case SDL.SDL_EventType.SDL_APP_TERMINATING:
                                HandleQuitEvent(e.quit);
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
                    
                    EventScheduler.Update();
                    
                    Update?.Invoke();
                }
            }

            Player.Log("Closing application...");

            Destroying?.Invoke();

            Player.Log("Destroying SDL objects...");
            SDL.SDL_DestroyRenderer(Renderer);
            SDL.SDL_DestroyWindow(WindowHandle);

            Player.Log("Quit SDL TTF...");
            SDL_ttf.TTF_Quit();
            Player.Log("Quit SDL Image...");
            SDL_image.IMG_Quit();
            Player.Log("Quit SDL...");
            SDL.SDL_Quit();

            Destroyed?.Invoke();
        }
        /// <summary>
        /// Starts the engine and main loop.
        /// </summary>
        /// <param name="scenes">Scenes to load.</param>
        protected static void Start(Scene[] scenes)
        {
            SceneManager.AddScenes(scenes);
            Player.Log("Scenes loaded.");

            Thread inputThread = new(() =>
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
                    Player.Log($"\"{SceneManager.LoadedScene.Name}\" loaded.");
                }
                else
                {
                    Player.LogError("Scene loading failed.");
                    Quit = true;
                }
            };

            SceneManager.Initialize();
        }

        private static SDL.SDL_DisplayMode GetClosestDisplayMode(Size size, int refreshRate, int displayIndex)
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
        public static event Action Update;

        /// <summary>
        /// Frame-rate independent window event loop.
        /// </summary>
        public static event Action FixedUpdate;

        /// <summary>
        /// Invoked when the application has started to destroy.
        /// </summary>
        public static event Action Destroying;

        /// <summary>
        /// Invoked when the application has destroyed.
        /// </summary>
        public static event Action Destroyed;

        /// <summary>
        /// Invoked when mouse has entered the window.
        /// </summary>
        public static event Action MouseEntered;
        
        /// <summary>
        /// Invoked when mouse has left the window.
        /// </summary>
        public static event Action MouseLeft;

        /// <summary>
        /// Delegate handler for window position change.
        /// </summary>
        public delegate void WindowMovedHandler(Point position);
        /// <summary>
        /// Invoked after the window has moved.
        /// </summary>
        public static event WindowMovedHandler Moved;
        
        /// <summary>
        /// Delegate handler for window size change.
        /// </summary>
        public delegate void WindowResizedHandler(Size size);
        /// <summary>
        /// Invoked after the window has resized.
        /// </summary>
        public static event WindowResizedHandler Resized;
        
        private static void HandleQuitEvent(SDL.SDL_QuitEvent e)
        {
            Quit = true;
        }
        
        private static void HandleDropEvent(SDL.SDL_DropEvent evtDrop)
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

        private static void HandleMouseWheelEvent(SDL.SDL_MouseWheelEvent evtWheel)
        {
            // TODO: HandleMouseWheelEvent
            //ScheduleEvent(() => TriggerMouseWheel(new Vector2(evtWheel.x, evtWheel.y), false));
        }
        
        private static void HandleMouseButtonEvent(SDL.SDL_MouseButtonEvent evtButton)
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

        private static void HandleMouseMotionEvent(SDL.SDL_MouseMotionEvent evtMotion)
        {
            // TODO: HandleMouseButtonEvent
            /*if (SDL.SDL_GetRelativeMouseMode() == SDL.SDL_bool.SDL_FALSE)
                ScheduleEvent(() => MouseMove?.Invoke(new Vector2(evtMotion.x * Scale, evtMotion.y * Scale)));
            else
                ScheduleEvent(() => MouseMoveRelative?.Invoke(new Vector2(evtMotion.xrel * Scale, evtMotion.yrel * Scale)));*/
        }

        private static void HandleTextInputEvent(SDL.SDL_TextInputEvent evtText)
        {
            // TODO: HandleMouseButtonEvent
            /*var ptr = new IntPtr(evtText.text);
            if (ptr == IntPtr.Zero)
                return;

            string text = Marshal.PtrToStringUTF8(ptr) ?? "";

            foreach (char c in text)
                ScheduleEvent(() => KeyTyped?.Invoke(c));*/
        }

        private static void HandleTextEditingEvent(SDL.SDL_TextEditingEvent evtEdit)
        {
        }

        /// <summary>
        /// Delegate handler for key press.
        /// </summary>
        public delegate void KeyPressHandler(SDL.SDL_KeyboardEvent key);
        /// <summary>
        /// Delegate handler for key release.
        /// </summary>
        public delegate void KeyReleaseHandler(SDL.SDL_KeyboardEvent key);
        /// <summary>
        /// This event gets called when a key is pressed.
        /// </summary>
        public static event KeyPressHandler OnKeyPress;
        /// <summary>
        /// This event gets called when a key is released.
        /// </summary>
        public static event KeyReleaseHandler OnKeyRelease;

        private static void HandleKeyboardEvent(SDL.SDL_KeyboardEvent evtKey)
        {
            switch (evtKey.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    OnKeyPress?.Invoke(evtKey);
                    // TODO: Fullscreen fix
                    /*switch (evtKey.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_F11:
                            if (CurrentState == WindowState.Windowed)
                                ScheduleEvent(() => SetScreenState?.Invoke(WindowState.Fullscreen));
                            else if (CurrentState == WindowState.Fullscreen)
                                ScheduleEvent(() => SetScreenState?.Invoke(WindowState.Windowed));
                            break;
                    }*/
                    break;

                case SDL.SDL_EventType.SDL_KEYUP:
                    OnKeyRelease?.Invoke(evtKey);
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
        private delegate void ScreenState(WindowState state);

        private static event ScreenState SetScreenState;

        private static void HandleWindowEvent(SDL.SDL_WindowEvent evtWindow)
        {
            switch (evtWindow.windowEvent)
            {
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
                    
                    SDL.SDL_GetWindowPosition(WindowHandle, out int x, out int y);
                    var newPosition = new Point(x, y);

                    if (!newPosition.Equals(Position))
                    {
                        Position = newPosition;
                        ScheduleEvent(() => Moved?.Invoke(newPosition));
                    }

                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                    
                    SDL.SDL_GetWindowSize(WindowHandle, out int w, out int h);
                    var newSize = new Size(w, h);

                    if (!newSize.Equals(Size))
                    {
                        Size = newSize;
                        ScheduleEvent(() => Resized?.Invoke(newSize));
                    }

                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                    ScheduleEvent(() => Focused = true);
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                    ScheduleEvent(() => Focused = false);
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                    _cursorInWindow = true;
                    ScheduleEvent(() => MouseEntered?.Invoke());
                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
                    _cursorInWindow = false;
                    ScheduleEvent(() => MouseLeft?.Invoke());
                    break;
            }
        }

        /// <summary>
        /// Adds an Action to the Scheduler expected to handle event callbacks.
        /// </summary>
        /// <param name="action">The Action to execute.</param>
        private static void ScheduleEvent(Action action) => EventScheduler.Add(action, false);

        /// <summary>
        /// Returns the renderer fps.
        /// </summary>
        public static string Fps
        {
            get;
            private set;
        } = "0";
    }

    /// <summary>
    /// Window state.
    /// </summary>
    public enum WindowState
    {
        /// <summary>
        /// Windowed state.
        /// </summary>
        Windowed, 
        /*Borderless, */
        /// <summary>
        /// Fullscreen state.
        /// </summary>
        Fullscreen
    }
}
