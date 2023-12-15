// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

public static class MemoryHelpers
{
    /// <summary>
    /// Gets the total amount of installed ram.
    /// </summary>
    /// <returns>Total GB as a formatted string</returns>
    public static string GetInstalledRam()
    {
        try
        {
        CimSession cim = CimSession.Create(null);
        IEnumerable<CimInstance> cimVal = cim.QueryInstances(
            "root/CIMV2",
            "WQL",
            "SELECT * From Win32_PhysicalMemory");

        ulong mem = 0;
        foreach (CimInstance val in cimVal)
        {
            mem += Convert.ToUInt64(val.CimInstanceProperties["Capacity"].Value);
        }

        return $"{(double)(mem / Math.Pow(1024, 3)):N2} GB";
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_PhysicalMemory call failed.");
            return null;
        }
    }
}
