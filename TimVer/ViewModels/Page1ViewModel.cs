// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels
{
    internal class Page1ViewModel
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

        public string Version
        {
            get
            {
                string result = GetInfo.GetRegistryInfo("DisplayVersion");
                return result == "no data" ? GetInfo.GetRegistryInfo("ReleaseID") : result;
            }
        }

        public string BuildBranch => GetInfo.GetRegistryInfo("BuildBranch");

        public string Arch => GetInfo.CimQueryOS("OSArchitecture");

        public string InstallDate => GetInfo.CimQueryOS("InstallDate");
    }
}
