using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TimVer
{
    // Class to clean up previous versions of user settings
    // CleanPrevSettings() should not be called until after Properties.Settings.Default.Upgrade()

    public static class CleanUp
    {
        public static string GetVersion()
        {
            // Determine current app version
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void CleanupPrevSettings()
        {
            // Determine what folder the user.config file is in
            string config = ConfigurationManager.OpenExeConfiguration
                (ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

            // Determine the parent directory
            DirectoryInfo parentPath = Directory.GetParent(Directory.GetParent(config).FullName);

            //  Get a list of all the directories in the parent directory
            DirectoryInfo[] dirs = parentPath.GetDirectories();
            int count = dirs.Length;

            // If there is more than one directory, then the others are from a previous version
            // This assumes that Properties.Settings.Default.Upgrade() has completed.
            if (count > 1)
            {
                // Directory name must match 'num.num.num.num' with no alpha characters
                Regex regex = new Regex(@"(\A\d+.\d+.\d+.\d+\z)");
                foreach (DirectoryInfo dir in dirs)
                {
                    // Delete all the directories that aren't for the current version and
                    // match the regex pattern
                    Match match = regex.Match(dir.Name);
                    if (dir.Name != GetVersion() && match.Success)
                    {
                        dir.Delete(true);
                        Debug.WriteLine($"+++ Delete {dir.FullName}");
                    }
                }
            }
        }
    }
}
