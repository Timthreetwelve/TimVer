// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    #region MainWindow Instance
    private static readonly MainWindow? _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region Properties
    public static List<FontFamily>? FontList { get; private set; }
    #endregion Properties

    #region Constructor
    public SettingsViewModel()
    {
        FontList ??= [.. Fonts.SystemFontFamilies.OrderBy(x => x.Source)];
    }
    #endregion Constructor

    #region Relay commands
    #region Open folder
    [RelayCommand]
    private static void OpenAppFolder()
    {
        string filePath = string.Empty;
        try
        {
            filePath = Path.Combine(AppInfo.AppDirectory, "Strings.test.xaml");
            if (File.Exists(filePath))
            {
                _ = Process.Start("explorer.exe", $"/select,\"{filePath}\"");
            }
            else
            {
                using Process p = new();
                p.StartInfo.FileName = AppInfo.AppDirectory;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.ErrorDialog = false;
                _ = p.Start();
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error trying to open {filePath}: {ex.Message}");
            _ = new MDCustMsgBox(GetStringResource("MsgText_Error_FileExplorer"),
                     GetStringResource("MsgText_ErrorCaption"),
                     ButtonType.Ok,
                     false,
                     true,
                     _mainWindow,
                     true).ShowDialog();
        }
    }
    #endregion

    #region Open settings
    [RelayCommand]
    private static void OpenSettings()
    {
        ConfigHelpers.SaveSettings();
        TextFileViewer.ViewTextFile(ConfigHelpers.SettingsFileName!);
    }
    #endregion Open settings

    #region Export settings
    [RelayCommand]
    private static void ExportSettings()
    {
        ConfigHelpers.ExportSettings();
    }
    #endregion Export settings

    #region Import settings
    [RelayCommand]
    private static void ImportSettings()
    {
        ConfigHelpers.ImportSettings();
    }
    #endregion Import settings

    #region List (dump) settings to log file
    [RelayCommand]
    private static void DumpSettings()
    {
        ConfigHelpers.DumpSettings();
        NavigationViewModel.ViewLogFile();
    }
    #endregion List (dump) settings to log file

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
    #endregion Relay commands
}
