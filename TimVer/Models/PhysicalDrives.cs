// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

public partial class PhysicalDrives : ObservableObject
{
    /// <summary>
    /// Physical disk bus type. RAID, SATA, MMC, SCSI, USB, etc.
    /// </summary>
    [ObservableProperty]
    private string? _busType;

    /// <summary>
    /// Disk type. HDD, SSD, etc.
    /// </summary>
    [ObservableProperty]
    private string? _diskType;

    /// <summary>
    /// Firmware revision number.
    /// </summary>
    [ObservableProperty]
    private string? _firmwareRevision;

    /// <summary>
    /// Drive "friendly name".
    /// </summary>
    [ObservableProperty]
    private string? _friendlyName;

    /// <summary>
    /// Drive health status. Healthy, Warning, Unhealthy, etc.
    /// </summary>
    [ObservableProperty]
    private string? _health;

    /// <summary>
    /// The drive Index from WMI..
    /// </summary>
    [ObservableProperty]
    private uint _index;

    /// <summary>
    /// The drive interface. SCSI, USB, etc.
    /// </summary>
    [ObservableProperty]
    private string? _interface;

    /// <summary>
    /// Is the drive the Boot drive? String True or False.
    /// </summary>
    [ObservableProperty]
    private string? _isBoot;

    /// <summary>
    /// Is the drive the System drive? String True or False.
    /// </summary>
    [ObservableProperty]
    private string? _isSystem;

    /// <summary>
    /// Is this used?
    /// </summary>
    [ObservableProperty]
    private string? _location;

    /// <summary>
    /// Media type. Fixed hard disk, External hard disk, etc.
    /// </summary>
    [ObservableProperty]
    private string? _mediaType;

    /// <summary>
    /// Text indicating that the collection of physical disk information has been disabled.
    /// </summary>
    [ObservableProperty]
    private string? _message;

    /// <summary>
    /// Manufacturers model name.
    /// </summary>
    [ObservableProperty]
    private string? _model;

    /// <summary>
    /// Windows drive name. In the format of \\.\PHYSICALDRIVE0
    /// </summary>
    [ObservableProperty]
    private string? _name;

    /// <summary>
    /// Number of partitions.
    /// </summary>
    [ObservableProperty]
    private uint _partitions;

    /// <summary>
    /// Partition style. MBR or GPT.
    /// </summary>
    [ObservableProperty]
    private string? _partitionStyle;

    /// <summary>
    /// PNP device id. Currently not used.
    /// </summary>
    [ObservableProperty]
    private string? _pNPDeviceID;

    /// <summary>
    ///  Drive serial number.
    /// </summary>
    [ObservableProperty]
    private string? _serialNumber;

    /// <summary>
    /// Size of the drive.
    /// </summary>
    [ObservableProperty]
    private double _size;

    /// <summary>
    /// Drive status. OK, etc. Currently not used.
    /// </summary>
    [ObservableProperty]
    private string? _status;
}
