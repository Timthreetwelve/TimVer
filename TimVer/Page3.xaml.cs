// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CsvHelper;
using CsvHelper.Configuration;
using NLog;
#endregion using directives

namespace TimVer;
public partial class Page3 : Page
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
            _ = MessageBox.Show($"Cannot read the history file. It may be corrupt.\n\nDelete {DefaultHistoryFile()} and retry.",
                                "TimVer is Unable to Continue",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            log.Error(ex, "Cannot read the history file.");
            Environment.Exit(1);
        }
        return null;
    }
    #endregion Read history file

    #region Write the history file
    public static void WriteHistory()
    {
        //CombinedInfo vm = new();
        History newHist = new();
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

            log.Info($"History file was updated with {newHist.HBuild}");
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
