// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class to get Video information.
/// </summary>
internal static class VideoHelpers
{
    #region Get all video controllers with full info
    /// <summary>
    /// Gets all video controllers with their complete information.
    /// </summary>
    /// <returns>A list of dictionaries containing video controller information</returns>
    internal static List<Dictionary<string, string>> GetAllVideoControllers()
    {
        const string scope = @"\\.\root\CIMV2";
        const string dialect = "WQL";
        const string query = "SELECT DeviceID, Name, Description, CurrentHorizontalResolution, CurrentVerticalResolution, " +
                             "CurrentRefreshRate, CurrentBitsPerPixel, VideoProcessor, " +
                             "AdapterRam, CurrentNumberOfColors " +
                             "FROM Win32_VideoController";
        try
        {
            List<Dictionary<string, string>> controllers = [];
            using CimSession cimSession = CimSession.Create(null);
            Stopwatch sw = Stopwatch.StartNew();

            var instances = cimSession.QueryInstances(scope, dialect, query);

            int displayCount = GetDisplayCount();

            foreach (var gpu in instances)
            {
                Dictionary<string, string> controllerInfo = new()
                {
                    [GetStringResource("GraphicsInfo_GraphicsAdapter")] = CimStringProperty(gpu, "Name"),
                    [GetStringResource("GraphicsInfo_AdapterType")] = CimStringProperty(gpu, "VideoProcessor"),
                    [GetStringResource("GraphicsInfo_Description")] = CimStringProperty(gpu, "Description"),
                    [GetStringResource("GraphicsInfo_DeviceID")] = CimStringProperty(gpu, "DeviceID"),
                    [GetStringResource("GraphicsInfo_CurrentResolution")] = FormatResolution(gpu),
                    [GetStringResource("GraphicsInfo_CurrentRefreshRate")] = FormatCurrentRefresh(gpu),
                    //[GetStringResource("GraphicsInfo_AdapterRAM")] = FormatAdapterRamInfo(gpu),
                    [GetStringResource("GraphicsInfo_BitsPerPixel")] = CimStringProperty(gpu, "CurrentBitsPerPixel"),
                    [GetStringResource("GraphicsInfo_NumberOfColors")] = FormatColorsInfo(gpu),
                    [GetStringResource("GraphicsInfo_NumberOfDisplays")] = displayCount.ToString(CultureInfo.InvariantCulture),
                };
                controllers.Add(controllerInfo);
                _log.Debug($"Added video controller: {controllerInfo[GetStringResource("GraphicsInfo_GraphicsAdapter")]} Device ID is " +
                                  $"{controllerInfo[GetStringResource("GraphicsInfo_DeviceID")]}");
            }

            sw.Stop();
            _log.Debug($"Getting all video controller info took {sw.Elapsed.TotalMilliseconds:N2} ms. Found {controllers.Count} controller(s)");
            return controllers;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_VideoController call failed.");
            return [];
        }
    }
    #endregion Get all video controllers with full info

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
            return GetStringResource("MsgText_NotAvailable");
        }
        if (instance.CimInstanceProperties[name].Value == null)
        {
            _log.Debug($"Value for {name} was null");
            return GetStringResource("MsgText_NotAvailable");
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
        var rateValue = instance.CimInstanceProperties["CurrentRefreshRate"]?.Value;
        if (rateValue == null)
        {
            _log.Debug("CurrentRefreshRate was null");
            return GetStringResource("MsgText_NotAvailable");
        }
        return $"{rateValue} Hz";
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
        object? horzValue = instance.CimInstanceProperties["CurrentHorizontalResolution"]?.Value;
        object? vertValue = instance.CimInstanceProperties["CurrentVerticalResolution"]?.Value;

        if (horzValue == null || vertValue == null)
        {
            _log.Debug("Resolution properties were null");
            return GetStringResource("MsgText_NotAvailable");
        }
        return $"{horzValue} x {vertValue}";
    }
    #endregion Get current resolution

    #region Adapter Memory
    /// <summary>
    /// Gets video adapter Ram.
    /// </summary>
    /// <param name="instance">The CimInstance</param>
    /// <returns>A formatted string.</returns>
#pragma warning disable RCS1213 // Remove unused member declaration
    // Leaving this method in place for now as I may add it back in the future. It is currently commented out in the GetAllVideoControllers method.
    private static string FormatAdapterRamInfo(CimInstance instance)
#pragma warning restore RCS1213 // Remove unused member declaration
    {
        if (instance.CimInstanceProperties["AdapterRam"] == null)
        {
            _log.Debug("Value for AdapterRam was null");
            return GetStringResource("MsgText_NotAvailable");
        }
        double ram = Convert.ToDouble(instance.CimInstanceProperties["AdapterRam"].Value, CultureInfo.InvariantCulture);
        if (ram >= Math.Pow(1024, 3))
        {
            ram /= Math.Pow(1024, 3);
            return string.Format(CultureInfo.CurrentCulture, $"{ram:N2} GB");
        }
        ram /= Math.Pow(1024, 2);
        return string.Format(CultureInfo.CurrentCulture, $"{ram:N2} MB");
    }
    #endregion Adapter Memory

    #region Get display count
    /// <summary>
    /// Gets the count of visible display monitors.
    /// </summary>
    /// <returns>Count of monitors as int.</returns>
    private static int GetDisplayCount()
    {
        const string scope = @"\\.\root\WMI";
        const string dialect = "WQL";
        const string query = "SELECT InstanceName From WmiMonitorID";

        try
        {
            using CimSession cimSession = CimSession.Create(null);
            return cimSession.QueryInstances(scope, dialect, query).Count();
        }
        catch (Exception ex)
        {
            _log.Error(ex, "WmiMonitorID call failed.");
            return -1;
        }
    }
    #endregion Get display count

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

        double colors = Convert.ToDouble(instance.CimInstanceProperties["CurrentNumberOfColors"].Value, CultureInfo.InvariantCulture);
        if (colors == 0)
        {
            _log.Debug("Value for CurrentNumberOfColors was 0");
            return GetStringResource("MsgText_NotAvailable");
        }
        else if (colors >= Math.Pow(1024, 3))
            {
            colors /= (double)Math.Pow(1024, 3);
            return $"{colors:N2} {GetStringResource("MsgText_Billion")}";
        }
        else if (colors >= Math.Pow(1024, 2))
        {
            colors /= (double)Math.Pow(1024, 2);
            return $"{colors:N2} {GetStringResource("MsgText_Million")}";
        }
        else if (colors >= Math.Pow(1024, 1))
        {
            colors /= (double)Math.Pow(1024, 1);
            return $"{colors:N2} {GetStringResource("MsgText_Thousand")}";
        }
        else
        {
            return $"{colors:N0}";
        }
    }
    #endregion Total Colors
}
