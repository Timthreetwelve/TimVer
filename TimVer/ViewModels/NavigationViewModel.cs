// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal partial class NavigationViewModel : ObservableObject
{
    #region Constructor
    public NavigationViewModel()
    {
        if (CurrentViewModel == null)
        {
            NavigateToPage(UserSettings.Setting!.InitialPage);
        }
    }
    #endregion Constructor

    #region MainWindow Instance
    private static readonly MainWindow? _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region Properties
    [ObservableProperty]
    private object? _currentViewModel;

    [ObservableProperty]
    private string? _pageTitle;

    [ObservableProperty]
    private static NavigationItem? _navItem;
    #endregion Properties

    #region List of navigation items
    public static List<NavigationItem> NavigationViewModelTypes { get; } =
    [
        new() {
            Name=GetStringResource("NavItem_WindowsInfo"),
            NavPage = NavPage.WindowsInfo,
            ViewModelType= typeof(WindowsInfoViewModel),
            IconKind=PackIconKind.Microsoft,
            PageTitle=GetStringResource("NavTitle_WindowsInfo")
        },
        new() {
            Name=GetStringResource("NavItem_HardwareInfo"),
            NavPage = NavPage.ComputerInfo,
            ViewModelType= typeof(ComputerInfoViewModel),
            IconKind=PackIconKind.ComputerClassic,
            PageTitle=GetStringResource("NavTitle_HardwareInfo")
        },
        new() {
            Name=GetStringResource("NavItem_DriveInfo"),
            NavPage = NavPage.DriveInfo,
            ViewModelType= typeof(DriveInfoViewModel),
            IconKind=PackIconKind.Harddisk,
            PageTitle=GetStringResource("NavTitle_DriveInfo")
        },
        new() {
            Name=GetStringResource("NavItem_GraphicsInfo"),
            NavPage = NavPage.VideoInfo,
            ViewModelType= typeof(VideoViewModel),
            IconKind=PackIconKind.Monitor,
            PageTitle=GetStringResource("NavTitle_GraphicsInfo")
        },
        new() {
            Name=GetStringResource("NavItem_Environment"),
            NavPage = NavPage.Environment,
            ViewModelType= typeof(EnvVarViewModel),
            IconKind=PackIconKind.ListBoxOutline,
            PageTitle=GetStringResource("NavTitle_Environment")
        },
        new() {
            Name=GetStringResource("NavItem_BuildHistory"),
            NavPage = NavPage.History,
            ViewModelType= typeof(HistoryViewModel),
            IconKind=PackIconKind.History,
            PageTitle=GetStringResource("NavTitle_BuildHistory"),
            IsHistory = true
        },
        new() {
            Name = GetStringResource("NavItem_Settings"),
            NavPage=NavPage.Settings,
            ViewModelType= typeof(SettingsViewModel),
            IconKind=PackIconKind.SettingsOutline,
            PageTitle = GetStringResource("NavTitle_Settings")
        },
        new() {
            Name = GetStringResource("NavItem_About"),
            NavPage=NavPage.About,
            ViewModelType= typeof(AboutViewModel),
            IconKind=PackIconKind.AboutCircleOutline,
            PageTitle = GetStringResource("NavTitle_About")
        },
        new() {
            Name = GetStringResource("NavItem_Exit"),
            IconKind=PackIconKind.ExitToApp,
            IsExit=true
        }
    ];
    #endregion List of navigation items

    #region Navigation Methods
    private void NavigateToPage(NavPage page)
    {
        Navigate(FindNavPage(page));
    }

    private static NavigationItem FindNavPage(NavPage page)
    {
        return NavigationViewModelTypes.Find(x => x.NavPage == page)!;
    }
    #endregion Navigation Methods

    #region Navigate Command
    [RelayCommand]
    private void Navigate(object param)
    {
        if (param is NavigationItem item)
        {
            if (item.IsExit == true)
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
    private void CopyToClipboard()
    {
        ClipboardHelper.CopyPageToClipboard(CurrentViewModel!);
    }
    #endregion Copy to clipboard command

    #region Add and remove from startup in registry
    /// <summary>
    /// Adds history collection to registry
    /// </summary>
    [RelayCommand]
    private static void HistoryOnStartup(RoutedEventArgs e)
    {
        CheckBox? box = e.OriginalSource as CheckBox;
        string result;
        switch (box!.IsChecked)
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
                            GetStringResource("MsgText_ErrorCaption"),
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
                        GetStringResource("MsgText_ErrorCaption"),
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
    private static void ViewLogFile()
    {
        TextFileViewer.ViewTextFile(NLogHelpers.GetLogfileName());
    }
    #endregion View log file command

    #region View readme file command
    [RelayCommand]
    private static void ViewReadMeFile()
    {
        TextFileViewer.ViewTextFile(Path.Combine(AppInfo.AppDirectory, "readme.txt"));
    }
    #endregion View readme file command

    #region Open the application folder
    [RelayCommand]
    private static void OpenAppFolder()
    {
        using Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = "Explorer.exe";
        process.StartInfo.Arguments = AppInfo.AppDirectory;
        _ = process.Start();
    }
    #endregion Open the application folder

    #region Check for new release
    [RelayCommand]
    private static async Task CheckReleaseAsync()
    {
        await GitHubHelpers.CheckRelease();
    }
    #endregion Check for new release

    #region Right mouse button
    /// <summary>
    /// Copy (nearly) any text in a TextBlock to the clipboard on right mouse button up.
    /// </summary>
    [RelayCommand]
    private static void RightMouseUp(MouseButtonEventArgs e)
    {
        if (e.OriginalSource is TextBlock text)
        {
            try
            {
                // Skip the navigation menu
                ListBox lb = MainWindowHelpers.FindParent<ListBox>(text);
                if (lb?.Name == "NavigationListBox")
                {
                    return;
                }

                // Copy to clipboard and display message.
                if (ClipboardHelper.CopyTextToClipboard(text.Text))
                {
                    SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopiedToClipboardItem"));
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Right-click event handler failed. {ex.Message}");
            }
        }
    }
    #endregion Right mouse button

    #region Key down events
    /// <summary>
    /// Keyboard events
    /// </summary>
    [RelayCommand]
    private void KeyDown(KeyEventArgs e)
    {
        #region Keys without modifiers
        switch (e.Key)
        {
            case Key.F1:
                {
                    _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.About);
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
                        _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.Settings);
                        break;
                    }
                case Key.C:
                    {
                        CopyToClipboard();
                        break;
                    }
                case Key.Add:
                case Key.OemPlus:
                    {
                        MainWindowHelpers.EverythingLarger();
                        string size = EnumHelpers.GetEnumDescription(UserSettings.Setting!.UISize);
                        string message = string.Format(GetStringResource("MsgText_UISizeSet"), size);
                        SnackbarMsg.ClearAndQueueMessage(message, 2000);
                        break;
                    }
                case Key.Subtract:
                case Key.OemMinus:
                    {
                        MainWindowHelpers.EverythingSmaller();
                        string size = EnumHelpers.GetEnumDescription(UserSettings.Setting!.UISize);
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
                switch (UserSettings.Setting!.UITheme)
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
                string theme = EnumHelpers.GetEnumDescription(UserSettings.Setting.UITheme);
                string message = string.Format(GetStringResource("MsgText_UIThemeSet"), theme);
                SnackbarMsg.ClearAndQueueMessage(message, 2000);
            }
            if (e.Key == Key.C)
            {
                if (UserSettings.Setting!.PrimaryColor >= AccentColor.White)
                {
                    UserSettings.Setting.PrimaryColor = AccentColor.Red;
                }
                else
                {
                    UserSettings.Setting.PrimaryColor++;
                }
                string color = EnumHelpers.GetEnumDescription(UserSettings.Setting.PrimaryColor);
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
            if (e.Key == Key.R)
            {
                if (UserSettings.Setting?.RowSpacing >= Spacing.Wide)
                {
                    UserSettings.Setting.RowSpacing = Spacing.Compact;
                }
                else
                {
                    UserSettings.Setting!.RowSpacing++;
                }
            }
            if (e.Key == Key.S)
            {
                TextFileViewer.ViewTextFile(ConfigHelpers.SettingsFileName!);
            }
        }
        #endregion Keys with Ctrl and Shift
    }
    #endregion Key down events
}
