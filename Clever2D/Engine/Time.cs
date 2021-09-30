using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Clever2D.Engine
{
    public static class Time
    {
        private static long startTime = 0;
        /// <summary>
        /// The time at the beginning of this frame.
        /// </summary>
        public static float TotalTime
        {
            get
            {
                return (float)((DateTime.Now.Ticks - startTime) / 1000f);
            }
        }

        private static float deltaTime = 0f;
        /// <summary>
        /// The interval in seconds from the last frame to the current one.
        /// </summary>
        public static float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }

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
