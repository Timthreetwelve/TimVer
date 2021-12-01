// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region Using directives
using Microsoft.Management.Infrastructure;
using Microsoft.Win32;
using System;
using System.Linq;
#endregion Using directives

namespace TimVer
{
    public static class GetInfo
    {
        #region Get registry information
        /// <summary>
        /// Gets a value from HKLM\Software\Microsoft\Windows NT\CurrentVersion
        /// </summary>
        /// <param name="value">Value to retrieve </param>
        /// <returns>The value if it exists, "no data" otherwise</returns>
        public static string GetRegistryInfo(string value)
        {
            try
            {
                using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion");
                return key.GetValue(value) != null ? key.GetValue(value).ToString() : "no data";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Get OS information
        /// <summary>
        /// Get CIM value from Win32_OperatingSystem
        /// </summary>
        /// <param name="value">Value to retrieve</param>
        /// <returns>Data for value or exception message</returns>
        public static string CimQueryOS(string value)
        {
            try
            {
                CimSession cim = CimSession.Create(null);
                return cim.QueryInstances("root/cimv2", "WQL", $"SELECT {value} From Win32_OperatingSystem")
                    .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion Get OS information

        #region Get System Information
        /// <summary>
        /// Get CIM value from Win32_ComputerSystem
        /// </summary>
        /// <param name="value">Value to retrieve</param>
        /// <returns>Data for value or exception message</returns>
        public static string CimQuerySys(string value)
        {
            try
            {
                CimSession cim = CimSession.Create(null);
                return cim.QueryInstances("root/cimv2", "WQL", $"SELECT {value} From Win32_ComputerSystem")
                    .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Get Processor Information
        /// <summary>
        /// Get CIM value from Win32_Processor
        /// </summary>
        /// <param name="value">Value to retrieve</param>
        /// <returns>Data for value or exception message</returns>
        public static string CimQueryProc(string value)
        {
            try
            {
                CimSession cim = CimSession.Create(null);
                return cim.QueryInstances("root/cimv2", "WQL", $"SELECT {value} From Win32_Processor")
                    .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Get environment variable
        /// <summary>
        /// Get CIM value from Win32_Processor
        /// </summary>
        /// <param name="value">Value to retrieve</param>
        /// <returns>Data for value</returns>
        public static string GetEnvironment(string value)
        {
            return Environment.GetEnvironmentVariable(value);
        }
        #endregion Get environment variable
    }
}
