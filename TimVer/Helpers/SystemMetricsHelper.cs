// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using static Vanara.PInvoke.User32;

namespace TimVer.Helpers;
/// <summary>
/// Class for GetSystemMetrics via P/Invoke.
/// </summary>
internal static class SystemMetricsHelper
{
    /// <summary>
    /// Gets count of visible display monitors.
    /// </summary>
    /// <returns>Count of monitors as int.</returns>
    public static int GetDisplayCount()
    {
        try
        {
            return GetSystemMetrics(SystemMetric.SM_CMONITORS);
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Call to GetDisplayCount has failed.");
            return -1;
        }
    }

    /// <summary>
    /// Gets value that specifies how the system was started.
    /// </summary>
    /// <returns>
    /// 0 if Normal Boot,
    /// 1 if Safe Mode,
    /// 2 if Safe Mode with Networking
    /// </returns>
    public static int GetCleanBoot()
    {
        try
        {
            return GetSystemMetrics(SystemMetric.SM_CLEANBOOT);
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Call to GetCleanBoot has failed.");
            return -1;
        }
    }
}
