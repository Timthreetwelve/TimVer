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
                    Name="Windows Info",
                    NavPage = NavPage.WindowsInfo,
                    ViewModelType= typeof(WindowsInfoViewModel),
                    IconKind=PackIconKind.Microsoft,
                    PageTitle="Windows Information"
                },
                new NavigationItem
                {
                    Name="Hardware Info",
                    NavPage = NavPage.ComputerInfo,
                    ViewModelType= typeof(ComputerInfoViewModel),
                    IconKind=PackIconKind.ComputerClassic,
                    PageTitle="Hardware Information"
                },
                new NavigationItem
                {
                    Name="Drive Info",
                    NavPage = NavPage.DriveInfo,
                    ViewModelType= typeof(DriveInfoViewModel),
                    IconKind=PackIconKind.Harddisk,
                    PageTitle="Disk Drive Information"
                },
                new NavigationItem
                {
                    Name="Graphics Info",
                    NavPage = NavPage.VideoInfo,
                    ViewModelType= typeof(VideoViewModel),
                    IconKind=PackIconKind.Monitor,
                    PageTitle="Graphics Information"
                },
                new NavigationItem
                {
                    Name="Environment",
                    NavPage = NavPage.Environment,
                    ViewModelType= typeof(EnvVarViewModel),
                    IconKind=PackIconKind.ListBoxOutline,
                    PageTitle="Environment Variables"
                },
                new NavigationItem
                {
                    Name="History",
                    NavPage = NavPage.History,
                    ViewModelType= typeof(HistoryViewModel),
                    IconKind=PackIconKind.History,
                    PageTitle="Build History"
                },
                new NavigationItem
                {
                    Name="Settings",
                    NavPage=NavPage.Settings,
                    ViewModelType= typeof(SettingsViewModel),
                    IconKind=PackIconKind.SettingsOutline,
                    PageTitle = "Settings"
                },
                new NavigationItem
                {
                    Name="About",
                    NavPage=NavPage.About,
                    ViewModelType= typeof(AboutViewModel),
                    IconKind=PackIconKind.AboutCircleOutline,
                    PageTitle = "About TimVer"
                },
                new NavigationItem
                {
                    Name="Exit",
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
            _ = NavigationListNoHistory.Remove(NavigationViewModelTypes.Find(n => n.Name == "History"));
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
        switch (CurrentViewModel)
        {
            case WindowsInfoViewModel:
                {
                    StringBuilder builder = new();
                    _ = builder.AppendLine("WINDOWS INFORMATION");
                    _ = builder.AppendLine("-------------------");
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

            case ComputerInfoViewModel:
                {
                    StringBuilder builder = new();
                    _ = builder.AppendLine("COMPUTER INFORMATION");
                    _ = builder.AppendLine("--------------------");
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
                    SnackbarMsg.ClearAndQueueMessage("Hardware information copied to the clipboard");
                    break;
                }

            case EnvVarViewModel:
                {
                    StringBuilder builder = new();
                    _ = builder.AppendLine("ENVIRONMENT VARIABLES");
                    _ = builder.AppendLine("---------------------");
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

            case HistoryViewModel:
                {
                    StringBuilder builder = new();
                    _ = builder.AppendLine("BUILD HISTORY");
                    _ = builder.AppendLine("-------------");
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

            case DriveInfoViewModel:
                {
                    StringBuilder builder = new();
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
                        SnackbarMsg.ClearAndQueueMessage("Logical drive information copied to the clipboard");
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
                        SnackbarMsg.ClearAndQueueMessage("Physical drive information copied to the clipboard");
                    }
                    Clipboard.SetText(builder.ToString());
                    break;
                }

            case VideoViewModel:
                {
                    StringBuilder builder = new();
                    _ = builder.AppendLine("GRAPHICS ADAPTERS");
                    _ = builder.AppendLine("-----------------");
                    foreach (GpuInfo item in CombinedInfo.GpuList)
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
                    Clipboard.SetText(builder.ToString());
                    SnackbarMsg.ClearAndQueueMessage("Graphics adapters copied to the clipboard");
                    break;
                }

            default:
                SnackbarMsg.ClearAndQueueMessage("Copy to clipboard is not valid on this page");
                SystemSounds.Exclamation.Play();
                break;
        }
    }
    #endregion Copy to clipboard command

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

    #region Refresh command
    [RelayCommand]
    public void Refresh()
    {
        switch (CurrentViewModel)
        {
            case DriveInfoViewModel:
                CombinedInfo.LogicalDrivesList.Clear();
                DrivesPage.Instance.LDrivesDataGrid.ItemsSource = CombinedInfo.LogicalDrivesList;

                if (UserSettings.Setting.GetPhysicalDrives)
                {
                    CombinedInfo.PhysicalDrivesList.Clear();
                    DrivesPage.Instance.PDisksDataGrid.ItemsSource = CombinedInfo.PhysicalDrivesList;
                }
                SnackbarMsg.ClearAndQueueMessage("Disk drive info refreshed.");
                break;

            default:
                SnackbarMsg.ClearAndQueueMessage("Refresh is not available on this page.");
                break;
        }
    }
    #endregion Refresh command

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
                    //NavigateToPage(NavPage.About);
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
                        //NavigateToPage(NavPage.Settings);
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
                        SnackbarMsg.ClearAndQueueMessage($"Size set to {UserSettings.Setting.UISize}");
                        break;
                    }
                case Key.Subtract:
                    {
                        MainWindowUIHelpers.EverythingSmaller();
                        SnackbarMsg.ClearAndQueueMessage($"Size set to {UserSettings.Setting.UISize}");
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
                string theme = Converters.EnumDescConverter.GetEnumDescription(UserSettings.Setting.UITheme);
                SnackbarMsg.ClearAndQueueMessage($"Theme set to {theme}", 2000);
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
                string color = Converters.EnumDescConverter.GetEnumDescription(UserSettings.Setting.PrimaryColor);
                SnackbarMsg.ClearAndQueueMessage($"Accent color set to {color}");
            }
            if (e.Key == Key.S)
            {
                TextFileViewer.ViewTextFile(ConfigHelpers.SettingsFileName);
                SnackbarMsg.ClearAndQueueMessage("Opening settings file", 2000);
            }
        }
        #endregion
    }
    #endregion Key down events
}
