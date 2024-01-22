// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class FormatHelpers
{
    public static string FormattedProcessorCores
    {
        get
        {
            string hCores = GetStringResource("HardwareInfo_Cores");
            string hThreads = GetStringResource("HardwareInfo_Threads");
            return $"{CombinedInfo.ProcCores} {hCores} - {CombinedInfo.ProcThreads} {hThreads}";
        }
    }

    public static string FormattedMemory
    {
        get
        {
            string hUsable = GetStringResource("HardwareInfo_Usable");
            string hTotal = GetStringResource("HardwareInfo_Installed");
            return $"{CombinedInfo.InstalledMemory} {hTotal} - {CombinedInfo.TotalMemory} {hUsable}";
        }
    }

    public static string FormattedBiosVersionDate
    {
        get
        {
            string name = BiosHelpers.GetBiosVersion();
            string date = BiosHelpers.GetBiosDate().ToString("d", CultureInfo.CurrentCulture);
            return $"{name}   {date}";
        }
    }
}
