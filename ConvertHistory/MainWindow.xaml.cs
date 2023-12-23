// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System.Windows.Media;

namespace ConvertHistory;

public partial class MainWindow : Window
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

    public static string InputCsv { get; set; }
    public static string OutputJson { get; set; }
    public static List<string[]> CsvItems { get; set; } = new List<string[]>();

    public MainWindow()
    {
        InitializeSettings();

        InitializeComponent();

        ReadSettings();
    }

    #region Check & read the CSV file
    public bool CheckInput()
    {
        if (File.Exists(InputCsv))
        {
            ReadCSV();

            if (InputCsv != null && CsvItems.Count > 0)
            {
                _log.Info($"CSV format history was found. File contains {CsvItems.Count} history records.");
                return true;
            }
            else
            {
                txt1.Text = $"Input file {InputCsv} is not in the correct format.";
                txt1.Foreground = Brushes.Crimson;
                _log.Warn("Input file is not in the correct format.");
                return false;
            }
        }
        else
        {
            txt1.Text = "CSV format history file was not found.";
            txt1.Foreground = Brushes.Crimson;
            _log.Warn("CSV format history file was not found.");
            return false;
        }
    }
    #endregion Check & read the CSV file

    #region Read the CSV history file
    public static void ReadCSV()
    {
        CsvItems.Clear();
        foreach (string line in File.ReadAllLines(InputCsv))
        {
            int count = line.Count(f => f == ',');
            if (count == 3)
            {
                CsvItems.Add(line.Split(',', StringSplitOptions.None));
            }
        }
        _log.Info($"Read {CsvItems.Count} records from {InputCsv}");
    }
    #endregion Read the CSV history file

    #region Convert the CSV format file to JSON format
    public void ConvertToJson()
    {
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
                txt1.Text = $"Conversion is complete. {histList.Count} history records were written.";
                txt2.Text = $"Verify history in TimVer then feel free to delete {InputCsv}.";
                _log.Info($"{histList.Count} items written to {OutputJson}");
            }
        }
        catch (Exception ex)
        {
            txt1.Text = $"Sorry, there was an error converting or writing history file {OutputJson}.";
            txt1.Foreground = Brushes.Crimson;
            txt2.Text = $"See log file {NLHelpers.GetLogfileName()} for more information.";
            _log.Error(ex, $"Error converting or writing history file {OutputJson}");
        }
    }
    #endregion Convert the CSV format file to JSON format

    #region button click events
    private void BtnConvert_Click(object sender, RoutedEventArgs e)
    {
        txt1.Text = string.Empty;
        txt1.Foreground = Brushes.Black;
        txt2.Text = string.Empty;
        txt2.Foreground = Brushes.Black;

        if (CheckInput() && (!string.IsNullOrEmpty(OutputJson)))
        {
            ConvertToJson();
        }
    }

    private void Button_Input_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dlgOpen = new()
        {
            Title = "Browse for Input File",
            CheckFileExists = true,
            CheckPathExists = true,
            Filter = "CSV files (*.csv)|*.csv"
        };
        if (dlgOpen.ShowDialog() == true)
        {
            TbxInput.Text = dlgOpen.FileName;
        }
    }

    private void Button_Output_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog save = new()
        {
            Title = "Select Output File",
            Filter = "JSON File|*.json",
            FileName = "History.json",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
        };
        bool? result = save.ShowDialog();
        if (result == true)
        {
            TbxOutput.Text = save.FileName;
        }
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
        _log.Info($"{AppInfo.AppName} (TimVer History Conversion) {AppInfo.AppVersion} is starting up");
        _log.Info($"{AppInfo.AppName} {AppInfo.AppCopyright}");

        // Log the .NET version, app framework and OS platform
        string version = Environment.Version.ToString();
        _log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}  ({version})");
        _log.Debug(AppInfo.OsPlatform);
    }
    #endregion Settings

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

    private void TbxInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        InputCsv = TbxInput.Text.Trim('"');
    }

    private void TbxOutput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        OutputJson = TbxOutput.Text.Trim('"');
    }
}
