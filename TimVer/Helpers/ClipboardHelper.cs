// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class ClipboardHelper
{
    #region Copy text to clipboard
    /// <summary>
    /// Copy to clipboard with retry logic to handle potential exceptions when the clipboard is busy.
    /// </summary>
    public static async Task<bool> CopyTextToClipboardAsync(string? text, int maxRetries = 10, int delayMs = 50)
    {
        if (string.IsNullOrEmpty(text) || maxRetries <= 0)
        {
            return false;
        }

        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher is null)
        {
            return false;
        }

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                if (dispatcher.CheckAccess())
                {
                    Clipboard.SetText(text);
                }
                else
                {
                    await dispatcher.InvokeAsync(() => Clipboard.SetText(text));
                }

                return true;
            }
            catch (ExternalException) when (attempt < maxRetries)
            {
                await Task.Delay(delayMs).ConfigureAwait(false);
            }
            catch
            {
                return false;
            }
        }

        return false;
    }
    #endregion Copy text to clipboard

    #region Copy a page to clipboard
    /// <summary>
    /// Builds text to be placed in  the Windows clipboard based on the current ViewModel
    /// </summary>
    /// <param name="currentVM">The current ViewModel</param>
    public static async Task CopyPageToClipboard(object currentVM)
    {
        StringBuilder builder = new();

        bool isSupportedView = currentVM switch
        {
            WindowsInfoViewModel => BuildWindowsInfoClipboardText(builder),
            ComputerInfoViewModel => BuildComputerInfoClipboardText(builder),
            EnvVarViewModel => BuildEnvironmentClipboardText(builder),
            HistoryViewModel => BuildHistoryClipboardText(builder),
            DriveInfoViewModel => BuildDriveInfoClipboardText(builder),
            _ => false
        };

        if (!isSupportedView)
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopyToClipboardInvalid"));
            SystemSounds.Exclamation.Play();
            return;
        }

        if (await CopyTextToClipboardAsync(builder.ToString()))
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopiedToClipboard"));
        }
        else
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopyToClipboardFail"));
            SystemSounds.Exclamation.Play();
        }
    }

    private static bool BuildWindowsInfoClipboardText(StringBuilder builder)
    {
        AppendHeader(builder, GetStringResource("NavTitle_WindowsInfo"));
        AppendKeyValueCollection(builder, WindowsInfoViewModel.WindowsInfoList);
        return true;
    }

    private static bool BuildComputerInfoClipboardText(StringBuilder builder)
    {
        AppendHeader(builder, GetStringResource("NavTitle_HardwareInfo"));
        AppendKeyValueCollection(builder, ComputerInfoViewModel.ComputerInfoList);
        return true;
    }

    private static bool BuildEnvironmentClipboardText(StringBuilder builder)
    {
        AppendHeader(builder, GetStringResource("NavTitle_Environment"));

        foreach (EnvVariable item in EnvVarViewModel.EnvVariableList)
        {
            AppendKeyValueLine(builder, item.Variable!, item.Value);
        }

        return true;
    }

    private static bool BuildHistoryClipboardText(StringBuilder builder)
    {
        AppendHeader(builder, GetStringResource("NavTitle_BuildHistory"));

        foreach (History item in HistoryViewModel.HistoryList)
        {
            _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0,-18}", item.HDate)
                       .AppendFormat(CultureInfo.InvariantCulture, "{0,-12}", item.HBuild)
                       .AppendFormat(CultureInfo.InvariantCulture, "{0,-6}", item.HVersion)
                       .AppendLine(item.HBranch);
        }

        return true;
    }

    private static bool BuildDriveInfoClipboardText(StringBuilder builder)
    {
        string giga = UserSettings.Setting.Use1024 ? "GiB" : "GB";

        if (TempSettings.Setting.DriveSelectedTab == 0)
        {
            AppendHeader(
                builder,
                $"{GetStringResource("NavTitle_DriveInfo")} - {GetStringResource("DriveInfo_LogicalDrives")}");

            foreach (LogicalDrives item in DriveInfoViewModel.LogicalDrivesList)
            {
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Name"), item.Name);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Label"), item.Label);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Type"), item.DriveType);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Format"), item.Format);

                _ = builder.Append(GetStringResource("DriveInfo_Size"))
                           .Append(" = ")
                           .AppendFormat(CultureInfo.InvariantCulture, "{0:N2} ", item.TotalSize)
                           .AppendLine(giga);

                _ = builder.Append(GetStringResource("DriveInfo_Free"))
                           .Append(" = ")
                           .AppendFormat(CultureInfo.InvariantCulture, "{0:N2} ", item.GBFree)
                           .AppendLine(giga);

                _ = builder.Append(GetStringResource("DriveInfo_FreePercent"))
                           .Append(" = ")
                           .AppendFormat(CultureInfo.InvariantCulture, "{0:N2} %", item.PercentFree * 100)
                           .AppendLine();

                _ = builder.AppendLine();
            }

            return true;
        }

        AppendHeader(
            builder,
            $"{GetStringResource("NavTitle_DriveInfo")} - {GetStringResource("DriveInfo_PhysicalDrives")}");

        foreach (PhysicalDrives item in DriveInfoViewModel.PhysicalDrivesList)
        {
            if (UserSettings.Setting.GetPhysicalDrives)
            {
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_DeviceID"), item.Index.ToString(CultureInfo.InvariantCulture));

                _ = builder.Append(GetStringResource("DriveInfo_Size"))
                           .Append(" = ")
                           .AppendFormat(CultureInfo.InvariantCulture, "{0:N2} ", item.Size)
                           .AppendLine(giga);

                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Partitions"), item.Partitions.ToString(CultureInfo.InvariantCulture));
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_DiskType"), item.DiskType);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_MediaType"), item.MediaType);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Interface"), item.Interface);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_BusType"), item.BusType);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Health"), item.Health);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_PartitionStyle"), item.PartitionStyle);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_BootDrive"), item.IsBoot);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Name"), item.Name);
                AppendKeyValueLine(builder, GetStringResource("DriveInfo_Model"), item.Model);
                _ = builder.AppendLine();
            }
            else
            {
                _ = builder.AppendLine(GetStringResource("DriveInfo_PhysicalDisabled"));
            }
        }

        return true;
    }

    private static void AppendHeader(StringBuilder builder, string title)
    {
        _ = builder.AppendLine(title);
        _ = builder.AppendLine(new string('-', title.Length));
    }

    private static void AppendKeyValueCollection(StringBuilder builder, IReadOnlyDictionary<string, string>? values)
    {
        if (values is null)
        {
            return;
        }

        foreach ((string key, string value) in values)
        {
            AppendKeyValueLine(builder, key, value);
        }
    }

    private static void AppendKeyValueLine(StringBuilder builder, string key, string? value)
    {
        _ = builder.Append(key)
                   .Append(" = ")
                   .AppendLine(value);
    }
    #endregion Copy a page to clipboard
}
