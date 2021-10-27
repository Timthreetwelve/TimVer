// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using TinyCsvParser;

namespace TimVer.ViewModels
{
    internal class HistoryViewModel
    {
        public string HDate { get; set; }

        public string HBuild { get; set; }

        public string HBranch { get; set; }

        public string HVersion { get; set; }

        public static List<HistoryViewModel> ReadHistory()
        {
            CsvParserOptions opts = new CsvParserOptions(false, ',');
            CsvParser<HistoryViewModel> csvParser = new CsvParser<HistoryViewModel>(opts,
                new CsvHistoryMapping());
            try
            {
                ParallelQuery<TinyCsvParser.Mapping.CsvMappingResult<HistoryViewModel>> records =
            csvParser.ReadFromFile(DefaultHistoryFile(), Encoding.UTF8);
                return records.Select(x => x.Result)
                    .OrderByDescending(o => o.HDate).ToList();
            }
            catch (Exception)
            {
                _ = MessageBox.Show($"Cannot read the history file. It may be corrupt.\n\nDelete {DefaultHistoryFile()} and retry.", "TimVer Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
            return null;
        }

        public static void WriteHistory()
        {
            Page1ViewModel p1 = new Page1ViewModel();
            string build = p1.Build;
            string version = p1.Version;
            string branch = p1.BuildBranch;
            string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            string historyCSV = string.Format($"{now}, {build}, {version}, {branch}\r\n");

            if (File.Exists(DefaultHistoryFile()))
            {
                List<HistoryViewModel> hist = ReadHistory();
                if (!hist.Exists(x => x.HBuild == build))
                {
                    File.AppendAllText(DefaultHistoryFile(), historyCSV);
                }
            }
            else
            {
                File.AppendAllText(DefaultHistoryFile(), historyCSV);
            }
        }

        private static string DefaultHistoryFile()
        {
            string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return Path.Combine(dir, "history.csv");
        }
    }
}
