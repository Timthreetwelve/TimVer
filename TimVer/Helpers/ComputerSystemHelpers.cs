// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Helper class for computer hardware information.
/// </summary>
internal static class ComputerSystemHelpers
{
    #region Get computer hardware info
    /// <summary>
    /// Aggregates computer hardware information.
    /// </summary>
    /// <returns>Computer info as a dictionary.</returns>
    public static Dictionary<string, string> GetComputerInfo()
    {
        Stopwatch sw = Stopwatch.StartNew();
        ComputerInfo info = new()
        {
            BiosManufacturer = BiosHelpers.GetBiosManufacturer(),
            LastBoot = FormatLastBoot(),
            LastBootType = GetBootType(),
            MachineName = Environment.MachineName,
            Manufacturer = CimQuerySys("Manufacturer"),
            Model = CimQuerySys("Model"),
            ProcessorArchitecture = $"{ProcessorHelpers.CimQueryProc("AddressWidth")} bit",
            ProcessorDescription = ProcessorHelpers.CimQueryProc("Description"),
            ProcessorName = ProcessorHelpers.CimQueryProc("Name"),
            UefiMode = UefiHelpers.UEFIEnabled(),
            UefiSecureBoot = UefiHelpers.UEFISecureBoot(),
            Uptime = EnvironmentHelpers.GetUptime(),
            FormattedMemory = GetFormattedMemory(),
            FormattedBiosVersion = GetFormattedBiosVersion(),
            FormattedProcessorCores = FormatProcessorCores(),
            FormattedUptime = FormatUptime(),
        };
        sw.Stop();
        _log.Debug($"Getting hardware info took {sw.Elapsed.TotalMilliseconds:N2} ms");

        return new()
        {
            // The order listed here is the order that properties will appear in the Hardware Info page!
            {GetStringResource("HardwareInfo_Manufacturer"), info.Manufacturer},
            {GetStringResource("HardwareInfo_Model"), info.Model},
            {GetStringResource("HardwareInfo_MachineName"), info.MachineName},
            {GetStringResource("HardwareInfo_LastBoot"), info.LastBoot},
            {GetStringResource("HardwareInfo_LastBootType"), info.LastBootType},
            {GetStringResource("HardwareInfo_Uptime"), info.FormattedUptime},
            {GetStringResource("HardwareInfo_Processor"), info.ProcessorName},
            {GetStringResource("HardwareInfo_ProcessorDescription"), info.ProcessorDescription},
            {GetStringResource("HardwareInfo_ProcessorArch"), info.ProcessorArchitecture},
            {GetStringResource("HardwareInfo_ProcessorCores"), info.FormattedProcessorCores},
            {GetStringResource("HardwareInfo_BiosManufacturer"), info.BiosManufacturer},
            {GetStringResource("HardwareInfo_BiosVersion"), info.FormattedBiosVersion},
            {GetStringResource("HardwareInfo_BiosMode"), info.UefiMode},
            {GetStringResource("HardwareInfo_SecureBoot"), info.UefiSecureBoot},
            {GetStringResource("HardwareInfo_PhysicalMemory"), info.FormattedMemory},
        };
    }
    #endregion Get computer hardware info

    #region Constants
    private const string _scope = @"\\.\root\CIMV2";
    private const string _dialect = "WQL";
    #endregion Constants

    #region Get System information
    /// <summary>
    /// Get CIM value from Win32_ComputerSystem
    /// </summary>
    /// <param name="value">Value to retrieve</param>
    /// <returns>String for value or exception message</returns>
    private static string CimQuerySys(string value)
    {
        try
        {
            CimSession cim = CimSession.Create(null);
            return cim.QueryInstances(_scope, _dialect, $"SELECT {value} From Win32_ComputerSystem")
                .FirstOrDefault()?.CimInstanceProperties[value].Value.ToString()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_ComputerSystem call failed.");
            return ex.Message;
        }
    }
    #endregion Get System information

    #region Format processor cores string
    /// <summary>
    /// Combines processor cores and processor threads into a single string.
    /// </summary>
    private static string FormatProcessorCores()
    {
        string hCores = GetStringResource("HardwareInfo_Cores");
        string hThreads = GetStringResource("HardwareInfo_Threads");
        return $"{ProcessorHelpers.CimQueryProc("NumberOfCores")} {hCores} / {ProcessorHelpers.CimQueryProc("NumberOfLogicalProcessors")} {hThreads}";
    }
    #endregion Format processor cores string

    #region Format memory string
    /// <summary>
    /// Combines installed memory and usable memory into a single string.
    /// </summary>
    private static string GetFormattedMemory()
    {
        string hUsable = GetStringResource("HardwareInfo_Usable");
        string hTotal = GetStringResource("HardwareInfo_Installed");
        return $"{MemoryHelpers.GetInstalledRam()} {hTotal} - {MemoryHelpers.GetUsableRam()} {hUsable}";
    }
    #endregion Format memory string

    #region Format BIOS string
    /// <summary>
    /// Combines BIOS version and date into a single string.
    /// </summary>
    private static string GetFormattedBiosVersion()
    {
        string name = BiosHelpers.GetBiosVersion();
        string date = BiosHelpers.GetBiosDate().ToString("d", CultureInfo.CurrentCulture);
        return $"{name}   {date}";
    }
    #endregion Format BIOS string

    #region Get last restart date & time
    /// <summary>
    /// Get last restart time from Environment.TickCount64.
    /// </summary>
    /// <returns>Restart date and time as DateTime.</returns>
    private static DateTime GetRestartTime()
    {
        TimeSpan t = TimeSpan.FromMilliseconds(Environment.TickCount64);
        return DateTime.Now.Subtract(t);
    }
    #endregion Get last restart date & time

    #region Format last boot time
    /// <summary>
    /// Formats the date to use the current language and date format.
    /// </summary>
    /// <returns>Formatted string.</returns>
    private static string FormatLastBoot()
    {
        DateTime restartDate = GetRestartTime();
        string datePart = restartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern, CultureInfo.CurrentCulture);
        string timePart = restartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern, CultureInfo.CurrentCulture);
        return $"{datePart}  {timePart}";
    }
    #endregion Format last boot time

    #region Format up-time
    /// <summary>
    /// Formats up-time in the current language.
    /// </summary>
    /// <returns>Formatted string.</returns>
    private static string FormatUptime()
    {
        TimeSpan up = EnvironmentHelpers.GetUptime();
        return string.Format(CultureInfo.InvariantCulture, HardwareInfoUptimeString, up.Days, up.Hours, up.Minutes, up.Seconds);
    }
    #endregion Format up-time

    #region Format reboot type
    private static string GetBootType()
    {
        try
        {
            using CimSession cimSession = CimSession.Create(null);
            CimInstance? computerSystem = cimSession.QueryInstances(_scope, _dialect, "SELECT BootupState From Win32_ComputerSystem")
                .FirstOrDefault();
            string? bootupState = computerSystem?.CimInstanceProperties["BootupState"]?.Value?.ToString();

            return bootupState switch
            {
                "Normal boot" => GetStringResource("HardwareInfo_LastBootNormal"),
                "Fail-safe boot" => GetStringResource("HardwareInfo_LastBootSafe"),
                "Fail-safe with network boot" => GetStringResource("HardwareInfo_LastBootSafeNetwork"),
                _ => bootupState ?? string.Empty,
            };
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Win32_ComputerSystem BootupState call failed.");
            return string.Empty;
        }
    }
    #endregion Format reboot type
}
