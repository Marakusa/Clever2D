using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever2D.Engine
{
    public class Player
    {
        private static string logFile = "";
        private static string logDirectory = "";

        public static void Log(object value)
        {
            string message = $"{GetTimestamp()} | [I] {value}";
            WriteLog(message, ConsoleColor.Cyan);
        }
        public static void LogWarn(object value)
        {
            string message = $"{GetTimestamp()} | [W] {value}";
            WriteLog(message, ConsoleColor.Yellow);
        }
        public static void LogError(object value)
        {
            string message = $"{GetTimestamp()} | [E] {value}";
            WriteLog(message, ConsoleColor.Red);
        }
        public static void LogError(object value, Exception e)
        {
            if (e == null) e = new Exception();

            string message = $"{GetTimestamp()} | [E] {e} {value}";
            WriteLog(message, ConsoleColor.Red);
        }

        private static void WriteLog(string message, ConsoleColor color)
        {
            if (logFile == "")
            {
                logDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/" + Application.Config.AuthorName + "/" + Application.Config.ProjectName + "/";
                logFile = logDirectory + "/PlayerLog.txt";
                
                // Backup the of log file and remove old backup if exists
                if (File.Exists(logFile))
                {
                    if (File.Exists(logFile + ".old")) File.Delete(logFile + ".old");
                    File.Move(logFile, logFile + ".old");
                }

                // Log startup lines
                string[] logStart = new string[]
                {
                    $"{Application.Config.ProjectName} v{Application.Config.Version} (c) {Application.Config.AuthorName} {DateTime.Now.Year} | Clever2D v{Version.CurrentVersion} {Version.Copyright}"
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
        private static string GetTimestamp()
        {
            DateTime dateTime = DateTime.Now;
            return $"{dateTime.ToString("MMM d")} {dateTime.ToLongTimeString()}";
        }
    }
}
