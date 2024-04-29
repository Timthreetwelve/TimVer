// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class to get BIOS information.
/// </summary>
/// <seealso cref="UefiHelpers"/>
public static class BiosHelpers
{
    private const string _scope = @"\\.\root\CIMV2";
    private const string _dialect = "WQL";

    #region BIOS Manufacturer
    /// <summary>
    /// Gets BIOS manufacturer
    /// </summary>
    /// <returns>BIOS manufacturer as a string</returns>
    public static string GetBiosManufacturer()
    {
        try
        {
            const string query = "SELECT Manufacturer From Win32_BIOS";
            using CimSession cim = CimSession.Create(null);
            return cim.QueryInstances(_scope, _dialect, query)
                .FirstOrDefault()!.CimInstanceProperties["Manufacturer"].Value.ToString()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "GetBiosManufacturer call failed.");
            return ex.Message;
        }
    }
    #endregion BIOS Manufacturer

    #region Get BIOS version
    /// <summary>
    /// Gets BIOS name which should be the version
    /// </summary>
    /// <returns>BIOS version as a string</returns>
    public static string GetBiosVersion()
    {
        try
        {
            const string query = "SELECT Name From Win32_BIOS";
            using CimSession cim = CimSession.Create(null);
            return cim.QueryInstances(_scope, _dialect, query)
                .FirstOrDefault()!.CimInstanceProperties["Name"].Value.ToString()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "GetBiosVersion call failed.");
            return ex.Message;
        }
    }
    #endregion Get BIOS version

    #region Get BIOS release date
    /// <summary>
    /// Gets BIOS release date
    /// </summary>
    /// <returns>Release date as string</returns>
    public static DateTime GetBiosDate()
    {
        try
        {
            const string query = "SELECT ReleaseDate From Win32_BIOS";
            using CimSession cim = CimSession.Create(null);
            return (DateTime)cim.QueryInstances(_scope, _dialect, query)
                .FirstOrDefault()!.CimInstanceProperties["ReleaseDate"].Value;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "GetBiosDate call failed.");
            return default;
        }
    }
    #endregion  Get BIOS release date
}
