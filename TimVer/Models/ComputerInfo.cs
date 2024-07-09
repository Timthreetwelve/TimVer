// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

internal partial class ComputerInfo : ObservableObject
{
    #region Properties
    /// <summary>
    /// Manufacturer of the BIOS. From WMI.
    /// </summary>
    [ObservableProperty]
    private string? _biosManufacturer;

    /// <summary>
    /// BIOS version and date. From WMI. Formatted for display.
    /// </summary>
    [ObservableProperty]
    private string? _formattedBiosVersion;

    /// <summary>
    /// The total amount of installed memory from WMI and the total amount of usable memory from GC.
    /// </summary>
    [ObservableProperty]
    private string? _formattedMemory;

    /// <summary>
    /// The number of CPU cores and the number of logical processors. Both from WMI.
    /// </summary>
    [ObservableProperty]
    private string? _formattedProcessorCores;

    /// <summary>
    /// The amount of time since the last boot. From Environment.TickCount64.
    /// </summary>
    [ObservableProperty]
    private string? _formattedUptime;

    /// <summary>
    /// Date and time of the last boot. From Environment.TickCount64.
    /// </summary>
    [ObservableProperty]
    private string? _lastBoot;

    /// <summary>
    /// Last boot type. Normal, Safe, Safe w/Networking. From GetSystemMetrics.
    /// </summary>
    [ObservableProperty]
    private string? _lastBootType;

    /// <summary>
    /// The amount of time since the last boot. From Environment.TickCount64.
    /// </summary>
    [ObservableProperty]
    private TimeSpan _uptime;

    /// <summary>
    /// Manufacturer of the computer system. From WMI.
    /// </summary>
    [ObservableProperty]
    private string? _manufacturer;

    /// <summary>
    /// Model of the computer system.
    /// </summary>
    [ObservableProperty]
    private string? _model;

    /// <summary>
    /// Name given to the computer. From the Environment.
    /// </summary>
    [ObservableProperty]
    private string? _machineName;

    /// <summary>
    /// Processor architecture. From WMI.
    /// </summary>
    [ObservableProperty]
    private string? _processorArchitecture;

    /// <summary>
    /// Processor name. From WMI.
    /// </summary>
    [ObservableProperty]
    private string? _processorName;

    /// <summary>
    /// Processor description. From WMI.
    /// </summary>
    [ObservableProperty]
    private string? _processorDescription;

    /// <summary>
    /// The state of UEFI Secure Boot. From the Registry.
    /// </summary>
    [ObservableProperty]
    private string? _uefiSecureBoot;

    /// <summary>
    /// The UEFI mode; either UEFI or BIOS. From PInvoke.Kernel32.
    /// </summary>
    [ObservableProperty]
    private string? _uefiMode;
    #endregion Properties
}
