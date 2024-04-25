// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

public partial class PhysicalDrives : ObservableObject
{
    [ObservableProperty]
    private string _busType;

    [ObservableProperty]
    private string _diskType;

    [ObservableProperty]
    private string _firmwareRevision;

    [ObservableProperty]
    private string _friendlyName;

    [ObservableProperty]
    private string _health;

    [ObservableProperty]
    private uint _index;

    [ObservableProperty]
    private string _interface;

    [ObservableProperty]
    private string _isBoot;

    [ObservableProperty]
    private string _isSystem;

    [ObservableProperty]
    private string _location;

    [ObservableProperty]
    private string _mediaType;

    [ObservableProperty]
    private string _message;

    [ObservableProperty]
    private string _model;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private uint _partitions;

    [ObservableProperty]
    private string _partitionStyle;

    [ObservableProperty]
    private string _pNPDeviceID;

    [ObservableProperty]
    private string _serialNumber;

    [ObservableProperty]
    private double _size;

    [ObservableProperty]
    private string _status;
}
