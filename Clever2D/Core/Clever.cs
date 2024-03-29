﻿using Clever2D.Engine;
using Clever2D.Input;
using Clever2D.Threading;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using SDL2;
using System.Timers;
using static SDL2.SDL;

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
                ScheduleCommand(() => SDL_SetWindowPosition(WindowHandle, value.X, value.Y));
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
            
            string tempFolder = Application.TempDirectory;

            Player.Log($"Temp files located: {tempFolder}");

            if (Directory.Exists(tempFolder))
            {
                Directory.Delete(tempFolder);
                Directory.CreateDirectory(tempFolder);
            }
            else
                Directory.CreateDirectory(tempFolder);

            if (SDL_Init(SDL_INIT_VIDEO) < 0)
            {
                Player.LogError($"Unable to initialize SDL. Error: {SDL_GetError()}");
            }
            
            if (SDL_ttf.TTF_Init() < 0)
            {
                Player.LogError($"There was an error initializing SDL_ttf. {SDL_GetError()}");
            }

            List<IntPtr> fonts = new();
            fonts.Add(SDL_ttf.TTF_OpenFont(Application.ExecutableDirectory + "/res/fonts/Pixel-UniCode.ttf", 32));
            fonts.Add(SDL_ttf.TTF_OpenFont(Application.ExecutableDirectory + "/res/fonts/Poppins-Regular.ttf", 24));

            Fonts = fonts.ToArray();

            SDL_Init(SDL_INIT_VIDEO);
            
            WindowHandle = SDL_CreateWindow(
                $"{Application.CompanyName} - {Application.ProductName} {Application.ProductVersion}",
                SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED,
                1280,
                720,
                SDL_WindowFlags.SDL_WINDOW_OPENGL
            );

            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);

            SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK,
                SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);

            Renderer = SDL_GL_CreateContext(WindowHandle);
            
            //Renderer = SDL_CreateRenderer(WindowHandle, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            // VSync
            SDL_GL_SetSwapInterval(0);
            
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
                Player.Log(string.Format("Unable to create a window. Error: {0}", SDL_GetError()));
            }
            else
            {
                if (Renderer == IntPtr.Zero)
                {
                    Player.Log(string.Format("Unable to create a renderer. Error: {0}", SDL_GetError()));
                }
                else
                {
                    SDL_GetWindowPosition(WindowHandle, out int x, out int y);
                    var newPosition = new Point(x, y);
                    Position = newPosition;
                    ScheduleEvent(() => Moved?.Invoke(newPosition));

                    SDL_GetWindowSize(WindowHandle, out int w, out int h);
                    var newSize = new Size(w, h);
                    Size = newSize;
                    ScheduleEvent(() => Resized?.Invoke(newSize));

                    SDL_Event e;

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

                        if (SDL_GetTicks() - lastFpsTick >= 1000)
                        {
                            Fps = framesInFrame.ToString();
                            lastFpsTick = SDL_GetTicks();
                            framesInFrame = 0;
                        }
                        else
                        {
                            framesInFrame++;
                        }

                        while (SDL_PollEvent(out e) != 0)
                        {
                            switch (e.type)
                            {
                                case SDL_EventType.SDL_QUIT:
                                case SDL_EventType.SDL_APP_TERMINATING:
                                    HandleQuitEvent(e.quit);
                                    break;

                                case SDL_EventType.SDL_WINDOWEVENT:
                                    HandleWindowEvent(e.window);
                                    break;

                                case SDL_EventType.SDL_KEYDOWN:
                                case SDL_EventType.SDL_KEYUP:
                                    HandleKeyboardEvent(e.key);
                                    break;

                                case SDL_EventType.SDL_TEXTEDITING:
                                    HandleTextEditingEvent(e.edit);
                                    break;

                                case SDL_EventType.SDL_TEXTINPUT:
                                    HandleTextInputEvent(e.text);
                                    break;

                                case SDL_EventType.SDL_MOUSEMOTION:
                                    HandleMouseMotionEvent(e.motion);
                                    break;

                                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                                case SDL_EventType.SDL_MOUSEBUTTONUP:
                                    HandleMouseButtonEvent(e.button);
                                    break;

                                case SDL_EventType.SDL_MOUSEWHEEL:
                                    HandleMouseWheelEvent(e.wheel);
                                    break;

                                case SDL_EventType.SDL_DROPFILE:
                                case SDL_EventType.SDL_DROPTEXT:
                                case SDL_EventType.SDL_DROPBEGIN:
                                case SDL_EventType.SDL_DROPCOMPLETE:
                                    HandleDropEvent(e.drop);
                                    break;
                            }
                        }

                        EventScheduler.Update();

                        Update?.Invoke();
                    }
                }
            }

            Player.Log("Closing application...");

            Destroying?.Invoke();

            Player.Log("Destroying SDL objects...");
            
            try { SDL_DestroyRenderer(Renderer); }
            catch (Exception e) { Player.LogError($"Failed to destroy the renderer: {e.Message}", e); }
            
            try { SDL_DestroyWindow(WindowHandle); }
            catch (Exception e) { Player.LogError($"Failed to destroy the window: {e.Message}", e); }

            Player.Log("Quit SDL TTF...");
            try { SDL_ttf.TTF_Quit(); }
            catch (Exception e) { Player.LogError($"Failed to quit SDL TTF: {e.Message}", e); }

            Player.Log("Quit SDL Image...");
            try { SDL_image.IMG_Quit(); }
            catch (Exception e) { Player.LogError($"Failed to quit SDL Image: {e.Message}", e); }

            Player.Log("Quit SDL...");
            try { SDL_Quit(); }
            catch (Exception e) { Player.LogError($"Failed to quit SDL: {e.Message}", e); }

            Player.Log("Removing temp files...");
            try
            {
                if (Directory.Exists(Application.TempDirectory))
                    DeleteDirectory(Application.TempDirectory);
            }
            catch (Exception e) { Player.LogError($"Failed to remove temp files: {e.Message}", e); }

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
                    Player.Log($"\"{SceneManager.LoadedScene.name}\" loaded.");
                }
                else
                {
                    Player.LogError("Scene loading failed.");
                    Quit = true;
                }
            };

            SceneManager.Initialize();
        }

        private static void DeleteDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }
            Directory.Delete(path);
        }
        
        private static SDL_DisplayMode GetClosestDisplayMode(Size size, int refreshRate, int displayIndex)
        {
            var targetMode = new SDL_DisplayMode { w = size.Width, h = size.Height, refresh_rate = refreshRate };

            if (SDL_GetClosestDisplayMode(displayIndex, ref targetMode, out var mode) != IntPtr.Zero)
                return mode;

            // fallback to current display's native bounds
            targetMode.w = 1920;
            targetMode.h = 1080;
            targetMode.refresh_rate = 0;

            if (SDL_GetClosestDisplayMode(displayIndex, ref targetMode, out mode) != IntPtr.Zero)
                return mode;

            // finally return the current mode if everything else fails.
            // not sure this is required.
            if (SDL_GetWindowDisplayMode(WindowHandle, out mode) >= 0)
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
        
        private static void HandleQuitEvent(SDL_QuitEvent e)
        {
            Quit = true;
        }
        
        private static void HandleDropEvent(SDL_DropEvent evtDrop)
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

        private static void HandleMouseWheelEvent(SDL_MouseWheelEvent evtWheel)
        {
            // TODO: HandleMouseWheelEvent
            //ScheduleEvent(() => TriggerMouseWheel(new Vector2(evtWheel.x, evtWheel.y), false));
        }
        
        private static void HandleMouseButtonEvent(SDL_MouseButtonEvent evtButton)
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

        private static void HandleMouseMotionEvent(SDL_MouseMotionEvent evtMotion)
        {
            // TODO: HandleMouseButtonEvent
            /*if (SDL.SDL_GetRelativeMouseMode() == SDL.SDL_bool.SDL_FALSE)
                ScheduleEvent(() => MouseMove?.Invoke(new Vector2(evtMotion.x * Scale, evtMotion.y * Scale)));
            else
                ScheduleEvent(() => MouseMoveRelative?.Invoke(new Vector2(evtMotion.xrel * Scale, evtMotion.yrel * Scale)));*/
        }

        private static void HandleTextInputEvent(SDL_TextInputEvent evtText)
        {
            // TODO: HandleMouseButtonEvent
            /*var ptr = new IntPtr(evtText.text);
            if (ptr == IntPtr.Zero)
                return;

            string text = Marshal.PtrToStringUTF8(ptr) ?? "";

            foreach (char c in text)
                ScheduleEvent(() => KeyTyped?.Invoke(c));*/
        }

        private static void HandleTextEditingEvent(SDL_TextEditingEvent evtEdit)
        {
        }

        /// <summary>
        /// Delegate handler for key press.
        /// </summary>
        public delegate void KeyPressHandler(SDL_KeyboardEvent key);
        /// <summary>
        /// Delegate handler for key release.
        /// </summary>
        public delegate void KeyReleaseHandler(SDL_KeyboardEvent key);
        /// <summary>
        /// This event gets called when a key is pressed.
        /// </summary>
        public static event KeyPressHandler OnKeyPress;
        /// <summary>
        /// This event gets called when a key is released.
        /// </summary>
        public static event KeyReleaseHandler OnKeyRelease;

        private static void HandleKeyboardEvent(SDL_KeyboardEvent evtKey)
        {
            switch (evtKey.type)
            {
                case SDL_EventType.SDL_KEYDOWN:
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

                case SDL_EventType.SDL_KEYUP:
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

        private static void HandleWindowEvent(SDL_WindowEvent evtWindow)
        {
            switch (evtWindow.windowEvent)
            {
                case SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
                    
                    SDL_GetWindowPosition(WindowHandle, out int x, out int y);
                    var newPosition = new Point(x, y);

                    if (!newPosition.Equals(Position))
                    {
                        Position = newPosition;
                        ScheduleEvent(() => Moved?.Invoke(newPosition));
                    }

                    break;

                case SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                    
                    SDL_GetWindowSize(WindowHandle, out int w, out int h);
                    var newSize = new Size(w, h);

                    if (!newSize.Equals(Size))
                    {
                        Size = newSize;
                        ScheduleEvent(() => Resized?.Invoke(newSize));
                    }

                    break;

                case SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
                case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
                    ScheduleEvent(() => Focused = true);
                    break;

                case SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
                case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
                    ScheduleEvent(() => Focused = false);
                    break;

                case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
                    _cursorInWindow = true;
                    ScheduleEvent(() => MouseEntered?.Invoke());
                    break;

                case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
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
