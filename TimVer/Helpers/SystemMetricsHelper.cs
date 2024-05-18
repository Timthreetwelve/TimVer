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
}
