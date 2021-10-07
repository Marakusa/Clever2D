using System;

namespace Clever2D.Engine
{
    /// <summary>
    /// The base class that manages the time of this Clever game.
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// The time at the beginning of the first frame.
        /// </summary>
        private static long _startTime;
        /// <summary>
        /// Returns the time from the beginning of the first frame.
        /// </summary>
        public static float TotalTime
        {
            get
            {
                var milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                return (milliseconds - _startTime) / 1000f;
            }
        }

        /// <summary>
        /// Returns the interval in seconds from the last frame to the current one.
        /// </summary>
        public static float DeltaTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the Time class.
        /// </summary>
        public static void Initialize()
        {
            _startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            var frameStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            System.Timers.Timer tickTimer = new();

            tickTimer.Interval = 1f;
            tickTimer.Elapsed += delegate
            {
                long frameEnd = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                DeltaTime = (frameEnd - frameStart) / 1000f;
                frameStart = frameEnd;
            };

            tickTimer.Start();
        }
    }
}
