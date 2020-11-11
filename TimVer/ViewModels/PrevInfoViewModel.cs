using System;

namespace TimVer.ViewModels
{
    internal class PrevInfoViewModel
    {
        public string PreviousBuild
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.PrevBuild))
                {
                    return Properties.Settings.Default.PrevBuild;
                }
                return "no data";
            }
        }

        public string PreviousBranch
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.PrevBranch))
                {
                    return Properties.Settings.Default.PrevBranch;
                }
                return "no data";
            }
        }

        public string PreviousVersion
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.PrevVersion))
                {
                    return Properties.Settings.Default.PrevVersion;
                }
                return "no data";
            }
        }

        public string LastRun
        {
            get
            {
                DateTime lr = Properties.Settings.Default.LastRun;
                return lr.ToString("g");
            }
        }
    }
}
