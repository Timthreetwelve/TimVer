// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

public static class VideoHelpers
{
    #region Get WMI info for video controller(s)
    /// <summary>
    /// Get CIM value from Win32_VideoController
    /// </summary>
    /// <returns>IEnumerable or null if there was an exception</returns>
    private static IEnumerable<CimInstance> CimQueryGPU()
    {
        try
        {
            Stopwatch sw = Stopwatch.StartNew();
            CimSession cim = CimSession.Create(null);
            IEnumerable<CimInstance> cimInst = cim.QueryInstances(
                "root/CIMV2",
                "WQL",
                "SELECT * From Win32_VideoController");
            sw.Stop();
            if (cimInst.Count() == 1)
            {
                _log.Debug($"Found {cimInst.Count()} video adapter in {sw.Elapsed.TotalMilliseconds:N2} ms");
            }
            else
            {
                _log.Debug($"Found {cimInst.Count()} video adapters in {sw.Elapsed.TotalMilliseconds:N2} ms");
            }
            return cimInst;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_VideoController call failed.");
            return null;
        }
    }
    #endregion Get WMI info for video controller(s)

    #region Build list of video info
    /// <summary>
    /// Aggregates video information
    /// </summary>
    /// <returns>A List of GpuInfo</returns>
    public static List<GpuInfo> GetGpuInfo()
    {
        List<GpuInfo> gpuInfoList = new();
        foreach (CimInstance gpu in CimQueryGPU())
        {
            GpuInfo info = new()
            {
                GpuDeviceID = GetCimStringInfo(gpu, "DeviceID"),
                GpuName = GetCimStringInfo(gpu, "Name"),
                GpuDescription = GetCimStringInfo(gpu, "Description"),
                GpuHorizontalResolution = GetCimStringInfo(gpu, "CurrentHorizontalResolution"),
                GpuVerticalResolution = GetCimStringInfo(gpu, "CurrentVerticalResolution"),
                GpuCurrentRefresh = GetCimStringInfo(gpu, "CurrentRefreshRate"),
                GpuMinRefresh = GetCimStringInfo(gpu, "MinRefreshRate"),
                GpuMaxRefresh = GetCimStringInfo(gpu, "MaxRefreshRate"),
                GpuBitsPerPixel = GetCimStringInfo(gpu, "CurrentBitsPerPixel"),
                GpuVideoProcessor = GetCimStringInfo(gpu, "VideoProcessor"),
                GpuVideoDescription = GetCimStringInfo(gpu, "VideoModeDescription"),
                GpuAdapterRam = GetAdapterRamInfo(gpu),
                GpuNumberOfColors = GetColorsInfo(gpu)
            };
            gpuInfoList.Add(info);
        }
        return gpuInfoList;
    }
    #endregion Build list of video info

    #region Get WMI infomation formatted as string
    /// <summary>
    /// Gets WMI video info as string
    /// </summary>
    /// <param name="ci">The CimInstance</param>
    /// <param name="itemName">CIM Property</param>
    /// <returns>A string</returns>
    private static string GetCimStringInfo(CimInstance ci, string itemName)
    {
        if (ci.CimInstanceProperties[itemName] == null)
        {
            _log.Debug($"Value for {itemName} was null");
            return "Not Available";
        }
        return ci.CimInstanceProperties[itemName].Value.ToString();
    }
    #endregion Get WMI infomation formatted as string

    #region Adapter Memory
    /// <summary>
    /// Gets video adapter Ram
    /// </summary>
    /// <param name="ci">The CimInstance</param>
    /// <returns>A formatted string</returns>
    private static string GetAdapterRamInfo(CimInstance ci)
    {
        if (ci.CimInstanceProperties["AdapterRam"] == null)
        {
            _log.Debug("Value for AdapterRam was null");
            return "Not Available";
        }
        double ram = Convert.ToDouble(ci.CimInstanceProperties["AdapterRam"].Value);
        if (ram >= Math.Pow(1024, 3))
        {
            ram /= (double)Math.Pow(1024, 3);
            return string.Format(CultureInfo.CurrentCulture, $"{ram:N2} GB");
        }
        ram /= (double)Math.Pow(1024, 2);
        return string.Format(CultureInfo.CurrentCulture, $"{ram:N2} MB");
    }
    #endregion Adapter Memory

    #region Total Colors
    /// <summary>
    /// Gets to number of colors
    /// </summary>
    /// <param name="ci">The CimInstance</param>
    /// <returns>A formatted string</returns>
    private static string GetColorsInfo(CimInstance ci)
    {
        if (ci.CimInstanceProperties["CurrentNumberOfColors"] == null)
        {
            _log.Debug("Value for CurrentNumberOfColors was null");
            return GetStringResource("MsgText_NotAvailable");
        }

        ulong colors = Convert.ToUInt64(ci.CimInstanceProperties["CurrentNumberOfColors"].Value);
        if (colors >= (ulong)Math.Pow(1024, 2))
        {
            colors /= (ulong)Math.Pow(1024, 3);
            return string.Format(CultureInfo.CurrentCulture, $"{colors:N2} {GetStringResource("MsgText_Million")}");
        }
        else if (colors >= (ulong)Math.Pow(1024, 1))
        {
            colors /= (ulong)Math.Pow(1024, 2);
            return string.Format(CultureInfo.CurrentCulture, $"{colors:N2} {GetStringResource("MsgText_Thousand")}");
        }
        else
        {
            return string.Format(CultureInfo.CurrentCulture, $"{colors:N0}");
        }
    }
    #endregion Total Colors
}
