using System.Reflection;
using System.Diagnostics;

namespace Clever2D
{
    class Version
    {
        public static string CurrentVersion
        {
            get
            {
                ReleaseState releaseState = ReleaseState.Alpha;
                string release = "";

                switch (releaseState)
                {
                    case ReleaseState.Alpha:
                        release = "a";
                        break;
                    case ReleaseState.Beta:
                        release = "b";
                        break;
                    case ReleaseState.Release:
                        release = "r";
                        break;
                }

                string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
                return "v" + version + release;
            }
        }
        public static string Copyright
        {
            get
            {
                string copyright = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright;
                return copyright;
            }
        }

        enum ReleaseState
        {
            Alpha, Beta, Release
        }
    }
}
