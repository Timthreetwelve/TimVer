// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Displays the history page
/// </summary>
public partial class Page3 : UserControl
{
    #region NLog Instance
    private static readonly Logger log = LogManager.GetCurrentClassLogger();
    #endregion NLog Instance

    public Page3()
    {
        InitializeComponent();

        HistoryGrid.ItemsSource = ReadHistory();
    }

    #region Read history file
    public static List<History> ReadHistory()
    {
        CsvConfiguration config = new(CultureInfo.InvariantCulture);
        config.HasHeaderRecord = false;
        config.Delimiter = ",";
        config.TrimOptions = TrimOptions.Trim;
        config.IgnoreBlankLines = true;

        try
        {
            using StreamReader reader = new(DefaultHistoryFile());
            using CsvReader csv = new(reader, config);
            IEnumerable<History> history = csv.GetRecords<History>();
            // Sort history file by date (descending)
            return history.OrderByDescending(o => o.HDate).ToList();
        }
        catch (Exception ex)
        {
            log.Error(ex, "Cannot read the history file.");
            // This needs to stay a message box since it can occur before the window is loaded
            _ = MessageBox.Show($"Cannot read the history file. It may be corrupt.\n\nDelete {DefaultHistoryFile()} and retry.",
                                "TimVer is Unable to Continue",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            Environment.Exit(1);
        }
        return null;
    }
    #endregion Read history file

    #region Write the history file
    public static void WriteHistory()
    {
        History newHist = new();
        log.Debug("Getting build info:");
        newHist.HBuild = CombinedInfo.Build;
        newHist.HVersion = CombinedInfo.Version;
        newHist.HBranch = CombinedInfo.BuildBranch;
        newHist.HDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

        CsvConfiguration config = new(CultureInfo.InvariantCulture);
        config.HasHeaderRecord = false;
        config.Delimiter = ",";
        config.TrimOptions = TrimOptions.Trim;

        if (File.Exists(DefaultHistoryFile()))
        {
            List<History> hist = ReadHistory();
            // Add to history file if build doesn't exist
            if (!hist.Exists(x => x.HBuild == newHist.HBuild))
            {
                hist.Add(newHist);
                hist = hist.OrderByDescending(o => o.HDate).ToList();
                using (StreamWriter writer = new(DefaultHistoryFile()))
                using (CsvWriter csv = new(writer, config))
                {
                    csv.WriteRecords(hist);
                }

                log.Info($"History file was updated with {newHist.HBuild}");
            }
            else
            {
                log.Info("History file is up to date.");
            }
        }
        else
        {
            using (StreamWriter writer = new(DefaultHistoryFile()))
            using (CsvWriter csv = new(writer, config))
            {
                csv.WriteRecord(newHist);
            }

            log.Info($"History file was created with {newHist.HBuild}");
        }
    }
    #endregion Write the history file

    #region Get path to history file
    private static string DefaultHistoryFile()
    {
        string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        return Path.Combine(dir, "history.csv");
    }
    #endregion Get path to history file
}
