using Clever2D.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;

namespace Clever2D.Engine
{
    /// <summary>
    /// The base class for the Clever Player which controls the logging.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Path to the currently active log file.
        /// </summary>
        private static string _logFile = "";
        /// <summary>
        /// Path to the currently active log files directory.
        /// </summary>
        private static string _logDirectory = "";

        private static readonly List<string> LogLines = new();

        /// <summary>
        /// Logs a message into the log file and Console.
        /// </summary>
        /// <param name="value">String or object to be displayed in the logs.</param>
        public static void Log(object value)
        {
            string message = $"{GetTimestamp()} | [I] {value}";
            WriteLog(message, ConsoleColor.Cyan);
        }
        /// <summary>
        /// Logs a warning into the log file and Console.
        /// </summary>
        /// <param name="value">String or object to be displayed in the logs.</param>
        public static void LogWarn(object value)
        {
            string message = $"{GetTimestamp()} | [W] {value}";
            WriteLog(message, ConsoleColor.Yellow);
        }
        /// <summary>
        /// Logs an error into the log file and Console.
        /// </summary>
        /// <param name="value">String or object to be displayed in the logs.</param>
        public static void LogError(object value)
        {
            string message = $"{GetTimestamp()} | [E] {value}";
            WriteLog(message, ConsoleColor.Red);
        }
        /// <summary>
        /// Logs an error into the log file and Console.
        /// </summary>
        /// <param name="value">String or object to be displayed in the logs.</param>
        /// <param name="e">Exception to be displayed in the logs.</param>
        public static void LogError(object value, Exception e)
        {
            if (e == null) e = new Exception();

            string message = $"{GetTimestamp()} | [E] {e} {value}";
            WriteLog(message, ConsoleColor.Red);
        }

        /// <summary>
        /// Base method to save the log message into the log file and to display it in the Console.
        /// </summary>
        /// <param name="message">String to be displayed in the logs.</param>
        /// <param name="color">Color of the log message in Console.</param>
        private static void WriteLog(string message, ConsoleColor color)
        {
            try
            {
                if (_logFile == "")
                {
                    _logDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/" + Application.CompanyName + "/" + Application.ProductName + "/";
                    _logFile = _logDirectory + "PlayerLog.txt";

                    // Backup the of log file and remove old backup if exists
                    if (File.Exists(_logFile))
                    {
                        if (File.Exists(_logFile + ".old")) File.Delete(_logFile + ".old");
                        File.Move(_logFile, _logFile + ".old");
                    }

                    // Log startup lines
                    string[] logStart = new[]
                    {
                    $"{Application.ProductName} {Application.ProductVersion} (c) {Application.CompanyName} {DateTime.Now.Year.ToString()} | Clever2D {Version.CurrentVersion} {Version.Copyright}",
                    $"Log file location: {_logFile}"
                    };

                    if (!Directory.Exists(_logDirectory))
                    {
                        Directory.CreateDirectory(_logDirectory);
                    }

                    File.WriteAllLines(_logFile, logStart, Encoding.UTF8);

                    foreach (string logMessage in logStart)
                    {
                        LogLines.Add(logMessage);
                        Console.WriteLine(logMessage);
                    }

                    Clever.Destroyed += SaveLogs;

                    Timer saveTimer = new()
                    {
                        Interval = 3f
                    };
                    saveTimer.Elapsed += delegate
                    {
                        SaveLogs();
                    };
                    saveTimer.Start();
                }

                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                LogLines.Add(message);
            }
            catch (Exception e)
            {
                LogError(e.Message, e);
            }
        }
        /// <summary>
        /// Returns the timestamp of the frame this gets called.
        /// </summary>
        private static string GetTimestamp()
        {
            DateTime dateTime = DateTime.Now;
            return $"{dateTime:MMM d} {dateTime.ToLongTimeString()}";
        }

        private static void SaveLogs()
        {
            try
            {
                File.WriteAllLines(_logFile, LogLines, Encoding.UTF8);
            }
            catch
            {
                // ignored
            }
        }
    }
}
