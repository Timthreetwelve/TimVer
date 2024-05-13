// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using static Vanara.PInvoke.User32;

namespace TimVer.Helpers;

internal static class ClipboardHelper
{
    #region Copy text to clipboard
    private const uint _const_CF_UNICODETEXT = 13;

    /// <summary>
    /// Copies text to clipboard using Vanara PInvoke instead of DllImport
    /// </summary>
    /// <param name="text">Text to be placed in the Windows clipboard</param>
    public static bool CopyTextToClipboard(string text)
    {
        if (!OpenClipboard(IntPtr.Zero) || text.Length < 1)
        {
            return false;
        }

        IntPtr global = Marshal.StringToHGlobalUni(text);

        _ = SetClipboardData(_const_CF_UNICODETEXT, global);
        _ = CloseClipboard();

        return true;
    }
    #endregion Copy text to clipboard

    #region Copy a page to clipboard
    /// <summary>
    /// Builds text to be placed in  the Windows clipboard based on the current ViewModel
    /// </summary>
    /// <param name="currentVM">The current ViewModel</param>
    public static void CopyPageToClipboard(object currentVM)
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
                    _ = builder.Append(GetStringResource("HardwareInfo_Manufacturer"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.Manufacturer);
                    _ = builder.Append(GetStringResource("HardwareInfo_Model"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.Model);
                    _ = builder.Append(GetStringResource("HardwareInfo_MachineName"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.MachName);
                    _ = builder.Append(GetStringResource("HardwareInfo_LastBoot"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.LastBoot.ToString("f"));
                    _ = builder.Append(GetStringResource("HardwareInfo_Processor"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.ProcName);
                    _ = builder.Append(GetStringResource("HardwareInfo_ProcessorDescription"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.ProcDescription);
                    _ = builder.Append(GetStringResource("HardwareInfo_ProcessorCores"))
                               .Append(" = ")
                               .Append(CombinedInfo.ProcCores)
                               .Append(' ')
                               .Append(GetStringResource("HardwareInfo_Threads"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.ProcThreads);
                    _ = builder.Append(GetStringResource("HardwareInfo_ProcessorArch"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.ProcArch);
                    _ = builder.Append(GetStringResource("HardwareInfo_BiosManufacturer"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.BiosManufacturer);
                    _ = builder.Append(GetStringResource("HardwareInfo_BiosVersion"))
                               .Append(" = ")
                               .Append(CombinedInfo.BiosName)
                               .Append(" - ")
                               .AppendLine(CombinedInfo.BiosDate.ToString("f"));
                    _ = builder.Append(GetStringResource("HardwareInfo_PhysicalMemory"))
                               .Append(" = ")
                               .Append(CombinedInfo.InstalledMemory)
                               .Append(' ')
                               .Append(GetStringResource("HardwareInfo_Installed"))
                               .Append(" - ")
                               .Append(CombinedInfo.TotalMemory)
                               .Append(' ')
                               .AppendLine(GetStringResource("HardwareInfo_Usable"));
                    break;
                }

            case EnvVarViewModel:
                {
                    _ = builder.AppendLine(GetStringResource("NavTitle_Environment"));
                    _ = builder.AppendLine(new string('-', builder.Length - 2));
                    foreach (EnvVariable item in EnvVariable.EnvVariableList)
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
                        _ = builder.AppendFormat("{0,-18}", item.HDate)
                                   .AppendFormat("{0,-12}", item.HBuild)
                                   .AppendFormat("{0,-6}", item.HVersion)
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
                                       .AppendFormat("{0:N2} ", item.TotalSize)
                                       .AppendLine(giga);
                            _ = builder.Append(GetStringResource("DriveInfo_Free"))
                                       .Append(" = ")
                                       .AppendFormat("{0:N2} ", item.GBFree)
                                       .AppendLine(giga);
                            _ = builder.Append(GetStringResource("DriveInfo_FreePercent"))
                                       .Append(" = ")
                                       .AppendFormat("{0:N2} %", item.PercentFree * 100)
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
                                           .AppendLine(item.Index.ToString());
                                _ = builder.Append(GetStringResource("DriveInfo_Size"))
                                           .Append(" = ")
                                           .AppendFormat("{0:N2} ", item.Size)
                                           .AppendLine(giga);
                                _ = builder.Append(GetStringResource("DriveInfo_Partitions"))
                                           .Append(" = ")
                                           .AppendLine(item.Partitions.ToString());
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
                    foreach (GpuInfo item in CombinedInfo.GPUList)
                    {
                        _ = builder.Append(GetStringResource("GraphicsInfo_GraphicsAdapter"))
                                   .Append(" = ")
                                   .AppendLine(item.GpuName);
                        _ = builder.Append(GetStringResource("GraphicsInfo_AdapterType"))
                                   .Append(" = ")
                                   .AppendLine(item.GpuVideoProcessor);
                        _ = builder.Append(GetStringResource("GraphicsInfo_Description"))
                                   .Append(" = ")
                                   .AppendLine(item.GpuDescription);
                        _ = builder.Append(GetStringResource("GraphicsInfo_DeviceID"))
                                   .Append(" = ")
                                   .AppendLine(item.GpuDeviceID);
                        _ = builder.Append(GetStringResource("GraphicsInfo_CurrentResolution"))
                                   .Append(" = ")
                                   .Append(item.GpuHorizontalResolution)
                                   .Append(" x ")
                                   .AppendLine(item.GpuVerticalResolution);
                        _ = builder.Append(GetStringResource("GraphicsInfo_CurrentRefreshRate"))
                                   .Append(" = ")
                                   .Append(item.GpuCurrentRefresh)
                                   .AppendLine(" Hz");
                        _ = builder.Append(GetStringResource("GraphicsInfo_BitsPerPixel"))
                                   .Append(" = ")
                                   .AppendLine(item.GpuBitsPerPixel);
                        _ = builder.Append(GetStringResource("GraphicsInfo_AdapterRAM"))
                                   .Append(" = ")
                                   .AppendLine(item.GpuAdapterRam);
                        _ = builder.Append(GetStringResource("GraphicsInfo_NumberOfColors"))
                                   .Append(" = ")
                                   .AppendLine(item.GpuNumberOfColors);
                        _ = builder.AppendLine("");
                    }
                    break;
                }

            default:
                SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopyToClipboardInvalid"));
                SystemSounds.Exclamation.Play();
                break;
        }
        if (CopyTextToClipboard(builder.ToString()))
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopiedToClipboard"));
        }
    }
    #endregion Copy a page to clipboard
}
