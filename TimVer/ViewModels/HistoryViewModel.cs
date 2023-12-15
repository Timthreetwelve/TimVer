// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    #region Read history file
    /// <summary>
    /// Reads the history file.
    /// </summary>
    public static void ReadHistory()
    {
        try
        {
            string json = File.ReadAllText(DefaultHistoryFile());
            History.HistoryList = JsonSerializer.Deserialize<List<History>>(json);
            int count = History.HistoryList.Count;
            string entry = string.Empty;
            entry = count == 1 ? "entry" : "entries";
            _log.Debug($"History file has {count} {entry}");
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Cannot read the history file.");
            // This needs to stay a message box since it can occur before the window is loaded
            _ = MessageBox.Show($"Cannot read the history file. It may be corrupt.\n\nDelete {DefaultHistoryFile()} and retry.",
                                "TimVer is Unable to Continue",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            Environment.Exit(1);
        }
    }
    #endregion Read history file

    #region Write the history file
    /// <summary>
    /// Writes the history file (json) if needed.
    /// </summary>
    public static void WriteHistory()
    {
        History newHist = new()
        {
            HBuild = CombinedInfo.Build,
            HVersion = CombinedInfo.Version,
            HBranch = CombinedInfo.BuildBranch,
            HDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm")
        };

        if (File.Exists(DefaultHistoryFile()))
        {
            ReadHistory();
            // Add to history file if build doesn't exist
            if (!History.HistoryList.Exists(x => x.HBuild == newHist.HBuild))
            {
                History.HistoryList.Add(newHist);
                History.HistoryList = History.HistoryList.OrderByDescending(o => o.HDate).ToList();
                JsonSerializerOptions opts = new() { WriteIndented = true };
                string json = JsonSerializer.Serialize(History.HistoryList, opts);
                File.WriteAllText(DefaultHistoryFile(), json);
                _log.Info($"History file was updated with {newHist.HBuild}");
            }
            else
            {
                _log.Info("History file is up to date.");
            }
        }
        else
        {
            History.HistoryList.Add(newHist);
            JsonSerializerOptions opts = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(History.HistoryList, opts);
            File.WriteAllText(DefaultHistoryFile(), json);
            _log.Info($"History file was created with {newHist.HBuild}");
        }
    }
    #endregion Write the history file

    #region Get path to history file
    /// <summary>
    /// Gets the history file.
    /// </summary>
    /// <returns></returns>
    private static string DefaultHistoryFile()
    {
        return Path.Combine(AppInfo.AppDirectory, "history.json");
    }
    #endregion Get path to history file
}
