// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class to get Processor (CPU) information.
/// </summary>
internal static class ProcessorHelpers
{
    private const string _scope = @"\\.\root\CIMV2";
    private const string _dialect = "WQL";

    #region Get Processor Information
    /// <summary>
    /// Get CIM value from Win32_Processor
    /// </summary>
    /// <param name="value">Value to retrieve</param>
    /// <returns>String for value or exception message</returns>
    public static string CimQueryProc(string value)
    {
        try
        {
            CimSession cim = CimSession.Create(null);
            return cim.QueryInstances(_scope, _dialect, $"SELECT {value} From Win32_Processor")
                .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_Processor call failed.");
            return ex.Message;
        }
    }
    #endregion Get Processor Information
}
