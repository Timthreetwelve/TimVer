// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

public partial class LogicalDrives : ObservableObject
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _label;

    [ObservableProperty]
    private string? _driveType;

    [ObservableProperty]
    private string? _format;

    [ObservableProperty]
    private double? _totalSize;

    [ObservableProperty]
    private double? _gBFree;

    [ObservableProperty]
    private double? _percentFree;
}
