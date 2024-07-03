// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

public partial class LogicalDrives : ObservableObject
{
    /// <summary>
    /// The name of the drive. e.g. C:
    /// </summary>
    [ObservableProperty]
    private string? _name;

    /// <summary>
    /// The drive label.
    /// </summary>
    [ObservableProperty]
    private string? _label;

    /// <summary>
    /// Type of drive. Fixed, removable, network, etc.
    /// </summary>
    [ObservableProperty]
    private string? _driveType;

    /// <summary>
    /// The drive format. NTFS, FAT32, etc.
    /// </summary>
    [ObservableProperty]
    private string? _format;

    /// <summary>
    /// Total size of the drive.
    /// </summary>
    [ObservableProperty]
    private double? _totalSize;

    /// <summary>
    /// Available free space.
    /// </summary>
    [ObservableProperty]
    private double? _gBFree;

    /// <summary>
    /// Free space expressed as a percentage.
    /// </summary>
    [ObservableProperty]
    private double? _percentFree;
}
