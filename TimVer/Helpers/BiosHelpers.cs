// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

public static class BiosHelpers
{
    #region BIOS Manufacturer
    /// <summary>
    /// Gets BIOS manufacturer
    /// </summary>
    /// <returns>BIOS manufacturer as a string</returns>
    public static string GetBiosManufacturer()
    {
        try
        {
            CimSession cim = CimSession.Create(null);
            return cim.QueryInstances(
                "root/CIMV2",
                "WQL",
                "SELECT Manufacturer From Win32_BIOS")
                .FirstOrDefault().CimInstanceProperties["Manufacturer"].Value.ToString();
        }
        catch (Exception ex)
        {
            _log.Error(ex, "GetBiosManufacturer call failed.");
            return null;
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
            CimSession cim = CimSession.Create(null);
            return cim.QueryInstances(
                "root/CIMV2",
                "WQL",
                "SELECT Name From Win32_BIOS")
                .FirstOrDefault().CimInstanceProperties["Name"].Value.ToString();
        }
        catch (Exception ex)
        {
            _log.Error(ex, "GetBiosVersion call failed.");
            return null;
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
            CimSession cim = CimSession.Create(null);
            return (DateTime)cim.QueryInstances(
                "root/CIMV2",
                "WQL",
                "SELECT ReleaseDate From Win32_BIOS")
                .FirstOrDefault().CimInstanceProperties["ReleaseDate"].Value;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "GetBiosDate call failed.");
            return default;
        }
    }
    #endregion  Get BIOS release date
}
