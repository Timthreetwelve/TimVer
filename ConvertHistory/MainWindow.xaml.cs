// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace ConvertHistory;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

    public static string InputCsv { get; } = Path.Join(AppInfo.AppDirectory, "history.csv");
    public static string OutputJson { get; } = Path.Join(AppInfo.AppDirectory, "history.json");
    public static List<string[]> CsvItems { get; set; } = new List<string[]>();
    public static bool FirstRun { get; set; } = true;

    public MainWindow()
    {
        InitializeSettings();

        InitializeComponent();

        ReadSettings();

        CheckInput();

        CheckJson();
    }

    #region Check & read the CSV file
    public void CheckInput()
    {
        if (CheckCSV())
        {
            ReadCSV();

            if (InputCsv != null && CsvItems.Count > 0)
            {
                txt1.Text = $"CSV format history was found. File contains {CsvItems.Count} history records.";
                BtnConvert.IsEnabled = true;
            }
            else
            {
                txt1.Text = "CSV format history was found but appears to be empty.";
                txt2.Text = $"There was no history to convert. Feel free to delete {InputCsv}";
                _log.Warn("CSV format history was found but appears to be empty.");
            }
        }
        else
        {
            txt1.Text = "CSV format history file was not found.";
            txt2.Text = "Feel free to close this and begin using TimVer.";
            _log.Warn("CSV format history file was not found.");
        }
    }
    #endregion Check & read the CSV file

    #region Verify CSV file exists
    public static bool CheckCSV()
    {
        return File.Exists(InputCsv);
    }
    #endregion Verify CSV file exists

    #region Check to see if a JSON history file exists
    public void CheckJson()
    {
        if (!File.Exists(OutputJson))
        {
            return;
        }
        if (CheckCSV())
        {
            txt2.Text = "A JSON format history file exists. Clicking the Convert button will overwrite this file.";
        }
        else
        {
            txt2.Text = "A JSON format history file exists.";
        }
    }
    #endregion Check to see if a JSON history file exists

    #region Read the CSV history file
    public static void ReadCSV()
    {
        CsvItems.Clear();
        foreach (string line in File.ReadAllLines(InputCsv))
        {
            CsvItems.Add(line.Split(',', StringSplitOptions.None));
        }
        _log.Info($"Read {CsvItems.Count} records from {InputCsv}");
    }
    #endregion Read the CSV history file

    #region Convert the CSV format file to JSON format
    public void ConvertToJson()
    {
        FirstRun = false;
        List<History> histList = new();
        histList.Clear();
        try
        {
            if (ReadCSV != null)
            {
                foreach (string[] item in CsvItems)
                {
                    History history = new()
                    {
                        HDate = item[0],
                        HBuild = item[1],
                        HVersion = item[2],
                        HBranch = item[3]
                    };

                    histList.Add(history);
                }
                JsonSerializerOptions opts = new() { WriteIndented = true };
                string json = JsonSerializer.Serialize(histList, opts);
                File.WriteAllText(OutputJson, json);
                txt2.Text = $"Conversion is complete. {histList.Count} history records were written.";
                txt3.Text = $"Verify history in TimVer then feel free to delete {InputCsv}.";
                _log.Info($"{histList.Count} items written to {OutputJson}");
            }
        }
        catch (Exception ex)
        {
            txt2.Text = $"Sorry, there was an error converting or writing history file {OutputJson}.";
            txt3.Text = $"Perhaps the file was corrupt. See log file {NLHelpers.GetLogfileName()} for more information.";
            _log.Error(ex, $"Error converting or writing history file {OutputJson}");
        }
    }
    #endregion Convert the CSV format file to JSON format

    #region Convert button click event
    private void BtnConvert_Click(object sender, RoutedEventArgs e)
    {
        if (!FirstRun)
        {
            txt1.Text = string.Empty;
            CheckInput();
        }
        txt2.Text = string.Empty;
        txt3.Text = string.Empty;
        ConvertToJson();
    }
    #endregion Convert button click event

    #region Settings
    /// <summary>
    /// Read and apply settings
    /// </summary>
    private static void InitializeSettings()
    {
        UserSettings.Init();
    }

    public void ReadSettings()
    {
        // Set NLog configuration
        NLHelpers.NLogConfig(false);

        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Put version number in window title
        WindowTitleVersionAdmin();

        // Log the version, build date and commit id
        _log.Info($"{AppInfo.AppName} ({AppInfo.AppProduct}) {AppInfo.AppVersion} is starting up");
        _log.Info($"{AppInfo.AppName} {AppInfo.AppCopyright}");

        // Log the .NET version, app framework and OS platform
        string version = Environment.Version.ToString();
        _log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}  ({version})");
        _log.Debug(AppInfo.OsPlatform);

        // Settings change event
        UserSettings.Setting.PropertyChanged += UserSettingChanged;

        MainWindowUIHelpers.SetBaseTheme((bool)UserSettings.Setting.DarkMode);
    }
    #endregion Settings

    #region Setting change
    /// <summary>
    /// My way of handling changes in UserSettings
    /// </summary>
    /// <param name="sender"></param>
    private void UserSettingChanged(object sender, PropertyChangedEventArgs e)
    {
        PropertyInfo prop = sender.GetType().GetProperty(e.PropertyName);
        object newValue = prop?.GetValue(sender, null);
        _log.Debug($"Setting change: {e.PropertyName} New Value: {newValue}");
        switch (e.PropertyName)
        {
            case nameof(UserSettings.Setting.DarkMode):
                MainWindowUIHelpers.SetBaseTheme((bool)newValue);
                break;
        }
    }
    #endregion Setting change

    #region Window Events
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        // Stop the stopwatch and record elapsed time
        _log.Info($"{AppInfo.AppName} is shutting down.");

        // Shut down NLog
        LogManager.Shutdown();
    }
    #endregion Window Events

    #region Window Title
    /// <summary>
    /// Puts the version number in the title bar as well as Administrator if running elevated
    /// </summary>
    public void WindowTitleVersionAdmin()
    {
        // Set the windows title
        if (IsAdministrator())
        {
            Title = "TimVer History Conversion - (Administrator)";
        }
        else
        {
            Title = "TimVer History Conversion";
        }
    }
    #endregion Window Title

    #region Running as Administrator?
    /// <summary>
    /// Determines if running as administrator (elevated)
    /// </summary>
    /// <returns>True if running elevated</returns>
    public static bool IsAdministrator()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }
    #endregion Running as Administrator?

    #region Unhandled Exception Handler
    /// <summary>
    /// Handles any exceptions that weren't caught by a try-catch statement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        _log.Error("Unhandled Exception");
        Exception e = (Exception)args.ExceptionObject;
        _log.Error(e.Message);
        if (e.InnerException != null)
        {
            _log.Error(e.InnerException.ToString());
        }
        _log.Error(e.StackTrace);

        _ = MessageBox.Show("An error has occurred. See the log file",
            "ERROR",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
    #endregion Unhandled Exception Handler
}
