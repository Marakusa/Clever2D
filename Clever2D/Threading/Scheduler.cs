// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Clever2D.Core;
using Clever2D.Timing;

namespace Clever2D.Threading
{
    /// <summary>
    /// Marshals delegates to run from the Scheduler's base thread in a threadsafe manner
    /// </summary>
    public class Scheduler
    {
        private readonly Queue<ScheduledDelegate> runQueue = new Queue<ScheduledDelegate>();
        private readonly List<ScheduledDelegate> timedTasks = new List<ScheduledDelegate>();
        private readonly List<ScheduledDelegate> perUpdateTasks = new List<ScheduledDelegate>();

        private readonly Func<bool> isCurrentThread;

        private IClock clock;
        private double currentTime => clock?.CurrentTime ?? 0;

        private readonly object queueLock = new object();

        /// <summary>
        /// Whether there are any tasks queued to run (including delayed tasks in the future).
        /// </summary>
        public bool HasPendingTasks => TotalPendingTasks > 0;

        /// <summary>
        /// The total number of <see cref="ScheduledDelegate"/>s tracked by this instance for future execution.
        /// </summary>
        internal int TotalPendingTasks => runQueue.Count + timedTasks.Count + perUpdateTasks.Count;

        /// <summary>
        /// The base thread is assumed to be the thread on which the constructor is run.
        /// </summary>
        public Scheduler()
        {
            var currentThread = Thread.CurrentThread;
            isCurrentThread = () => Thread.CurrentThread == currentThread;

            clock = new StopwatchClock(true);
        }

        /// <summary>
        /// The base thread is assumed to be the thread on which the constructor is run.
        /// </summary>
        public Scheduler(Func<bool> isCurrentThread, IClock clock)
        {
            this.isCurrentThread = isCurrentThread;
            this.clock = clock;
        }

        public void UpdateClock(IClock newClock)
        {
            if (newClock == null)
                throw new ArgumentNullException(nameof(newClock));

            if (newClock == clock)
                return;

            lock (queueLock)
            {
                if (clock == null)
                {
                    // This is the first time we will get a valid time, so assume this is the
                    // reference point everything scheduled so far starts from.
                    foreach (var s in timedTasks)
                        s.ExecutionTime += newClock.CurrentTime;
                }

                clock = newClock;
            }
        }

        /// <summary>
        /// Returns whether we are on the main thread or not.
        /// </summary>
        protected bool IsMainThread => isCurrentThread?.Invoke() ?? true;

        private readonly List<ScheduledDelegate> tasksToSchedule = new List<ScheduledDelegate>();
        private readonly List<ScheduledDelegate> tasksToRemove = new List<ScheduledDelegate>();

        /// <summary>
        /// Run any pending work tasks.
        /// </summary>
        /// <returns>The number of tasks that were run.</returns>
        public virtual int Update()
        {
            lock (queueLock)
            {
                queueTimedTasks();
                queuePerUpdateTasks();
            }

            int countToRun = runQueue.Count;
            int countRun = 0;

            while (getNextTask(out ScheduledDelegate sd))
            {
                //todo: error handling
                sd.RunTaskInternal();

                if (++countRun == countToRun)
                    break;
            }

            return countRun;
        }

        private void queueTimedTasks()
        {
            double currentTimeLocal = currentTime;

            if (timedTasks.Count > 0)
            {
                foreach (var sd in timedTasks)
                {
                    if (sd.ExecutionTime <= currentTimeLocal)
                    {
                        tasksToRemove.Add(sd);

                        if (sd.Cancelled) continue;

                        if (sd.RepeatInterval == 0)
                        {
                            // handling of every-frame tasks is slightly different to reduce overhead.
                            perUpdateTasks.Add(sd);
                            continue;
                        }

                        if (sd.RepeatInterval > 0)
                        {
                            if (timedTasks.Count > 1000)
                                throw new ArgumentException("Too many timed tasks are in the queue!");

                            // schedule the next repeat of the task.
                            sd.SetNextExecution(currentTimeLocal);
                            tasksToSchedule.Add(sd);
                        }

                        if (!sd.Completed) runQueue.Enqueue(sd);
                    }
                }

                foreach (var t in tasksToRemove)
                    timedTasks.Remove(t);

                tasksToRemove.Clear();

                foreach (var t in tasksToSchedule)
                    timedTasks.AddInPlace(t);

                tasksToSchedule.Clear();
            }
        }

        private void queuePerUpdateTasks()
        {
            for (int i = 0; i < perUpdateTasks.Count; i++)
            {
                ScheduledDelegate task = perUpdateTasks[i];

                task.SetNextExecution(null);

                if (task.Cancelled)
                {
                    perUpdateTasks.RemoveAt(i--);
                    continue;
                }

                runQueue.Enqueue(task);
            }
        }

        private bool getNextTask(out ScheduledDelegate task)
        {
            lock (queueLock)
            {
                if (runQueue.Count > 0)
                {
                    task = runQueue.Dequeue();
                    return true;
                }
            }

            task = null;
            return false;
        }

        /// <summary>
        /// Cancel any pending work tasks.
        /// </summary>
        public void CancelDelayedTasks()
        {
            lock (queueLock)
            {
                foreach (var t in timedTasks)
                    t.Cancel();
                timedTasks.Clear();
            }
        }

        /// <summary>
        /// Add a task to be scheduled.
        /// </summary>
        /// <remarks>If scheduled, the task will be run on the next <see cref="Update"/> independent of the current clock time.</remarks>
        /// <param name="task">The work to be done.</param>
        /// <param name="forceScheduled">If set to false, the task will be executed immediately if we are on the main thread.</param>
        /// <returns>The scheduled task, or <c>null</c> if the task was executed immediately.</returns>
        public ScheduledDelegate Add(Action task, bool forceScheduled = true)
        {
            if (!forceScheduled && IsMainThread)
            {
                //We are on the main thread already - don't need to schedule.
                task.Invoke();
                return null;
            }

            var del = new ScheduledDelegate(task);

            lock (queueLock)
                runQueue.Enqueue(del);

            return del;
        }

        /// <summary>
        /// Add a task to be scheduled.
        /// </summary>
        /// <remarks>The task will be run on the next <see cref="Update"/> independent of the current clock time.</remarks>
        /// <param name="task">The scheduled delegate to add.</param>
        /// <exception cref="InvalidOperationException">Thrown when attempting to add a scheduled delegate that has been already completed.</exception>
        public void Add(ScheduledDelegate task)
        {
            if (task.Completed)
                throw new InvalidOperationException($"Can not add a {nameof(ScheduledDelegate)} that has been already {nameof(ScheduledDelegate.Completed)}");

            lock (queueLock)
                timedTasks.AddInPlace(task);
        }

        /// <summary>
        /// Add a task which will be run after a specified delay from the current clock time.
        /// </summary>
        /// <param name="task">The work to be done.</param>
        /// <param name="timeUntilRun">Milliseconds until run.</param>
        /// <param name="repeat">Whether this task should repeat.</param>
        /// <returns>The scheduled task.</returns>
        public ScheduledDelegate AddDelayed(Action task, double timeUntilRun, bool repeat = false)
        {
            // We are locking here already to make sure we have no concurrent access to currentTime
            lock (queueLock)
            {
                ScheduledDelegate del = new ScheduledDelegate(task, currentTime + timeUntilRun, repeat ? timeUntilRun : -1);
                Add(del);
                return del;
            }
        }

        /// <summary>
        /// Adds a task which will only be run once per frame, no matter how many times it was scheduled in the previous frame.
        /// </summary>
        /// <remarks>The task will be run on the next <see cref="Update"/> independent of the current clock time.</remarks>
        /// <param name="task">The work to be done.</param>
        /// <returns>Whether this is the first queue attempt of this work.</returns>
        public bool AddOnce(Action task)
        {
            lock (queueLock)
            {
                if (runQueue.Any(sd => sd.Task == task))
                    return false;

                runQueue.Enqueue(new ScheduledDelegate(task));
            }

            return true;
        }
    }
}
