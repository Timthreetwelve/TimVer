// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class OperatingSystemHelpers
{
    #region Constants
    private const string _scope = @"\\.\root\CIMV2";
    private const string _dialect = "WQL";
    #endregion Constants

    #region Get string value from Win32_OperatingSystem
    /// <summary>
    /// Get CIM value from Win32_OperatingSystem
    /// </summary>
    /// <param name="value">Value to retrieve</param>
    /// <returns>String for value or exception message</returns>
    public static string GetWin32OperatingSystem(string value)
    {
        try
        {
            CimSession? cim = CimSession.Create(null);
            return cim.QueryInstances(_scope, _dialect, $"SELECT {value} From Win32_OperatingSystem")
                .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_OperatingSystem call failed.");
            return ex.Message;
        }
    }
    #endregion Get string value from Win32_OperatingSystem

    #region Get OS Architecture
    /// <summary>
    /// Gets the operating system architecture.
    /// </summary>
    /// <returns>Architecture as a string.</returns>
    public static string OsArchitecture()
    {
        try
        {
            Architecture? architecture = RuntimeInformation.OSArchitecture;
            return architecture.ToString()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "RuntimeInformation.OSArchitecture call failed.");
            return ex.Message;
        }
    }
    #endregion Get OS Architecture
}
