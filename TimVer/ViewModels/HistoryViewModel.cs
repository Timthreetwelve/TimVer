// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public class HistoryViewModel : ObservableObject
{
    #region constructor
    public HistoryViewModel()
    {
        if (HistoryList.Count < 1)
        {
            LoadData();
        }
    }
    #endregion constructor

    #region Collection of build history records
    public static List<History> HistoryList { get; set; } = [];
    #endregion Collection of build history records

    #region Load data
    /// <summary>
    /// Get data for all properties.
    /// </summary>
    public static void LoadData()
    {
        HistoryHelpers.WriteHistory();
    }
    #endregion Load data
}
