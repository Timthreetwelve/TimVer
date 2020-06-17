using Microsoft.Management.Infrastructure;
using Microsoft.Win32;
using System;
using System.Linq;

namespace TimVer
{
    public static class GetInfo
    {
        #region Get registry information
        public static string GetRegistryInfo(string value)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion"))
                {
                    if (key != null)
                    {
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

        #region Get OS information
        public static string CimQueryOS(string value)
        {
            try
            {
                CimSession cim = CimSession.Create(null);
                return cim.QueryInstances(@"root/cimv2", "WQL", "SELECT * From Win32_OperatingSystem")
                    .FirstOrDefault().CimInstanceProperties[value].Value.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Get System Information
        public static string CimQuerySys(string value)
        {
            try
            {
                CimSession cim = CimSession.Create(null);
                return cim.QueryInstances(@"root/cimv2", "WQL", "SELECT * From Win32_ComputerSystem")
                    .FirstOrDefault().CimInstanceProperties[value].Value.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
