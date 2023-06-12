// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

/// <summary>
/// Gets information from the registry, CIM or the Environment
/// </summary>
public static class GetInfo
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

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
            string regVal = key.GetValue(value) != null ? key.GetValue(value).ToString() : "no data";
            _log.Debug($"Registry: {value} = {regVal}");
            return regVal;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Registry call failed.");
            return ex.Message;
        }
    }
    #endregion Get registry information

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
            string cimVal = cim.QueryInstances("root/CIMV2", "WQL", $"SELECT {value} From Win32_OperatingSystem")
                .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString();
            _log.Debug($"Win32_OperatingSystem: {value} = {cimVal}");
            return cimVal;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_OperatingSystem call failed.");
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
            string cimVal = cim.QueryInstances("root/CIMV2", "WQL", $"SELECT {value} From Win32_ComputerSystem")
                .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString();
            _log.Debug($"Win32_ComputerSystem: {value} = {cimVal}");
            return cimVal;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_ComputerSystem call failed.");
            return ex.Message;
        }
    }
    #endregion Get System Information

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
            string cimVal = cim.QueryInstances("root/CIMV2", "WQL", $"SELECT {value} From Win32_Processor")
                .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString();
            _log.Debug($"Win32_Processor: {value} = {cimVal}");
            return cimVal;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_Processor call failed.");
            return ex.Message;
        }
    }
    #endregion Get Processor Information

    #region Get special folder
    /// <summary>
    /// Get special folder
    /// </summary>
    /// <param name="value">Special folder name</param>
    /// <returns>Path to folder</returns>
    public static string GetSpecialFolder(Environment.SpecialFolder value)
    {
        string cimVal = Environment.GetFolderPath(value);
        _log.Debug($"Environment: {value} folder = {cimVal}");
        return cimVal;
    }
    #endregion Get special folder

    #region Get environment variables
    /// <summary>
    /// Gets the environment variables and sorts by variable name
    /// </summary>
    public static List<EnvVariable> GetEnvironmentVariables()
    {
        try
        {
            Stopwatch watch = Stopwatch.StartNew();
            List<EnvVariable> envList = new();
            IDictionary env = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry entry in env)
            {
                EnvVariable envVariable = new()
                {
                    Variable = entry.Key.ToString(),
                    Value = entry.Value.ToString()
                };
                envList.Add(envVariable);
            }
            watch.Stop();
            _log.Debug($"Found {env.Count} environment variables in {watch.Elapsed.TotalMilliseconds:N2} ms");
            return envList.OrderBy(envVariable => envVariable.Variable).ToList();
        }
        catch (Exception ex)
        {
            _log.Debug(ex, "Error occurred in GetEnvironmentVariables.");
            List<EnvVariable> errorList = new();
            EnvVariable envVariable = new()
            {
                Variable = "An Error has occurred.",
                Value = "See the log for more information."
            };
            errorList.Add(envVariable);
            return errorList;
        }
    }
    #endregion Get environment variables
}
