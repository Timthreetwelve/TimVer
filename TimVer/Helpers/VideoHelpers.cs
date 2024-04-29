// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class to get Video information.
/// </summary>
public static class VideoHelpers
{
    #region Build list of video info
    /// <summary>
    /// Aggregates video information
    /// </summary>
    /// <returns>A List of GpuInfo</returns>
    public static List<GpuInfo> GetGpuInfo()
    {
        Stopwatch sw = Stopwatch.StartNew();
        List<GpuInfo> gpuInfoList = [];
        foreach (string? gpu in GetGPUList())
        {
            Dictionary<string, string> results = GetWin32VideoController(gpu!);
            GpuInfo gpuInfo = new()
            {
                GpuDeviceID = gpu!,
                GpuName = results["GpuName"],
                GpuDescription = results["GpuDescription"],
                GpuHorizontalResolution = results["GpuHorizontalResolution"],
                GpuVerticalResolution = results["GpuVerticalResolution"],
                GpuCurrentRefresh = results["GpuCurrentRefresh"],
                GpuMinRefresh = results["GpuMinRefresh"],
                GpuMaxRefresh = results["GpuMaxRefresh"],
                GpuBitsPerPixel = results["GpuBitsPerPixel"],
                GpuVideoProcessor = results["GpuVideoProcessor"],
                GpuVideoDescription = results["GpuVideoDescription"],
                GpuAdapterRam = results["GpuAdapterRam"],
                GpuNumberOfColors = results["GpuNumberOfColors"]
            };
            gpuInfoList.Add(gpuInfo);
        }
        sw.Stop();
        Debug.WriteLine($"Video info took {sw.ElapsedMilliseconds} ms");
        return gpuInfoList;
    }
    #endregion Build list of video info

    #region Get list of video controllers
    private static List<string?> GetGPUList()
    {
        const string scope = @"\\.\root\CIMV2";
        const string dialect = "WQL";
        const string query = "SELECT DeviceID From Win32_VideoController";

        try
        {
            List<string?> devices = [];
            using CimSession cimSession = CimSession.Create(null);
            devices.AddRange(cimSession.QueryInstances(scope, dialect, query)
                .Select(drive => drive.CimInstanceProperties["DeviceID"]?.Value!.ToString()));
            return devices;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_VideoController call failed.");
            return [];
        }
    }
    #endregion Get list of video controllers

    #region Get WMI info for video controller(s)
    /// <summary>
    /// Get CIM value from Win32_VideoController
    /// </summary>
    /// <returns>IEnumerable or null if there was an exception</returns>
    private static Dictionary<string, string> GetWin32VideoController(string controller)
    {
        const string scope = @"\\.\root\CIMV2";
        const string dialect = "WQL";
        string query = "SELECT DeviceID, Name, Description, CurrentHorizontalResolution, CurrentVerticalResolution, " +
                       "CurrentRefreshRate, MinRefreshRate, MaxRefreshRate, CurrentBitsPerPixel, VideoProcessor, " +
                       "VideoModeDescription, AdapterRam, CurrentNumberOfColors " +
                       $"From Win32_VideoController Where DeviceID = '{controller}'";
        try
        {
            using CimSession cimSession = CimSession.Create(null);
            IEnumerable<CimInstance> instance = cimSession.QueryInstances(scope, dialect, query);
            return instance
                .Select(gpu => new Dictionary<string, string>
                {
                    ["GpuDeviceID"] = CimStringProperty(gpu, "DeviceID"),
                    ["GpuName"] = CimStringProperty(gpu, "Name"),
                    ["GpuDescription"] = CimStringProperty(gpu, "Description"),
                    ["GpuHorizontalResolution"] = CimStringProperty(gpu, "CurrentHorizontalResolution"),
                    ["GpuVerticalResolution"] = CimStringProperty(gpu, "CurrentVerticalResolution"),
                    ["GpuCurrentRefresh"] = CimStringProperty(gpu, "CurrentRefreshRate"),
                    ["GpuMinRefresh"] = CimStringProperty(gpu, "MinRefreshRate"),
                    ["GpuMaxRefresh"] = CimStringProperty(gpu, "MaxRefreshRate"),
                    ["GpuBitsPerPixel"] = CimStringProperty(gpu, "CurrentBitsPerPixel"),
                    ["GpuVideoProcessor"] = CimStringProperty(gpu, "VideoProcessor"),
                    ["GpuVideoDescription"] = CimStringProperty(gpu, "VideoModeDescription"),
                    ["GpuAdapterRam"] = GetAdapterRamInfo(gpu),
                    ["GpuNumberOfColors"] = GetColorsInfo(gpu)
                })
                .FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_VideoController call failed.");
            return new()
            {
                { "GpuDeviceID", "" },
                { "GpuName", "" },
                { "GpuDescription", "" },
                { "GpuHorizontalResolution", "" },
                { "GpuVerticalResolution", "" },
                { "GpuCurrentRefresh", "" },
                { "GpuMinRefresh", "" },
                { "GpuMaxRefresh", "" },
                { "GpuBitsPerPixel", "" },
                { "GpuVideoProcessor", "" },
                { "GpuVideoDescription", "" },
                { "GpuAdapterRam", "" },
                { "GpuNumberOfColors", "" },
            };
        }
    }
    #endregion Get WMI info for video controller(s)

    #region Get WMI information formatted as string
    /// <summary>
    /// Gets WMI video info as string.
    /// </summary>
    /// <param name="instance">The CimInstance instance.</param>
    /// <param name="name">The name of the property.</param>
    /// <returns>A string</returns>
    private static string CimStringProperty(CimInstance instance, string name)
    {
        if (instance.CimInstanceProperties[name] == null)
        {
            _log.Debug($"Value for {name} was null");
            return "Not Available";
        }
        return instance.CimInstanceProperties[name].Value.ToString()!;
    }
    #endregion Get WMI information formatted as string

    #region Adapter Memory
    /// <summary>
    /// Gets video adapter Ram
    /// </summary>
    /// <param name="instance">The CimInstance</param>
    /// <returns>A formatted string</returns>
    private static string GetAdapterRamInfo(CimInstance instance)
    {
        if (instance.CimInstanceProperties["AdapterRam"] == null)
        {
            _log.Debug("Value for AdapterRam was null");
            return "Not Available";
        }
        double ram = Convert.ToDouble(instance.CimInstanceProperties["AdapterRam"].Value);
        if (ram >= Math.Pow(1024, 3))
        {
            ram /= Math.Pow(1024, 3);
            return string.Format(CultureInfo.CurrentCulture, $"{ram:N2} GB");
        }
        ram /= Math.Pow(1024, 2);
        return string.Format(CultureInfo.CurrentCulture, $"{ram:N2} MB");
    }
    #endregion Adapter Memory

    #region Total Colors
    /// <summary>
    /// Gets to number of colors
    /// </summary>
    /// <param name="instance">The CimInstance</param>
    /// <returns>A formatted string</returns>
    private static string GetColorsInfo(CimInstance instance)
    {
        if (instance.CimInstanceProperties["CurrentNumberOfColors"] == null)
        {
            _log.Debug("Value for CurrentNumberOfColors was null");
            return GetStringResource("MsgText_NotAvailable");
        }

        ulong colors = Convert.ToUInt64(instance.CimInstanceProperties["CurrentNumberOfColors"].Value);
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
