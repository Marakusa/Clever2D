using System;
using System.IO;
using System.Text;

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
        private static string logFile = "";
        /// <summary>
        /// Path to the currently active log files directory.
        /// </summary>
        private static string logDirectory = "";

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
                if (logFile == "")
                {
                    logDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/" + Application.CompanyName + "/" + Application.ProductName + "/";
                    logFile = logDirectory + "PlayerLog.txt";

                    // Backup the of log file and remove old backup if exists
                    if (File.Exists(logFile))
                    {
                        if (File.Exists(logFile + ".old")) File.Delete(logFile + ".old");
                        File.Move(logFile, logFile + ".old");
                    }

                    // Log startup lines
                    string[] logStart = new string[]
                    {
                    $"{Application.ProductName} {Application.ProductVersion} (c) {Application.CompanyName} {DateTime.Now.Year} | Clever2D {Version.CurrentVersion} {Version.Copyright}",
                    $"Log file location: {logFile}"
                    };

                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    File.WriteAllLines(logFile, logStart, Encoding.UTF8);

                    foreach (string logMessage in logStart)
                    {
                        Console.WriteLine(logMessage);
                    }
                }

                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                File.AppendAllText(logFile, "\n" + message, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Player.LogError(e.Message, e);
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
    }
}
