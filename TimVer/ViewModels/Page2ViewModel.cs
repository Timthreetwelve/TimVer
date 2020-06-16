using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Management;

namespace TimVer.ViewModels
{
    class Page2ViewModel
    {
        public string MachName => Environment.MachineName;

        public string BuildBranch => GetRegistryInfo("BuildBranch");

        public string RegOwner => GetRegistryInfo("RegisteredOwner");

        public string EditionID => GetRegistryInfo("EditionID");

        public string SystemDevice => GetWmiInfo("SystemDevice");

        public string BootDevice => GetWmiInfo("BootDevice");

        #region Get registry information
        public string GetRegistryInfo(string value)
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
