// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class FormatHelpers
{
    #region Format processor string
    /// <summary>
    /// Combines processor cores and processor threads into a single string.
    /// </summary>
    public static string FormattedProcessorCores
    {
        get
        {
            string hCores = GetStringResource("HardwareInfo_Cores");
            string hThreads = GetStringResource("HardwareInfo_Threads");
            return $"{CombinedInfo.ProcCores} {hCores} - {CombinedInfo.ProcThreads} {hThreads}";
        }
    }
    #endregion Format processor string

    #region Format memory string
    /// <summary>
    /// Combines installed memory and usable memory into a single string.
    /// </summary>
    public static string FormattedMemory
    {
        get
        {
            string hUsable = GetStringResource("HardwareInfo_Usable");
            string hTotal = GetStringResource("HardwareInfo_Installed");
            return $"{CombinedInfo.InstalledMemory} {hTotal} - {CombinedInfo.TotalMemory} {hUsable}";
        }
    }
    #endregion Format memory string

    #region Format BIOS string
    /// <summary>
    /// Combines BIOS version and date into a single string.
    /// </summary>
    public static string FormattedBiosVersionDate
    {
        get
        {
            string name = BiosHelpers.GetBiosVersion();
            string date = BiosHelpers.GetBiosDate().ToString("d", CultureInfo.CurrentCulture);
            return $"{name}   {date}";
        }
    }
    #endregion Format BIOS string
}
