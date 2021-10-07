using Clever2D.Core;

namespace Clever2D.Engine
{
    /// <summary>
    /// Access to application data.
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// Executable directory (path to .exe without the executable file).
        /// </summary>
        public static string ExecutableDirectory
        {
            get;
        } = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Get the Applications configuration.
        /// </summary>
        public static ApplicationConfig Config
        {
            get;
            set;
        } = new();

        /// <summary>
        /// Returns application product name (Read Only).
        /// </summary>
        public static string ProductName => Config.ProductName;
        /// <summary>
        /// Returns application company name (Read Only).
        /// </summary>
        public static string CompanyName => Config.CompanyName;
        /// <summary>
        /// Returns application product version (Read Only).
        /// </summary>
        public static string ProductVersion => Config.Version;

        /// <summary>
        /// Quits the application.
        /// </summary>
        public static void Exit()
        {
            Player.Log("Exiting...");
            Clever.Quit = true;
        }
    }

    /// <summary>
    /// Configure an application.
    /// </summary>
    public class ApplicationConfig
    {
        /// <summary>
        /// Returns application product name.
        /// </summary>
        public string ProductName
        {
            get => projectName;
            init { if (projectName == "") projectName = value; }
        }
        /// <summary>
        /// Returns application company name.
        /// </summary>
        public string CompanyName
        {
            get => authorName;
            init { if (authorName == "") authorName = value; }
        }
        /// <summary>
        /// Returns application product version.
        /// </summary>
        public string Version
        {
            get => version;
            init { if (version == "") version = value; }
        }

        private readonly string projectName = "";
        private readonly string authorName = "";
        private readonly string version = "";
    }
}
