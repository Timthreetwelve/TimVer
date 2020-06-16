using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Management;

namespace TimVer.ViewModels
{
    class Page1ViewModel
    {
        public string MachName = Environment.MachineName;

        public string Build
        {
            get
            {
                string curBuild = GetRegistryInfo("CurrentBuild");
                string ubr = GetRegistryInfo("UBR");
                return string.Format($"{curBuild}.{ubr}");
            }
            set => Build = value;
        }

        public string ProdName => GetRegistryInfo("ProductName");

        public string Version => GetRegistryInfo("ReleaseID");

        public string Arch => GetWmiInfo("OSArchitecture");

        public string InstallDate
        {
            get
            {
                var date = GetWmiInfo("InstallDate");
                return ManagementDateTimeConverter.ToDateTime(date).ToString();
            }
        }

        public string LastBoot
        {
            get
            {
                string date = GetWmiInfo("LastBootUpTime");
                return ManagementDateTimeConverter.ToDateTime(date).ToString();
            }
        }

        #region Get registry information
        private string GetRegistryInfo(string value)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion"))
                {
                    if (key != null)
                    {
                        Debug.WriteLine($"Returning {value} from GetRestistryInfo");
                        return key.GetValue(value).ToString();
                    }
                    return "no data";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Get WMI information
        private string GetWmiInfo(string value)
        {
            try
            {
                ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectCollection osDetailsCollection = objOSDetails.Get();

                foreach (ManagementObject manObj in osDetailsCollection)
                {
                    Debug.WriteLine($"Returning {value} from GetWmiInfo");
                    return manObj.GetPropertyValue(value).ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
