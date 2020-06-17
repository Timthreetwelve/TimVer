using System;

namespace TimVer.ViewModels
{
    class Page1ViewModel
    {
        public string Build
        {
            get
            {
                string curBuild = GetInfo.GetRegistryInfo("CurrentBuild");
                string ubr = GetInfo.GetRegistryInfo("UBR");
                return string.Format($"{curBuild}.{ubr}");
            }
            set => Build = value;
        }

        public string ProdName => GetInfo.GetRegistryInfo("ProductName");

        public string Version => GetInfo.GetRegistryInfo("ReleaseID");

        public string BuildBranch => GetInfo.GetRegistryInfo("BuildBranch");

        public string Arch => GetInfo.CimQueryOS("OSArchitecture");

        public string InstallDate => GetInfo.CimQueryOS("InstallDate");

        
    }
}
