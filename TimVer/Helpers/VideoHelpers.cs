// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class to get Video information.
/// </summary>
public static class VideoHelpers
{
    #region Get list of video controllers
    /// <summary>
    /// Gets the DeviceID for each video controller.
    /// </summary>
    /// <returns>A List of controller DeviceID's</returns>
    internal static List<string?> GetGPUList()
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
    /// <param name="controller">DeviceID of the controller</param>
    /// <returns>Video controller information as a Dictionary</returns>
    public static Dictionary<string, string> GetVideoInfo(string controller)
    {
        const string scope = @"\\.\root\CIMV2";
        const string dialect = "WQL";
        string query = "SELECT DeviceID, Name, Description, CurrentHorizontalResolution, CurrentVerticalResolution, " +
                       "CurrentRefreshRate, CurrentBitsPerPixel, VideoProcessor, " +
                       "AdapterRam, CurrentNumberOfColors " +
                       $"From Win32_VideoController Where DeviceID = '{controller}'";
        try
        {
            using CimSession cimSession = CimSession.Create(null);
            IEnumerable<CimInstance> instance = cimSession.QueryInstances(scope, dialect, query);
            Stopwatch sw = Stopwatch.StartNew();
            Dictionary<string, string> info = instance
                .Select(gpu => new Dictionary<string, string>
                {
                    [GetStringResource("GraphicsInfo_GraphicsAdapter")] = CimStringProperty(gpu, "Name"),
                    [GetStringResource("GraphicsInfo_AdapterType")] = CimStringProperty(gpu, "VideoProcessor"),
                    [GetStringResource("GraphicsInfo_Description")] = CimStringProperty(gpu, "Description"),
                    [GetStringResource("GraphicsInfo_DeviceID")] = CimStringProperty(gpu, "DeviceID"),
                    [GetStringResource("GraphicsInfo_CurrentResolution")] = FormatResolution(gpu),
                    [GetStringResource("GraphicsInfo_CurrentRefreshRate")] = FormatCurrentRefresh(gpu),
                    [GetStringResource("GraphicsInfo_AdapterRAM")] = FormatAdapterRamInfo(gpu),
                    [GetStringResource("GraphicsInfo_BitsPerPixel")] = CimStringProperty(gpu, "CurrentBitsPerPixel"),
                    [GetStringResource("GraphicsInfo_NumberOfColors")] = FormatColorsInfo(gpu)
                })
                .FirstOrDefault()!;
            sw.Stop();
            _log.Debug($"Getting video controller info took {sw.Elapsed.TotalMilliseconds:N2} ms");
            return info;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_VideoController call failed.");
            return [];
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

    #region Get current refresh rate
    /// <summary>
    /// Formats the current refresh rate.
    /// </summary>
    /// <param name="instance">The CimInstace</param>
    /// <returns>A formatted string.</returns>
    private static string FormatCurrentRefresh(CimInstance instance)
    {
        string? rate = instance.CimInstanceProperties["CurrentRefreshRate"].Value.ToString();
        return $"{rate} Hz";
    }
    #endregion Get current refresh rate

    #region Get current resolution
    /// <summary>
    /// Formats the current resolution.
    /// </summary>
    /// <param name="instance">The CimInstnace</param>
    /// <returns>A formatted string.</returns>
    private static string FormatResolution(CimInstance instance)
    {
        string? horz = instance.CimInstanceProperties["CurrentHorizontalResolution"].Value.ToString();
        string? vert = instance.CimInstanceProperties["CurrentVerticalResolution"].Value.ToString();
        return $"{horz} x {vert}";
    }
    #endregion Get current resolution

    #region Adapter Memory
    /// <summary>
    /// Gets video adapter Ram.
    /// </summary>
    /// <param name="instance">The CimInstance</param>
    /// <returns>A formatted string.</returns>
    private static string FormatAdapterRamInfo(CimInstance instance)
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
    /// Gets to number of colors.
    /// </summary>
    /// <param name="instance">The CimInstance</param>
    /// <returns>A formatted string.</returns>
    private static string FormatColorsInfo(CimInstance instance)
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
