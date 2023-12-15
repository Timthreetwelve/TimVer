// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

public static class MemoryHelpers
{
    /// <summary>
    /// Gets the total amount of installed ram.
    /// </summary>
    /// <returns>Total GB as a formatted string in the current culture.</returns>
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

            CultureInfo culture = CultureInfo.CurrentUICulture;
            return string.Format(culture, "{0:N2} GB", mem / Math.Pow(1024, 3));
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_PhysicalMemory call failed.");
            return null;
        }
    }

    /// <summary>
    /// Gets the amount of usable memory.
    /// </summary>
    /// <returns>Usable GB as a formatted string in the current culture.</returns>
    public static string GetUsableRam()
    {
        try
        {
            var gcMemoryInfo = GC.GetGCMemoryInfo();
            long installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes;
            double GB = Math.Round(Convert.ToDouble(installedMemory) / Math.Pow(1024, 3), 2);
            CultureInfo culture = CultureInfo.CurrentUICulture;
            return string.Format(culture, "{0:N2} GB", GB);
        }
        catch (Exception ex)
        {
            _log.Error(ex, " GC.GetGCMemoryInfo() call failed.");
            return null;
        }
    }
}
