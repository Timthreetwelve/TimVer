// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Helper class for Windows information.
/// </summary>
internal static class WindowsInfoHelpers
{
    #region Get Windows info
    /// <summary>
    /// Aggregates Windows information.
    /// </summary>
    /// <returns>Windows information as a dictionary.</returns>
    public static Dictionary<string, string> GetWindowsInfo()
    {
        Stopwatch sw = Stopwatch.StartNew();
        WindowsInfo info = new()
        {
            DotNetVersion = RuntimeInformation.FrameworkDescription.Replace(".NET", "").Trim(),
            OsArch = OperatingSystemHelpers.OsArchitecture(),
            OsBranch = RegistryHelpers.GetRegistryInfo("BuildBranch"),
            OsBuild = GetBuild(),
            OsEdition = OperatingSystemHelpers.GetWin32OperatingSystem("Caption"),
            OsEditionID = RegistryHelpers.GetRegistryInfo("EditionID"),
            OsInstallDate = FormatInstallDate(),
            OsVersion = GetVersion(),
            TempFolder = Path.GetTempPath(),
            WindowsFolder = EnvironmentHelpers.GetSpecialFolder(Environment.SpecialFolder.Windows),
            RegisteredUser = RegistryHelpers.GetRegistryInfo("RegisteredOwner"),
            RegisteredOrg = RegistryHelpers.GetRegistryInfo("RegisteredOrganization"),
        };
        sw.Stop();
        _log.Debug($"Getting Windows info took {sw.Elapsed.TotalMilliseconds:N2} ms");

        Dictionary<string, string> windowsInfo = new()
        {
            // The order listed here is the order that properties will appear in the Windows Info page!
            {GetStringResource("WindowsInfo_OSEdition"), info.OsEdition},
            {GetStringResource("WindowsInfo_OSVersion"), info.OsVersion},
            {GetStringResource("WindowsInfo_BuildNumber"), info.OsBuild},
            {GetStringResource("WindowsInfo_BuildBranch"), info.OsBranch},
            {GetStringResource("WindowsInfo_Architecture"), info.OsArch},
            {GetStringResource("WindowsInfo_EditionID"), info.OsEditionID},
            {GetStringResource("WindowsInfo_Installed"), info.OsInstallDate},
            {GetStringResource("WindowsInfo_WindowsFolder"), info.WindowsFolder},
            {GetStringResource("WindowsInfo_TempFolder"), info.TempFolder},
            {GetStringResource("WindowsInfo_DotNetVersion"), info.DotNetVersion},
        };
        if (UserSettings.Setting!.ShowUser)
        {
            windowsInfo.Add(GetStringResource("WindowsInfo_RegisteredUser"), info.RegisteredUser);
            windowsInfo.Add(GetStringResource("WindowsInfo_RegisteredOrg"), info.RegisteredOrg);
        }
        return windowsInfo;
    }
    #endregion Get Windows info

    #region Format the install date
    /// <summary>
    /// Formats the installation date to use the current language and date format.
    /// </summary>
    /// <returns>Formatted string.</returns>
    private static string FormatInstallDate()
    {
        DateTime instDate = RegistryHelpers.GetInstallDate();
        string datePart = instDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
        string timePart = instDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
        return $"{datePart}  {timePart}";
    }
    #endregion Format the install date

    #region Format the build number
    /// <summary>
    /// Formats the build number by combining CurrentBuild and UBR.
    /// </summary>
    /// <returns>String in xxxxx.xxx format.</returns>
    public static string GetBuild()
    {
        string curBuild = RegistryHelpers.GetRegistryInfo("CurrentBuild");
        string ubr = RegistryHelpers.GetRegistryInfo("UBR");
        return string.Format($"{curBuild}.{ubr}");
    }
    #endregion

    #region Get Windows version
    /// <summary>
    /// Gets the version number from the registry.
    /// </summary>
    /// <returns>Version as string.</returns>
    public static string GetVersion()
    {
        string result = RegistryHelpers.GetRegistryInfo("DisplayVersion");
        if (result == GetStringResource("MsgText_NotAvailable"))
        {
            result = RegistryHelpers.GetRegistryInfo("ReleaseID");
        }
        return result;
    }
    #endregion Get Windows version
}
