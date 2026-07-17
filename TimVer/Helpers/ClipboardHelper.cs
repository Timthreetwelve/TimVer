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

        switch (currentVM)
        {
            case WindowsInfoViewModel:
                {
                    _ = builder.AppendLine(GetStringResource("NavTitle_WindowsInfo"));
                    _ = builder.AppendLine(new string('-', builder.Length - 2));
                    foreach (KeyValuePair<string, string> item in WindowsInfoViewModel.WindowsInfoList!)
                    {
                        _ = builder.Append(item.Key)
                                   .Append(" = ")
                                   .AppendLine(item.Value);
                    }
                    break;
                }

            case ComputerInfoViewModel:
                {
                    _ = builder.AppendLine(GetStringResource("NavTitle_HardwareInfo"));
                    _ = builder.AppendLine(new string('-', builder.Length - 2));
                    foreach (KeyValuePair<string, string> item in ComputerInfoViewModel.ComputerInfoList!)
                    {
                        _ = builder.Append(item.Key)
                                   .Append(" = ")
                                   .AppendLine(item.Value);
                    }
                    break;
                }

            case EnvVarViewModel:
                {
                    _ = builder.AppendLine(GetStringResource("NavTitle_Environment"));
                    _ = builder.AppendLine(new string('-', builder.Length - 2));
                    foreach (EnvVariable item in EnvVarViewModel.EnvVariableList)
                    {
                        _ = builder.Append(item.Variable)
                                   .Append(" = ")
                                   .AppendLine(item.Value);
                    }
                    break;
                }

            case HistoryViewModel:
                {
                    _ = builder.AppendLine(GetStringResource("NavTitle_BuildHistory"));
                    _ = builder.AppendLine(new string('-', builder.Length - 2));
                    foreach (History item in HistoryViewModel.HistoryList)
                    {
                        _ = builder.AppendFormat(CultureInfo.InvariantCulture, "{0,-18}", item.HDate)
                                   .AppendFormat(CultureInfo.InvariantCulture,"{0,-12}", item.HBuild)
                                   .AppendFormat(CultureInfo.InvariantCulture, "{0,-6}", item.HVersion)
                                   .AppendLine(item.HBranch);
                    }
                    break;
                }

            case DriveInfoViewModel:
                {
                    string giga = UserSettings.Setting!.Use1024 ? "GiB" : "GB";
                    if (TempSettings.Setting!.DriveSelectedTab == 0)
                    {
                        _ = builder.Append(GetStringResource("NavTitle_DriveInfo"))
                                   .Append(" - ")
                                   .AppendLine(GetStringResource("DriveInfo_LogicalDrives"));

                        _ = builder.AppendLine(new string('-', builder.Length - 2));
                        foreach (LogicalDrives item in DriveInfoViewModel.LogicalDrivesList)
                        {
                            _ = builder.Append(GetStringResource("DriveInfo_Name"))
                                       .Append(" = ")
                                       .AppendLine(item.Name);
                            _ = builder.Append(GetStringResource("DriveInfo_Label"))
                                       .Append(" = ")
                                       .AppendLine(item.Label);
                            _ = builder.Append(GetStringResource("DriveInfo_Type"))
                                       .Append(" = ")
                                       .AppendLine(item.DriveType);
                            _ = builder.Append(GetStringResource("DriveInfo_Format"))
                                       .Append(" = ")
                                       .AppendLine(item.Format);
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
                                       .AppendFormat(CultureInfo.InvariantCulture,"{0:N2} %", item.PercentFree * 100)
                                       .AppendLine();
                            _ = builder.AppendLine();
                        }
                    }
                    else
                    {
                        _ = builder.Append(GetStringResource("NavTitle_DriveInfo"))
                                   .Append(" - ")
                                   .AppendLine(GetStringResource("DriveInfo_PhysicalDrives"));
                        _ = builder.AppendLine(new string('-', builder.Length - 2));
                        foreach (PhysicalDrives item in DriveInfoViewModel.PhysicalDrivesList)
                        {
                            if (UserSettings.Setting.GetPhysicalDrives)
                            {
                                _ = builder.Append(GetStringResource("DriveInfo_DeviceID"))
                                           .Append(" = ")
                                           .AppendLine(item.Index.ToString(CultureInfo.InvariantCulture));
                                _ = builder.Append(GetStringResource("DriveInfo_Size"))
                                           .Append(" = ")
                                           .AppendFormat(CultureInfo.InvariantCulture, "{0:N2} ", item.Size)
                                           .AppendLine(giga);
                                _ = builder.Append(GetStringResource("DriveInfo_Partitions"))
                                           .Append(" = ")
                                           .AppendLine(item.Partitions.ToString(CultureInfo.InvariantCulture));
                                _ = builder.Append(GetStringResource("DriveInfo_DiskType"))
                                           .Append(" = ")
                                           .AppendLine(item.DiskType);
                                _ = builder.Append(GetStringResource("DriveInfo_MediaType"))
                                           .Append(" = ")
                                           .AppendLine(item.MediaType);
                                _ = builder.Append(GetStringResource("DriveInfo_Interface"))
                                           .Append(" = ")
                                           .AppendLine(item.Interface);
                                _ = builder.Append(GetStringResource("DriveInfo_BusType"))
                                           .Append(" = ")
                                           .AppendLine(item.BusType);
                                _ = builder.Append(GetStringResource("DriveInfo_Health"))
                                           .Append(" = ")
                                           .AppendLine(item.Health);
                                _ = builder.Append(GetStringResource("DriveInfo_PartitionStyle"))
                                           .Append(" = ")
                                           .AppendLine(item.PartitionStyle);
                                _ = builder.Append(GetStringResource("DriveInfo_BootDrive"))
                                           .Append(" = ")
                                           .AppendLine(item.IsBoot);
                                _ = builder.Append(GetStringResource("DriveInfo_Name"))
                                           .Append(" = ")
                                           .AppendLine(item.Name);
                                _ = builder.Append(GetStringResource("DriveInfo_Model"))
                                           .Append(" = ")
                                           .AppendLine(item.Model);
                                _ = builder.AppendLine();
                            }
                            else
                            {
                                _ = builder.AppendLine(GetStringResource("DriveInfo_PhysicalDisabled"));
                            }
                        }
                    }
                    break;
                }

            case VideoViewModel:
                {
                    _ = builder.AppendLine(GetStringResource("NavTitle_GraphicsInfo"));
                    _ = builder.AppendLine(new string('-', builder.Length - 2));
                    foreach (KeyValuePair<string, string> item in VideoViewModel.VideoInfoCollection!)
                    {
                        _ = builder.Append(item.Key)
                                   .Append(" = ")
                                   .AppendLine(item.Value);
                    }
                    break;
                }

            default:
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
    #endregion Copy a page to clipboard
}
