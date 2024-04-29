// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class ComputerSystemHelpers
{
    private const string _scope = @"\\.\root\CIMV2";
    private const string _dialect = "WQL";

    #region Get System information
    /// <summary>
    /// Get CIM value from Win32_ComputerSystem
    /// </summary>
    /// <param name="value">Value to retrieve</param>
    /// <returns>String for value or exception message</returns>
    public static string CimQuerySys(string value)
    {
        try
        {
            CimSession cim = CimSession.Create(null);
            return cim.QueryInstances(_scope, _dialect, $"SELECT {value} From Win32_ComputerSystem")
                .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_ComputerSystem call failed.");
            return ex.Message;
        }
    }
    #endregion Get System information
}
