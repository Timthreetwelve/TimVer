// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

/// <summary>
/// Build History
/// </summary>
public partial class History : ObservableObject
{
    [ObservableProperty]
    private string _hDate;

    [ObservableProperty]
    private string _hBuild;

    [ObservableProperty]
    private string _hVersion;

    [ObservableProperty]
    private string _hBranch;

    public static List<History> HistoryList { get; set; } = new();
}
