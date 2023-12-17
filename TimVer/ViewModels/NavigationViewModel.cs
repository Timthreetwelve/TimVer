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
                    _ = builder.AppendLine("WINDOWS INFORMATION");
                    _ = builder.AppendLine("-------------------");
                    _ = builder.Append("Product Name   = ").AppendLine(CombinedInfo.ProdName);
                    _ = builder.Append("Version        = ").AppendLine(CombinedInfo.Version);
                    _ = builder.Append("Build          = ").AppendLine(CombinedInfo.Build);
                    _ = builder.Append("Architecture   = ").AppendLine(CombinedInfo.Arch);
                    _ = builder.Append("Build Branch   = ").AppendLine(CombinedInfo.BuildBranch);
                    _ = builder.Append("Edition ID     = ").AppendLine(CombinedInfo.EditionID);
                    _ = builder.Append("Installed on   = ").AppendLine(CombinedInfo.InstallDate.ToString("f"));
                    _ = builder.Append("Windows Folder = ").AppendLine(CombinedInfo.WindowsFolder);
                    _ = builder.Append("Temp Folder    = ").AppendLine(CombinedInfo.TempFolder);
                    break;
                }

            case ComputerInfoViewModel:
                {
                    _ = builder.AppendLine("COMPUTER INFORMATION");
                    _ = builder.AppendLine("--------------------");
                    _ = builder.Append("Manufacturer    = ").AppendLine(CombinedInfo.Manufacturer);
                    _ = builder.Append("Model           = ").AppendLine(CombinedInfo.Model);
                    _ = builder.Append("Machine Name    = ").AppendLine(CombinedInfo.MachName);
                    _ = builder.Append("Last Rebooted   = ").AppendLine(CombinedInfo.LastBoot.ToString("f"));
                    _ = builder.Append("CPU             = ").AppendLine(CombinedInfo.ProcName);
                    _ = builder.Append("Total Cores     = ").AppendLine(CombinedInfo.ProcCores);
                    _ = builder.Append("Architecture    = ").AppendLine(CombinedInfo.ProcArch);
                    _ = builder.Append("Physical Memory = ").AppendLine(CombinedInfo.TotalMemory);
                    break;
                }

            case EnvVarViewModel:
                {
                    _ = builder.AppendLine("ENVIRONMENT VARIABLES");
                    _ = builder.AppendLine("---------------------");
                    foreach (EnvVariable item in EnvVariable.EnvVariableList)
                    {
                        _ = builder.Append(item.Variable);
                        _ = builder.Append(" = ");
                        _ = builder.AppendLine(item.Value);
                    }
                    break;
                }

            case HistoryViewModel:
                {
                    _ = builder.AppendLine("BUILD HISTORY");
                    _ = builder.AppendLine("-------------");
                    foreach (History item in History.HistoryList)
                    {
                        _ = builder.AppendFormat("{0,-18}", item.HDate);
                        _ = builder.AppendFormat("{0,-12}", item.HBuild);
                        _ = builder.AppendFormat("{0,-6}", item.HVersion);
                        _ = builder.AppendLine(item.HBranch);
                    }
                    break;
                }

            case DriveInfoViewModel:
                {
                    if (TempSettings.Setting.DriveSelectedTab == 0)
                    {
                        _ = builder.AppendLine("LOGICAL DISK DRIVES");
                        _ = builder.AppendLine("-------------------");
                        foreach (LogicalDrives item in CombinedInfo.LogicalDrivesList)
                        {
                            _ = builder.Append("Name         = ").AppendLine(item.Name);
                            _ = builder.Append("Label        = ").AppendLine(item.Label);
                            _ = builder.Append("Drive type   = ").AppendLine(item.DriveType);
                            _ = builder.Append("Format       = ").AppendLine(item.Format);
                            _ = builder.Append("Total size   = ").AppendFormat("{0:N2} GB", item.TotalSize).AppendLine();
                            _ = builder.Append("Free space   = ").AppendFormat("{0:N2} GB", item.GBFree).AppendLine();
                            _ = builder.Append("Percent free = ").AppendFormat("{0:N2} %", item.PercentFree * 100).AppendLine();
                            _ = builder.AppendLine();
                        }
                    }
                    else
                    {
                        _ = builder.AppendLine("PHSYICAL DISK DRIVES");
                        _ = builder.AppendLine("--------------------");
                        foreach (PhysicalDrives item in CombinedInfo.PhysicalDrivesList)
                        {
                            if (UserSettings.Setting.GetPhysicalDrives)
                            {
                                _ = builder.Append("Device ID   = ").AppendLine(item.Index.ToString());
                                _ = builder.Append("Size        = ").AppendFormat("{0:N2} GB", item.Size).AppendLine();
                                _ = builder.Append("Partitions  = ").AppendLine(item.Partitions.ToString());
                                _ = builder.Append("Disk type   = ").AppendLine(item.DiskType);
                                _ = builder.Append("Media type  = ").AppendLine(item.MediaType);
                                _ = builder.Append("Interface   = ").AppendLine(item.Interface);
                                _ = builder.Append("Bus type    = ").AppendLine(item.BusType);
                                _ = builder.Append("Health      = ").AppendLine(item.Health);
                                _ = builder.Append("Name        = ").AppendLine(item.Name);
                                _ = builder.Append("Model       = ").AppendLine(item.Model);
                                _ = builder.AppendLine();
                            }
                            else
                            {
                                _ = builder.AppendLine("Collection of Physical Drive information is disabled in Settings");
                            }
                        }
                    }
                    break;
                }

            case VideoViewModel:
                {
                    _ = builder.AppendLine("GRAPHICS ADAPTERS");
                    _ = builder.AppendLine("-----------------");
                    foreach (GpuInfo item in CombinedInfo.GPUList)
                    {
                        _ = builder.Append("Name                  = ").AppendLine(item.GpuName);
                        _ = builder.Append("Adapter type          = ").AppendLine(item.GpuVideoProcessor);
                        _ = builder.Append("Description           = ").AppendLine(item.GpuDescription);
                        _ = builder.Append("Device ID             = ").AppendLine(item.GpuDeviceID);
                        _ = builder.Append("Horizontal resolution = ").AppendLine(item.GpuHorizontalResolution);
                        _ = builder.Append("Vertical resolution   = ").AppendLine(item.GpuVerticalResolution);
                        _ = builder.Append("Current refresh rate  = ").Append(item.GpuCurrentRefresh).AppendLine(" Hz");
                        _ = builder.Append("Max refresh rate      = ").Append(item.GpuMaxRefresh).AppendLine(" Hz");
                        _ = builder.Append("Min refresh rate      = ").Append(item.GpuMinRefresh).AppendLine(" Hz");
                        _ = builder.Append("Bits per pixel        = ").AppendLine(item.GpuBitsPerPixel);
                        _ = builder.Append("Adapter RAM           = ").AppendLine(item.GpuAdapterRam);
                        _ = builder.Append("Number of colors      = ").AppendLine(item.GpuNumberOfColors);
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
