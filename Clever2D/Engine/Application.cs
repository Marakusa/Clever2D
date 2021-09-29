namespace Clever2D.Engine
{
    public static class Application
    {
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

        private static ApplicationConfig config = new();

        public static void Exit()
        {
            // TODO: Exit the application
        }
    }

    public class ApplicationConfig
    {
        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value;
            }
        }
        public string AuthorName
        {
            get
            {
                return authorName;
            }
            set
            {
                authorName = value;
            }
        }
        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        private string projectName = "ExampleProject";
        private string authorName = "ExampleCompany";
        private string version = "0.1.0";
    }
}
