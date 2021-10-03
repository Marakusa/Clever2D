﻿using Clever2D.Engine;
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
        internal static IntPtr WindowHandle { get; private set; } = IntPtr.Zero;
        private static IntPtr renderer = IntPtr.Zero;
        internal static IntPtr Renderer
        {
            get { return renderer; }
            private set { renderer = value; }
        }
        
        private static bool focused;
        private static bool Focused
        {
            get { return focused; }
            set { focused = value; }
        }
        private static bool Quit { get; set; } = false;
        private static WindowState CurrentState { get; set; } = WindowState.Windowed;
        
        private static Thread mainLoopThread = null;
        private static Thread inputThread = null;

        private const int defaultWidth = 1366;
        private const int defaultHeight = 768;
        
        private static Size size = new Size(defaultWidth, defaultHeight);
        /// <summary>
        /// Returns or sets the window's internal size, before scaling.
        /// </summary>
        public static Size Size
        {
            get => size;
            private set
            {
                if (value.Equals(size)) return;

                size = value;
                ScheduleEvent(() => Resized?.Invoke(size));
            }
        }
        
        private static Point position;
        /// <summary>
        /// Returns or sets the window's position in screen space.
        /// </summary>
        public static Point Position
        {
            get => position;
            set
            {
                position = value;
                ScheduleCommand(() => SDL.SDL_SetWindowPosition(WindowHandle, value.X, value.Y));
            }
        }
        
        private static readonly Size sizeFullscreen = new Size(1920, 1080);
        private static readonly Size sizeWindowed = new Size(defaultWidth, defaultHeight);
        
        private static readonly Scheduler commandScheduler = new();
        private static readonly Scheduler eventScheduler = new();
        
        protected static void ScheduleCommand(Action action) => commandScheduler.Add(action, false);

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
        public static void Initialize(ApplicationConfig config)
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
            
            SetScreenState += (WindowState state) =>
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
                    /*case WindowState.Borderless:
                        
                        Size = new Size(closestMode.w, closestMode.h);

                        SDL.SDL_SetWindowBordered(WindowHandle, SDL.SDL_bool.SDL_FALSE);
                        SDL.SDL_SetWindowFullscreen(WindowHandle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_MAXIMIZED);
                        
                        break;*/
                    case WindowState.Windowed:
                        
                        Size = new Size(defaultWidth, defaultHeight);

                        SDL.SDL_SetWindowBordered(WindowHandle, SDL.SDL_bool.SDL_TRUE);
                        SDL.SDL_SetWindowFullscreen(WindowHandle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                        
                        break;
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

                OnInitialized?.Invoke();
                
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
                    
                    SDL.SDL_SetRenderDrawColor(renderer, 30, 30, 100, 255);
            
                    SDL.SDL_RenderClear(renderer);

                    Scene loadedScene = SceneManager.LoadedScene;
                    
                    if (loadedScene != null)
                    {
                        var instances = loadedScene.SpawnedGameObjects;

                        if (instances.Count > 0)
                        {
                            foreach (var instance in instances)
                            {
                                SpriteRenderer spriteRenderer = instance.Value.GetComponent<SpriteRenderer>();
                                
                                if (spriteRenderer != null)
                                {
                                    float scale = (Size.Height / 600f) * 2f;
                                    
                                    SDL.SDL_Rect tRect;
                                    tRect.x = (int)Math.Round(instance.Value.transform.position.x * scale * instance.Value.transform.scale.x);
                                    tRect.y = (int)Math.Round(instance.Value.transform.position.y * scale * instance.Value.transform.scale.y);
                                    tRect.w = (int)Math.Round(spriteRenderer.Sprite.rect.w * scale);
                                    tRect.h = (int)Math.Round(spriteRenderer.Sprite.rect.h * scale);
                                    //tRect.x = 100;
                                    //tRect.y = 100;
                                    //tRect.w = 64;
                                    //tRect.h = 64;

                                    SDL.SDL_RenderCopy(renderer, spriteRenderer.Sprite.image, ref spriteRenderer.Sprite.rect, ref tRect);
                                    //SDL.SDL_RenderCopy(renderer, playerTexture, IntPtr.Zero, IntPtr.Zero);
                                }
                            }
                        }
                    }
                    
                    SDL.SDL_RenderPresent(renderer);

                    Update?.Invoke();
                }

                Destroying?.Invoke();
                
                SDL.SDL_DestroyRenderer(renderer);
                SDL.SDL_DestroyWindow(WindowHandle);

                SDL.SDL_Quit();
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
        /// Invoked when the application has started to destroy.
        /// </summary>
        public static event Action Destroying;

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

        private static void HandleKeyboardEvent(SDL.SDL_KeyboardEvent evtKey)
        {
            switch (evtKey.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    switch (evtKey.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_F11:
                            if (CurrentState == WindowState.Windowed)
                                ScheduleEvent(() => SetScreenState?.Invoke(WindowState.Fullscreen));
                            else if (CurrentState == WindowState.Fullscreen)
                                ScheduleEvent(() => SetScreenState?.Invoke(WindowState.Windowed));
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
                        position = newPosition;
                        ScheduleEvent(() => Moved?.Invoke(newPosition));
                    }

                    break;

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                    
                    SDL.SDL_GetWindowSize(WindowHandle, out int w, out int h);
                    var newSize = new Size(w, h);

                    if (!newSize.Equals(Size))
                    {
                        size = newSize;
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

                case SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                    break;
            }
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
        private static void ScheduleEvent(Action action) => eventScheduler.Add(action, false);

        /// <summary>
        /// Returns the main threads fps
        /// </summary>
        /// <param name="format">Format the fps output.</param>
        public static string GetFPS(string format = "0.00 fps")
        {
            return (1f / (Time.DeltaTime / 1000f)).ToString(format);
        }
    }

    public enum WindowState
    {
        Windowed, /*Borderless, */Fullscreen
    }
}
