// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Relay commands
/// </summary>
internal partial class Commands
{
    #region View log file
    [RelayCommand]
    public static void ViewLogFile()
    {
        TextFileViewer.ViewTextFile(NLogHelpers.GetLogfileName());
    }
    #endregion View log file

    #region View readme file
    [RelayCommand]
    public static void ViewReadMeFile()
    {
        TextFileViewer.ViewTextFile(Path.Combine(AppInfo.AppDirectory, "readme.txt"));
    }
    #endregion View readme file

    #region Copy to clipboard
    [RelayCommand]
    public static void CopyToClipboard(object obj)
    {
        if (obj is ContentControl mainContent)
        {
            switch (mainContent.Content)
            {
                case WindowsInfoPage:
                    {
                        StringBuilder builder = new();
                        _ = builder.AppendLine("WINDOWS INFORMATION");
                        _ = builder.Append("Product Name   = ").AppendLine(CombinedInfo.ProdName);
                        _ = builder.Append("Version        = ").AppendLine(CombinedInfo.Version);
                        _ = builder.Append("Build          = ").AppendLine(CombinedInfo.Build);
                        _ = builder.Append("Architecture   = ").AppendLine(CombinedInfo.Arch);
                        _ = builder.Append("Build Branch   = ").AppendLine(CombinedInfo.BuildBranch);
                        _ = builder.Append("Edition ID     = ").AppendLine(CombinedInfo.EditionID);
                        _ = builder.Append("Installed on   = ").AppendLine(CombinedInfo.InstallDate);
                        _ = builder.Append("Windows Folder = ").AppendLine(CombinedInfo.WindowsFolder);
                        _ = builder.Append("Temp Folder    = ").AppendLine(CombinedInfo.TempFolder);
                        Clipboard.SetText(builder.ToString());
                        SnackbarMsg.ClearAndQueueMessage("Windows information copied to the clipboard");

                        break;
                    }

                case ComputerInfoPage:
                    {
                        StringBuilder builder = new();
                        _ = builder.AppendLine("COMPUTER INFORMATION");
                        _ = builder.Append("Manufacturer    = ").AppendLine(CombinedInfo.Manufacturer);
                        _ = builder.Append("Model           = ").AppendLine(CombinedInfo.Model);
                        _ = builder.Append("Machine Name    = ").AppendLine(CombinedInfo.MachName);
                        _ = builder.Append("Last Rebooted   = ").AppendLine(CombinedInfo.LastBoot);
                        _ = builder.Append("CPU             = ").AppendLine(CombinedInfo.ProcName);
                        _ = builder.Append("Total Cores     = ").AppendLine(CombinedInfo.ProcCores);
                        _ = builder.Append("Architecture    = ").AppendLine(CombinedInfo.ProcArch);
                        _ = builder.Append("Physical Memory = ").AppendLine(CombinedInfo.TotalMemory);
                        if (UserSettings.Setting.ShowDrives)
                        {
                            _ = builder.Append("Disk Drives     = ").AppendLine(CombinedInfo.DiskDrives);
                        }
                        Clipboard.SetText(builder.ToString());
                        SnackbarMsg.ClearAndQueueMessage("Computer information copied to the clipboard");
                        break;
                    }

                case EnvVarPage:
                    {
                        StringBuilder builder = new();
                        _ = builder.AppendLine("ENVIRONMENT VARIABLES");
                        foreach (EnvVariable item in EnvVariable.EnvVariableList)
                        {
                            _ = builder.Append(item.Variable);
                            _ = builder.Append(" = ");
                            _ = builder.AppendLine(item.Value);
                        }
                        Clipboard.SetText(builder.ToString());
                        SnackbarMsg.ClearAndQueueMessage("Environment variables copied to the clipboard");
                        break;
                    }

                case HistoryPage:
                    {
                        StringBuilder builder = new();
                        _ = builder.AppendLine("HISTORY");
                        foreach (History item in History.HistoryList)
                        {
                            _ = builder.AppendFormat("{0,-18}", item.HDate);
                            _ = builder.AppendFormat("{0,-12}", item.HBuild);
                            _ = builder.AppendFormat("{0,-6}", item.HVersion);
                            _ = builder.AppendLine(item.HBranch);
                        }
                        Clipboard.SetText(builder.ToString());
                        SnackbarMsg.ClearAndQueueMessage("History copied to the clipboard");

                        break;
                    }

                default:
                    SnackbarMsg.ClearAndQueueMessage("Copy to clipboard is not valid on this page");
                    SystemSounds.Exclamation.Play();
                    break;
            }
        }
    }
    #endregion Copy to clipboard
}
