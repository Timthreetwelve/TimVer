using System;

namespace TimVer.ViewModels
{
    internal class PrevInfoViewModel
    {
        private readonly MySettings settings = MySettings.Read();

        public string PreviousBuild
        {
            get
            {
                if (!string.IsNullOrEmpty(settings.PrevBuild))
                {
                    return settings.PrevBuild;
                }
                return "no data";
            }
        }

        public string PreviousBranch
        {
            get
            {
                if (!string.IsNullOrEmpty(settings.PrevBranch))
                {
                    return settings.PrevBranch;
                }
                return "no data";
            }
        }

        public string PreviousVersion
        {
            get
            {
                if (!string.IsNullOrEmpty(settings.PrevVersion))
                {
                    return settings.PrevVersion;
                }
                return "no data";
            }
        }

        public string LastRun
        {
            get
            {
                DateTime lr = settings.LastRun;
                return lr.ToString("g");
            }
        }
    }
}
