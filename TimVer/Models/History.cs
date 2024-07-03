// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

/// <summary>
/// Build History
/// </summary>
public partial class History : ObservableObject
{
    #region Properties
    /// <summary>
    /// Date the record was added to the history file.
    /// </summary>
    [ObservableProperty]
    private string? _hDate;

    /// <summary>
    /// Windows build number.
    /// </summary>
    [ObservableProperty]
    private string? _hBuild;

    /// <summary>
    /// Windows version.
    /// </summary>
    [ObservableProperty]
    private string? _hVersion;

    /// <summary>
    /// Windows build branch.
    /// </summary>
    [ObservableProperty]
    private string? _hBranch;
    #endregion Properties
}
