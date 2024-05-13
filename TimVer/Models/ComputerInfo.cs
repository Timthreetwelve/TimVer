// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

internal partial class ComputerInfo : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private string? _biosManufacturer;

    [ObservableProperty]
    private string? _formattedBiosVersion;

    [ObservableProperty]
    private string? _formattedMemory;

    [ObservableProperty]
    private string? _formattedProcessorCores;

    [ObservableProperty]
    private string? _formattedUptime;

    [ObservableProperty]
    private string? _lastBoot;

    [ObservableProperty]
    private TimeSpan _uptime;

    [ObservableProperty]
    private string? _manufacturer;

    [ObservableProperty]
    private string? _model;

    [ObservableProperty]
    private string? _machineName;

    [ObservableProperty]
    private string? _processor;

    [ObservableProperty]
    private string? _processorArchitecture;

    [ObservableProperty]
    private string? _processorName;

    [ObservableProperty]
    private string? _processorDescription;

    [ObservableProperty]
    private string? _processorCores;

    [ObservableProperty]
    private string? _processorThreads;

    [ObservableProperty]
    private string? _uefiSecureBoot;

    [ObservableProperty]
    private string? _uefiMode;
    #endregion Properties
}
