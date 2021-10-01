using System.Reflection;
using System.Diagnostics;

namespace Clever2D
{
    /// <summary>
    /// The class that handles the Version of Clever.
    /// </summary>
    internal class Version
    {
        /// <summary>
        /// Returns the current version of Clever.
        /// </summary>
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
        /// <summary>
        /// Returns the copyright string of Celver.
        /// </summary>
        public static string Copyright
        {
            get
            {
                string copyright = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright;
                return copyright;
            }
        }

        /// <summary>
        /// The release state of a Version (Alpha, Beta, Release).
        /// </summary>
        enum ReleaseState
        {
            Alpha, Beta, Release
        }
    }
}
