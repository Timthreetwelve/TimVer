// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal partial class NavigationViewModel : ObservableObject
{
    #region Constructor
    public NavigationViewModel()
    {
        if (CurrentViewModel == null)
        {
            NavigateToPage(UserSettings.Setting.InitialPage);
        }
    }
    #endregion Constructor

    #region MainWindow Instance
    private static readonly MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region Properties
    [ObservableProperty]
    private object _currentViewModel;

    [ObservableProperty]
    private string _pageTitle;

    [ObservableProperty]
    private static NavigationItem _navItem;
    #endregion Properties

    #region List of navigation items
    public static List<NavigationItem> NavigationViewModelTypes { get; set; } = new List<NavigationItem>
        (new List<NavigationItem>
            {
                new NavigationItem
                {
                    Name=GetStringResource("NavItem_WindowsInfo"),
                    NavPage = NavPage.WindowsInfo,
                    ViewModelType= typeof(WindowsInfoViewModel),
                    IconKind=PackIconKind.Microsoft,
                    PageTitle=GetStringResource("NavTitle_WindowsInfo")
                },
                new NavigationItem
                {
                    Name=GetStringResource("NavItem_HardwareInfo"),
                    NavPage = NavPage.ComputerInfo,
                    ViewModelType= typeof(ComputerInfoViewModel),
                    IconKind=PackIconKind.ComputerClassic,
                    PageTitle=GetStringResource("NavTitle_HardwareInfo")
                },
                new NavigationItem
                {
                    Name=GetStringResource("NavItem_DriveInfo"),
                    NavPage = NavPage.DriveInfo,
                    ViewModelType= typeof(DriveInfoViewModel),
                    IconKind=PackIconKind.Harddisk,
                    PageTitle=GetStringResource("NavTitle_DriveInfo")
                },
                new NavigationItem
                {
                   Name=GetStringResource("NavItem_GraphicsInfo"),
                    NavPage = NavPage.VideoInfo,
                    ViewModelType= typeof(VideoViewModel),
                    IconKind=PackIconKind.Monitor,
                   PageTitle=GetStringResource("NavTitle_GraphicsInfo")
                },
                new NavigationItem
                {
                    Name=GetStringResource("NavItem_Environment"),
                    NavPage = NavPage.Environment,
                    ViewModelType= typeof(EnvVarViewModel),
                    IconKind=PackIconKind.ListBoxOutline,
                    PageTitle=GetStringResource("NavTitle_Environment")
                },
                new NavigationItem
                {
                    Name=GetStringResource("NavItem_BuildHistory"),
                    NavPage = NavPage.History,
                    ViewModelType= typeof(HistoryViewModel),
                    IconKind=PackIconKind.History,
                    PageTitle=GetStringResource("NavTitle_BuildHistory")
                },
                new NavigationItem
                {
                    Name = GetStringResource("NavItem_Settings"),
                    NavPage=NavPage.Settings,
                    ViewModelType= typeof(SettingsViewModel),
                    IconKind=PackIconKind.SettingsOutline,
                     PageTitle = GetStringResource("NavTitle_Settings")
                },
                new NavigationItem
                {
                    Name = GetStringResource("NavItem_About"),
                    NavPage=NavPage.About,
                    ViewModelType= typeof(AboutViewModel),
                    IconKind=PackIconKind.AboutCircleOutline,
                    PageTitle = GetStringResource("NavTitle_About")
                },
                new NavigationItem
                {
                    Name = GetStringResource("NavItem_Exit"),
                    IconKind=PackIconKind.ExitToApp,
                    IsExit=true
                }
            }
        );
    #endregion List of navigation items

    #region List of navigation items without History
    public static List<NavigationItem> NavigationListNoHistory { get; set; } = new List<NavigationItem>();

    public static void PopulateNoHistoryList()
    {
        if (NavigationListNoHistory.Count == 0)
        {
            NavigationListNoHistory.AddRange(NavigationViewModelTypes);
            _ = NavigationListNoHistory.Remove(NavigationViewModelTypes.Find(n => n.NavPage.ToString() == "History"));
        }
    }
    #endregion List of navigation items without History

    #region Navigation Methods
    public void NavigateToPage(NavPage page)
    {
        Navigate(FindNavPage(page));
    }

    private static NavigationItem FindNavPage(NavPage page)
    {
        return NavigationViewModelTypes.Find(x => x.NavPage == page);
    }
    #endregion Navigation Methods

    #region Navigate Command
    [RelayCommand]
    internal void Navigate(object param)
    {
        if (param is NavigationItem item)
        {
            if (item.IsExit)
            {
                Application.Current.Shutdown();
            }
            else if (item.ViewModelType is not null)
            {
                PageTitle = item.PageTitle;
                CurrentViewModel = null;
                CurrentViewModel = Activator.CreateInstance((Type)item.ViewModelType);
                NavItem = item;
            }
        }
    }
    #endregion Navigate Command

    #region Copy to clipboard command
    [RelayCommand]
    public void CopyToClipboard()
    {
        StringBuilder builder = new();
        Clipboard.Clear();

        switch (CurrentViewModel)
        {
            case WindowsInfoViewModel:
                {
                    _ = builder.AppendLine(GetStringResource("NavTitle_WindowsInfo"));
                    _ = builder.AppendLine(new string('-', builder.Length - 2));
                    _ = builder.Append(GetStringResource("WindowsInfo_OSEdition"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.ProdName);
                    _ = builder.Append(GetStringResource("WindowsInfo_OSVersion"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.Version);
                    _ = builder.Append(GetStringResource("WindowsInfo_BuildNumber"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.Build);
                    _ = builder.Append(GetStringResource("WindowsInfo_Architecture"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.Arch);
                    _ = builder.Append(GetStringResource("WindowsInfo_BuildBranch"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.BuildBranch);
                    _ = builder.Append(GetStringResource("WindowsInfo_EditionID"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.EditionID);
                    _ = builder.Append(GetStringResource("WindowsInfo_Installed"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.InstallDate.ToString("f"));
                    _ = builder.Append(GetStringResource("WindowsInfo_WindowsFolder"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.WindowsFolder);
                    _ = builder.Append(GetStringResource("WindowsInfo_TempFolder"))
                               .Append(" = ")
                               .AppendLine(CombinedInfo.TempFolder);
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
                    foreach (History item in History.HistoryList)
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
                    string giga = UserSettings.Setting.Use1024 ? "GiB" : "GB";
                    if (TempSettings.Setting.DriveSelectedTab == 0)
                    {
                        _ = builder.Append(GetStringResource("NavTitle_DriveInfo"))
                                   .Append(" - ")
                                   .AppendLine(GetStringResource("DriveInfo_LogicalDrives"));

                        _ = builder.AppendLine(new string('-', builder.Length - 2));
                        foreach (LogicalDrives item in CombinedInfo.LogicalDrivesList)
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
                                       .Append(giga)
                                       .AppendLine();
                            _ = builder.Append(GetStringResource("DriveInfo_Free"))
                                       .Append(" = ")
                                       .AppendFormat("{0:N2} ", item.GBFree)
                                       .Append(giga)
                                       .AppendLine();
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
                        foreach (PhysicalDrives item in CombinedInfo.PhysicalDrivesList)
                        {
                            if (UserSettings.Setting.GetPhysicalDrives)
                            {
                                _ = builder.Append(GetStringResource("DriveInfo_Index"))
                                           .Append(" = ")
                                           .AppendLine(item.Index.ToString());
                                _ = builder.Append(GetStringResource("DriveInfo_Size"))
                                           .Append(" = ")
                                           .AppendFormat("{0:N2} ", item.Size)
                                           .Append(giga)
                                           .AppendLine();
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
        // Don't merge with following if statement
        if (builder.Length > 0)
        {
            if (ClipboardHelper.CopyTextToClipboard(builder.ToString()))
            {
                SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopiedToClipboard"));
            }
        }
    }
    #endregion Copy to clipboard command

    #region Add and remove from startup in registry
    /// <summary>
    /// Adds history collection to registry
    /// </summary>
    [RelayCommand]
    private static void HistoryOnStartup(RoutedEventArgs e)
    {
        CheckBox box = e.OriginalSource as CheckBox;
        string result;
        switch (box.IsChecked)
        {
            case true:
                if (!RegistryHelpers.RegRunEntry("TimVer"))
                {
                    result = RegistryHelpers.AddRegEntry("TimVer", AppInfo.AppPath + " --hide");
                    if (result == "OK")
                    {
                        _log.Debug(@"TimVer added to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_AddedToStartup"));
                    }
                    else
                    {
                        _log.Info($"TimVer add to startup failed: {result}");
                        _ = new MDCustMsgBox(
                            GetStringResource("MsgText_AddToStartupFailed"),
                            "TimVer",
                            ButtonType.Ok,
                            true,
                            true,
                            Application.Current.MainWindow,
                            true).ShowDialog();
                    }
                }
                break;

            case false:
                result = RegistryHelpers.RemoveRegEntry("TimVer");
                if (result == "OK")
                {
                    _log.Info(@"TimVer removed from HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_RemovedFromStartup"));
                }
                else
                {
                    _log.Info($"Attempt to remove startup entry failed: {result}");
                    _ = new MDCustMsgBox(
                        GetStringResource("MsgText_RemoveFromStartupFailed"),
                        "TimVer",
                        ButtonType.Ok,
                        true,
                        true,
                        Application.Current.MainWindow,
                        true).ShowDialog();
                }
                break;
        }

    }

    #endregion Add and remove from startup in registry

    #region View log file command
    [RelayCommand]
    public static void ViewLogFile()
    {
        TextFileViewer.ViewTextFile(NLogHelpers.GetLogfileName());
    }
    #endregion View log file command

    #region View readme file command
    [RelayCommand]
    public static void ViewReadMeFile()
    {
        TextFileViewer.ViewTextFile(Path.Combine(AppInfo.AppDirectory, "readme.txt"));
    }
    #endregion View readme file command

    #region Open the application folder
    [RelayCommand]
    public static void OpenAppFolder()
    {
        using Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = "Explorer.exe";
        process.StartInfo.Arguments = AppInfo.AppDirectory;
        _ = process.Start();
    }
    #endregion Open the application folder

    #region Refresh drives command
    [RelayCommand]
    public static void RefreshDrives()
    {
        CombinedInfo.LogicalDrivesList.Clear();
        DrivesPage.Instance.LDrivesDataGrid.ItemsSource = CombinedInfo.LogicalDrivesList;

        if (UserSettings.Setting.GetPhysicalDrives)
        {
            CombinedInfo.PhysicalDrivesList.Clear();
            DrivesPage.Instance.PDisksDataGrid.ItemsSource = CombinedInfo.PhysicalDrivesList;
        }
        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_DriveInfoRefreshed"));
    }
    #endregion Refresh drives command

    #region Key down events
    /// <summary>
    /// Keyboard events
    /// </summary>
    [RelayCommand]
    public void KeyDown(KeyEventArgs e)
    {
        #region Keys without modifiers
        switch (e.Key)
        {
            case Key.F1:
                {
                    _mainWindow.NavigationListBox.SelectedValue = FindNavPage(NavPage.About);
                    break;
                }
        }
        #endregion Keys without modifiers

        #region Keys with Ctrl
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
        {
            switch (e.Key)
            {
                case Key.OemComma:
                    {
                        _mainWindow.NavigationListBox.SelectedValue = FindNavPage(NavPage.Settings);
                        break;
                    }
                case Key.C:
                    {
                        CopyToClipboard();
                        break;
                    }
                case Key.Add:
                    {
                        MainWindowUIHelpers.EverythingLarger();
                        string size = EnumDescConverter.GetEnumDescription(UserSettings.Setting.UISize);
                        string message = string.Format(GetStringResource("MsgText_UISizeSet"), size);
                        SnackbarMsg.ClearAndQueueMessage(message, 2000);
                        break;
                    }
                case Key.Subtract:
                    {
                        MainWindowUIHelpers.EverythingSmaller();
                        string size = EnumDescConverter.GetEnumDescription(UserSettings.Setting.UISize);
                        string message = string.Format(GetStringResource("MsgText_UISizeSet"), size);
                        SnackbarMsg.ClearAndQueueMessage(message, 2000);
                        break;
                    }
            }
        }
        #endregion Keys with Ctrl

        #region Keys with Ctrl and Shift
        if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
        {
            if (e.Key == Key.T)
            {
                switch (UserSettings.Setting.UITheme)
                {
                    case ThemeType.Light:
                        UserSettings.Setting.UITheme = ThemeType.Dark;
                        break;
                    case ThemeType.Dark:
                        UserSettings.Setting.UITheme = ThemeType.Darker;
                        break;
                    case ThemeType.Darker:
                        UserSettings.Setting.UITheme = ThemeType.System;
                        break;
                    case ThemeType.System:
                        UserSettings.Setting.UITheme = ThemeType.Light;
                        break;
                }
                string theme = EnumDescConverter.GetEnumDescription(UserSettings.Setting.UITheme);
                string message = string.Format(GetStringResource("MsgText_UIThemeSet"), theme);
                SnackbarMsg.ClearAndQueueMessage(message, 2000);
            }
            if (e.Key == Key.C)
            {
                if (UserSettings.Setting.PrimaryColor >= AccentColor.White)
                {
                    UserSettings.Setting.PrimaryColor = AccentColor.Red;
                }
                else
                {
                    UserSettings.Setting.PrimaryColor++;
                }
                string color = EnumDescConverter.GetEnumDescription(UserSettings.Setting.PrimaryColor);
                string message = string.Format(GetStringResource("MsgText_UIColorSet"), color);
                SnackbarMsg.ClearAndQueueMessage(message, 2000);
            }
            if (e.Key == Key.F)
            {
                using Process p = new();
                p.StartInfo.FileName = AppInfo.AppDirectory;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.ErrorDialog = false;
                _ = p.Start();
            }
            if (e.Key == Key.S)
            {
                TextFileViewer.ViewTextFile(ConfigHelpers.SettingsFileName);
            }
        }
        #endregion Keys with Ctrl and Shift
    }
    #endregion Key down events
}
