using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

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
        private static long startTime = 0;
        /// <summary>
        /// Returns the time from the beginning of the first frame.
        /// </summary>
        public static float TotalTime
        {
            get
            {
                return (float)((DateTime.Now.Ticks - startTime) / 1000f);
            }
        }

        /// <summary>
        /// The interval in seconds from the last frame to the current one.
        /// </summary>
        private static float deltaTime = 0f;
        /// <summary>
        /// Returns the interval in seconds from the last frame to the current one.
        /// </summary>
        public static float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }

        /// <summary>
        /// Initializes the Time class.
        /// </summary>
        public static void Initialize()
        {
            startTime = DateTime.Now.Ticks;

            Thread thread = new(() =>
            {
                System.Timers.Timer tickTimer = new();

                DateTime frameStart = DateTime.Now;
                DateTime frameEnd = DateTime.Now;

                tickTimer.Interval = 1f / 60f;
                tickTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    frameEnd = DateTime.Now;
                    deltaTime = (float)(frameEnd - frameStart).TotalMilliseconds / 1000f;
                    frameStart = DateTime.Now;
                };

                tickTimer.Start();
            });

            thread.Start();
        }
    }
}
