namespace Clever2D.Engine
{
    /// <summary>
    /// Access to application data.
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// Applications configuration.
        /// </summary>
        private static ApplicationConfig config = new();
        /// <summary>
        /// Get the Applications configuration.
        /// </summary>
        public static ApplicationConfig Config
        {
            get
            {
                return config;
            }
            set
            {
                config = value;
            }
        }

        /// <summary>
        /// Returns application product name (Read Only).
        /// </summary>
        public static string ProductName
        {
            get
            {
                return Config.ProductName;
            }
        }
        /// <summary>
        /// Returns application company name (Read Only).
        /// </summary>
        public static string CompanyName
        {
            get
            {
                return Config.CompanyName;
            }
        }
        /// <summary>
        /// Returns application product version (Read Only).
        /// </summary>
        public static string ProductVersion
        {
            get
            {
                return Config.Version;
            }
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        public static void Exit()
        {
            // TODO: Exit the application
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
            get
            {
                return projectName;
            }
            set
            {
                if (projectName == "")
                {
                    projectName = value;
                }
            }
        }
        /// <summary>
        /// Returns application company name.
        /// </summary>
        public string CompanyName
        {
            get
            {
                return authorName;
            }
            set
            {
                if (authorName == "")
                {
                    authorName = value;
                }
            }
        }
        /// <summary>
        /// Returns application product version.
        /// </summary>
        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                if (version == "")
                {
                    version = value;
                }
            }
        }

        private string projectName = "";
        private string authorName = "";
        private string version = "";
    }
}
