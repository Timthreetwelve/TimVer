// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal sealed partial class NavigationViewModel : ObservableObject
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
    private static readonly MainWindow? _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region Properties
#pragma warning disable MVVMTK0042 // Prefer using [ObservableProperty] on partial properties
    // Suppressing the MVVMTK0042 warning for this class until such time as it no longer requires Preview features.
    [ObservableProperty]
    private object? _currentViewModel;

    [ObservableProperty]
    private string? _pageTitle;

    [ObservableProperty]
    private static NavigationItem? _navItem;

    [ObservableProperty]
    private bool _showCopyButton = true;
#pragma warning restore MVVMTK0042
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
                ShowCopyButton = item.NavPage switch
                {
                    NavPage.Settings => false,
                    NavPage.About => false,
                    _ => true
                };
            }
        }
    }
    #endregion Navigate Command

    #region Copy to clipboard command
    [RelayCommand]
    private async Task CopyToClipboard()
    {
        await ClipboardHelper.CopyPageToClipboard(CurrentViewModel!);
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
    private static void ViewReadMeFile()
    {
        TextFileViewer.ViewTextFile(Path.Combine(AppInfo.AppDirectory, "readme.txt"));
    }
    #endregion View readme file command

    #region Open the application folder
    [RelayCommand]
    private static void OpenAppFolder()
    {
        string fileName = PathHelpers.FindOnPath("Explorer.exe");
        if (fileName == string.Empty)
        {
            _log.Error("Error trying to open application folder: Explorer.exe not found");
            string msg = $"{GetStringResource("MsgText_Error_FileExplorer")}" +
                         $"\n\n{GetStringResource("MsgText_SeeLogFile")}";
            _ = new MDCustMsgBox(msg,
                     GetStringResource("MsgText_ErrorCaption"),
                     ButtonType.Ok,
                     false,
                     true,
                     _mainWindow,
                     true).ShowDialog();
            return;
        }
        using Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = fileName;
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
    private static async Task RightMouseUp(MouseButtonEventArgs e)
    {
        if (e.OriginalSource is not TextBlock text)
        {
            return;
        }

        try
        {
            if (await ClipboardHelper.CopyTextToClipboardAsync(text.Text))
            {
                SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopiedToClipboardItem"));
                _log.Debug($"{text.Text.Length} bytes copied to the clipboard");
            }
            else
            {
                _log.Error("RightMouseUp clipboard copy failed.");
                SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopyToClipboardFail"));
            }

            DataGridRow dgr = MainWindowHelpers.FindParent<DataGridRow>(text);
            dgr.IsSelected = false;
            DataGrid dg = MainWindowHelpers.FindParent<DataGrid>(dgr);
            dg.Items.Refresh();
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Right-click event handler failed. {ex.Message}");
        }
    }
    #endregion Right mouse button

    #region Key down events
    /// <summary>
    /// Keyboard events
    /// </summary>
    [RelayCommand]
    private async Task KeyDown(KeyEventArgs e)
    {
        // The case statements are mostly in order by modifier keys (none, alt, control, control+shift), then key.
        // The underscore (_) is a discard for the value that is not needed.
        // The SystemKey is used for Alt key combinations, but is not needed for this application.
        // The e.Handled = true; statement prevents the key event from being processed further.
        switch (e.KeyboardDevice.Modifiers, e.Key, e.SystemKey)
        {
            // No modifiers
            case (ModifierKeys.None, Key.F1, _):
                e.Handled = true;
                _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.About);
                break;

            // With control
            case (ModifierKeys.Control, Key.C, _):
                e.Handled = true;
                await CopyToClipboard();
                break;

            case (ModifierKeys.Control, Key.OemComma, _):
                e.Handled = true;
                _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.Settings);
                break;

            case (ModifierKeys.Control, Key.Add, _):
            case (ModifierKeys.Control, Key.OemPlus, _):
                e.Handled = true;
                MainWindowHelpers.EverythingLarger();
                ShowUIChangeMessage("size");
                break;

            case (ModifierKeys.Control, Key.Subtract, _):
            case (ModifierKeys.Control, Key.OemMinus, _):
                e.Handled = true;
                MainWindowHelpers.EverythingSmaller();
                ShowUIChangeMessage("size");
                break;

            // With control and shift
            case (ModifierKeys.Control | ModifierKeys.Shift, Key.C, _):
                e.Handled = true;
                CycleColor();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.F, _):
                e.Handled = true;
                OpenAppFolder();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.K, _):
                e.Handled = true;
                CompareLanguageDictionaries();
                ViewLogFile();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.R, _):
                e.Handled = true;
                CycleRowSpacing();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.S, _):
                e.Handled = true;
                TextFileViewer.ViewTextFile(ConfigHelpers.SettingsFileName!);
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.T, _):
                e.Handled = true;
                CycleTheme();
                break;
        }
    }
    #endregion Key down events

    #region Helpers for key down events
    // The following methods are called by the KeyDown method above. They are separated out for clarity.
    // Hopefully the name of each method is self-explanatory.
    private static void CycleColor()
    {
        if (UserSettings.Setting.PrimaryColor >= AccentColor.White)
        {
            UserSettings.Setting.PrimaryColor = AccentColor.Red;
        }
        else
        {
            UserSettings.Setting.PrimaryColor++;
        }
        ShowUIChangeMessage("color");
    }

    private static void CycleRowSpacing()
    {
        if (UserSettings.Setting.RowSpacing >= Spacing.Wide)
        {
            UserSettings.Setting.RowSpacing = Spacing.Compact;
        }
        else
        {
            UserSettings.Setting.RowSpacing++;
        }
    }

    private static void CycleTheme()
    {
        UserSettings.Setting.UITheme = UserSettings.Setting.UITheme switch
        {
            ThemeType.Light => ThemeType.LightGray,
            ThemeType.LightGray => ThemeType.Dark,
            ThemeType.Dark => ThemeType.Darker,
            ThemeType.Darker => ThemeType.DarkBlue,
            ThemeType.DarkBlue => ThemeType.System,
            _ => ThemeType.Light,
        };
        ShowUIChangeMessage("theme");
    }
    #endregion Helpers for key down events

    #region Show snack bar message for UI changes
    private static void ShowUIChangeMessage(string messageType)
    {
        CompositeFormat? composite = null;
        string messageVar = string.Empty;

        switch (messageType)
        {
            case "size":
                composite = MsgTextUISizeSet;
                messageVar = EnumHelpers.GetEnumDescription(UserSettings.Setting.UISize);
                break;
            case "theme":
                composite = MsgTextUIThemeSet;
                messageVar = EnumHelpers.GetEnumDescription(UserSettings.Setting.UITheme);
                break;
            case "color":
                composite = MsgTextUIColorSet;
                messageVar = EnumHelpers.GetEnumDescription(UserSettings.Setting.PrimaryColor);
                break;
        }

        string message = string.Format(CultureInfo.InvariantCulture, composite!, messageVar);
        SnackbarMsg.ClearAndQueueMessage(message, 2000);
    }
    #endregion Show snack bar message for UI changes
}
