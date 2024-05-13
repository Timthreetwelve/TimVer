// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class HistoryHelpers
{
    #region JSON options
    public static readonly JsonSerializerOptions s_options = new()
    {
        WriteIndented = true
    };
    #endregion JSON options

    #region Read the history file
    /// <summary>
    /// Reads the history file.
    /// </summary>
    public static void ReadHistory()
    {
        try
        {
            string json = File.ReadAllText(DefaultHistoryFile());
            HistoryViewModel.HistoryList = JsonSerializer.Deserialize<List<History>>(json)!;
            int count = HistoryViewModel.HistoryList!.Count;
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
    #endregion Read the history file

    #region Write the history file
    /// <summary>
    /// Writes the history file (json) if needed.
    /// </summary>
    public static void WriteHistory()
    {
        History newHist = new()
        {
            HBuild = WindowsInfoHelpers.GetBuild(),
            HVersion = WindowsInfoHelpers.GetVersion(),
            HBranch = RegistryHelpers.GetRegistryInfo("BuildBranch"),
            HDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm")
        };

        if (File.Exists(DefaultHistoryFile()))
        {
            ReadHistory();
            // Add to history file if build doesn't exist
            if (!HistoryViewModel.HistoryList.Exists(x => x.HBuild == newHist.HBuild))
            {
                HistoryViewModel.HistoryList.Add(newHist);
                HistoryViewModel.HistoryList = [.. HistoryViewModel.HistoryList.OrderByDescending(o => o.HDate)];
                string json = JsonSerializer.Serialize(HistoryViewModel.HistoryList, s_options);
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
            HistoryViewModel.HistoryList.Add(newHist);
            string json = JsonSerializer.Serialize(HistoryViewModel.HistoryList, s_options);
            File.WriteAllText(DefaultHistoryFile(), json);
            _log.Info($"History file was created with build {newHist.HBuild}");
        }
    }
    #endregion Write the history file

    #region History file filename
    /// <summary>
    /// Gets the history file.
    /// </summary>
    /// <returns>Path to history file as string.</returns>
    private static string DefaultHistoryFile()
    {
        return Path.Combine(AppInfo.AppDirectory, "history.json");
    }
    #endregion History file filename
}
