// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class for gathering information about physical and logical disk drives.
/// </summary>
public static class DiskDriveHelpers
{
    #region Determine if drive is fixed
    /// <summary>
    /// Determines whether the specified drive is Fixed.
    /// </summary>
    /// <param name="drive">The drive to check.</param>
    /// <returns>
    ///   <c>true</c> if specified drive is Fixed; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDriveFixed(char drive)
    {
        if (drive is '\0')
        {
            return false;
        }
        DriveInfo[] driveArray = DriveInfo.GetDrives();
        for (int i = 0; i < driveArray.Length; i++)
        {
            DriveInfo driveInfo = driveArray[i];
            if (driveInfo.Name.StartsWith(drive.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return driveInfo.DriveType == DriveType.Fixed;
            }
        }
        return false;
    }
    #endregion Determine if drive is fixed

    #region Get array of logical drives
    private static DriveInfo[] GetLogicalDrives()
    {
        try
        {
            return DriveInfo.GetDrives();
        }
        catch (IOException ex)
        {
            _log.Error(ex, "I/O error");
            string msg = GetStringResource("DriveInfo_IOError");
            _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            _ = MessageBox.Show($"{msg}\n{ex.Message}",
                                GetStringResource("DriveInfo_Error"),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error)));
        }
        catch (UnauthorizedAccessException ex)
        {
            _log.Error(ex, "Unauthorized Access");
            string msg = GetStringResource("DriveInfo_UnauthorizedAccess");
            _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            _ = MessageBox.Show($"{msg}\n{ex.Message}",
                                GetStringResource("DriveInfo_Error"),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error)));
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Unknown error");
            string msg = GetStringResource("DriveInfo_UnknownError");
            _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            _ = MessageBox.Show($"{msg}\n{ex.Message}",
                                GetStringResource("DriveInfo_Error"),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error)));
        }
        return [];
    }
    #endregion Get array of logical drives

    #region Get logical drive information
    public static List<LogicalDrives> GetLogicalDriveInfo()
    {
        List<LogicalDrives> logicalDrives = [];
        if (GetLogicalDrives() != null)
        {
            MainWindowHelpers.MainWindowWaitPointer();
            Stopwatch watch = Stopwatch.StartNew();
            foreach (DriveInfo drive in GetLogicalDrives())
            {
                try
                {
                    if (GetDriveDetails(drive) != null)
                    {
                        logicalDrives.Add(GetDriveDetails(drive));
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex, $"Error processing drive {drive}");
                    string errorMsg = GetStringResource("DriveInfo_ErrorProcessingDrive");
                    _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    _ = MessageBox.Show($"{errorMsg} {drive}\n{ex.Message}",
                        GetStringResource("DriveInfo_Error"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error)));
                    LogicalDrives error = new()
                    {
                        Name = drive.Name,
                        Label = GetStringResource("DriveInfo_ErrorSeeLog")
                    };
                    logicalDrives.Add(error);
                }
            }
            watch.Stop();
            MainWindowHelpers.MainWindowNormalPointer();
            string suffix = (logicalDrives.Count == 1) ? string.Empty : "s";
            _log.Debug($"Found {logicalDrives.Count} logical drive{suffix} in {watch.Elapsed.TotalMilliseconds:N2} ms");
            return logicalDrives;
        }
        else
        {
            LogicalDrives error = new()
            {
                Name = "n/a",
                Label = GetStringResource("DriveInfo_ErrorSeeLog")
            };
            logicalDrives.Add(error);
            return logicalDrives;
        }
    }
    #endregion Get logical drive information

    #region Get details for an individual drive
    private static LogicalDrives GetDriveDetails(DriveInfo d)
    {
        // If the drive is network check settings to see if it needs to be included
        if (string.Equals(d.DriveType.ToString(), "network", StringComparison.OrdinalIgnoreCase)
            && !UserSettings.Setting!.IncludeNetwork)
        {
            return null!;
        }
        // If the drive is removable check settings to see if it needs to be included
        if (string.Equals(d.DriveType.ToString(), "removable", StringComparison.OrdinalIgnoreCase)
            && !UserSettings.Setting!.IncludeRemovable)
        {
            return null!;
        }
        // If the drive is ready
        if (d.IsReady)
        {
            int gbPref = UserSettings.Setting!.Use1024 ? 1024 : 1000;

            // Add the information for each drive to the list
            return new()
            {
                Name = d.Name,
                DriveType = d.DriveType switch
                {
                    DriveType.Fixed => GetStringResource("DriveInfo_DriveType_Fixed"),
                    DriveType.Network => GetStringResource("DriveInfo_DriveType_Network"),
                    DriveType.Removable => GetStringResource("DriveInfo_DriveType_Removable"),
                    DriveType.CDRom => GetStringResource("DriveInfo_DriveType_CDRom"),
                    DriveType.Ram => GetStringResource("DriveInfo_DriveType_Ram"),
                    DriveType.NoRootDirectory => GetStringResource("DriveInfo_DriveType_NoRootDirectory"),
                    _ => GetStringResource("DriveInfo_DriveType_Unknown")
                },
                Format = d.DriveFormat,
                Label = d.VolumeLabel,
                TotalSize = Math.Round(d.TotalSize / Math.Pow(gbPref, 3), 2),
                GBFree = Math.Round(d.AvailableFreeSpace / Math.Pow(gbPref, 3), 2),
                PercentFree = (double)d.AvailableFreeSpace / d.TotalSize
            };
        }
        // If the drive is not ready
        else if (UserSettings.Setting!.IncludeNotReady)
        {
            return new()
            {
                Name = d.Name,
                DriveType = d.DriveType.ToString(),
                Label = GetStringResource("DriveInfo_NotReady")
            };
        }
        return null!;
    }
    #endregion Get details for an individual drive

    #region Assemble the physical disk information
    /// <summary>
    /// Gathers the physical disk information.
    /// </summary>
    /// <returns>List of properties and values of type PhysicalDrives</returns>
    public static List<PhysicalDrives> GetPhysicalDriveInfo()
    {
        if (UserSettings.Setting!.GetPhysicalDrives)
        {
            MainWindowHelpers.MainWindowWaitPointer();
            Stopwatch pdWatch = Stopwatch.StartNew();

            // List of physical drive properties
            List<PhysicalDrives> physicalDrives = [];

            foreach (uint index in GetDriveIndicies())
            {
                PhysicalDrives pDisk = new();
                Dictionary<string, string> results = [];

                results = results.Concat(GetMsftDisk(index))
                    .ToDictionary(x => x.Key, x => x.Value)
                    .Concat(GetMsftPhysicalDisk(index))
                    .ToDictionary(x => x.Key, x => x.Value)
                    .Concat(GetWin32DiskDrive(index))
                    .ToDictionary(x => x.Key, x => x.Value);

                pDisk.BusType = results["BusType"];
                pDisk.DiskType = results["DiskType"];
                pDisk.DriveLetters = results["Letters"];
                pDisk.FriendlyName = results["FriendlyName"];
                pDisk.Health = results["HealthStatus"];
                pDisk.Index = index;
                pDisk.Interface = results["Interface"];
                pDisk.IsBoot = results["IsBoot"];
                pDisk.IsSystem = results["IsSystem"];
                pDisk.MediaType = results["MediaType"];
                pDisk.Model = results["Model"];
                pDisk.Name = results["Name"];
                pDisk.Partitions = Convert.ToUInt16(results["Partitions"], CultureInfo.InvariantCulture);
                pDisk.PartitionStyle = results["PartitionStyle"];
                pDisk.SerialNumber = results["SerialNumber"];
                pDisk.Size = Convert.ToDouble(results["Size"], CultureInfo.InvariantCulture);
                physicalDrives.Add(pDisk);
            }

            pdWatch.Stop();
            string suffix = (physicalDrives.Count == 1) ? string.Empty : "s";
            _log.Debug($"Found {physicalDrives.Count} physical drive{suffix} in {pdWatch.Elapsed.TotalMilliseconds:N2} ms");
            MainWindowHelpers.MainWindowNormalPointer();
            return physicalDrives;
        }

        // Gathering physical drive info is disabled
        PhysicalDrives drives = new()
        {
            Message = GetStringResource("DriveInfo_PhysicalDisabled")
        };
        return [drives];
    }
    #endregion Assemble the physical disk information

    #region Get list of drive indexes
    /// <summary>
    /// Get list of drive indexes.
    /// </summary>
    /// <remarks>This is needed because we can't depend on drives being consecutive or ordered.</remarks>
    /// <returns>List of uint.</returns>
    private static List<uint> GetDriveIndicies()
    {
        const string scope = @"\\.\root\CIMV2";
        const string query = "SELECT Index FROM Win32_DiskDrive";
        using CimSession cimSession = CimSession.Create(null);
        List<uint> indexes = [];
        indexes.AddRange(cimSession.QueryInstances(scope, "WQL", query)
            .Select(drive => (uint)drive.CimInstanceProperties["Index"]?.Value!)
            .Order());
        return indexes;
    }
    #endregion Get list of drive indexes

    #region Get info from Win32_DiskDrive
    /// <summary>
    /// Gets Win32_DiskDrive info for the specified device.
    /// </summary>
    /// <param name="index">Index number of the disk.</param>
    /// <returns>Dictionary of values.</returns>
    private static Dictionary<string, string> GetWin32DiskDrive(uint index)
    {
        const string scope = @"\\.\root\CIMV2";
        string query = $"SELECT InterfaceType, MediaType, Model, Name, Status, Partitions, Size, DeviceID FROM Win32_DiskDrive WHERE Index = {index}";

        try
        {
            using CimSession cimSession = CimSession.Create(null);
            int gbPref = UserSettings.Setting!.Use1024 ? 1024 : 1000;
            IEnumerable<CimInstance> diskInfo = cimSession.QueryInstances(scope, "WQL", query);
            return diskInfo.Select(drive => new Dictionary<string, string>
            {
                ["Interface"] = CimStringProperty(drive, "InterfaceType"),
                ["MediaType"] = CimStringProperty(drive, "MediaType")
                                   .Replace("media", "", StringComparison.OrdinalIgnoreCase),
                ["Model"] = CimStringProperty(drive, "Model"),
                ["Name"] = CimStringProperty(drive, "Name"),
                ["Partitions"] = CimStringProperty(drive, "Partitions"),
                ["Status"] = CimStringProperty(drive, "Status"),
                ["Size"] = Math.Round(Convert.ToDouble(drive.CimInstanceProperties["Size"]
                              .Value, CultureInfo.InvariantCulture) / Math.Pow(gbPref, 3), 2)
                              .ToString(CultureInfo.InvariantCulture),
                ["Letters"] = GetDriveLetters(CimStringProperty(drive,"DeviceID"))
            }).FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error retrieving MSFT_Disk data for device {index}");
            return new()
            {
                { "Interface", ""},
                { "MediaType", ""},
                { "Model", ""},
                { "Name", ""},
                { "Partitions", ""},
                { "Status", ""},
                { "Size", ""},
                { "Letters", ""}
            };
        }
    }
    #endregion Get info from Win32_DiskDrive

    /// <summary>
    /// Gets the drive letters on the supplied device ID.
    /// </summary>
    /// <param name="deviceID">Device ID from Win32_DiskDrive. e.g. \\.\PHYSICALDISK0</param>
    /// <returns>String of drive letters.</returns>
    static string GetDriveLetters(string deviceID)
    {
        const string scope = @"\\.\root\CIMV2";
        StringBuilder sb = new();

        CimSession cim = CimSession.Create(null);
        string query = $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{deviceID}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
        foreach (CimInstance partitions in cim.QueryInstances(scope, "WQL", query))
        {
            string? partition = partitions.CimInstanceProperties["DeviceID"].Value.ToString();
            query = $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition}'}} WHERE AssocClass = Win32_LogicalDiskToPartition";
            foreach (CimInstance drive in cim.QueryInstances(scope, "WQL", query))
            {
                string? name = drive.CimInstanceProperties["Name"].Value.ToString();
                _ = sb.Append(CultureInfo.InvariantCulture, $"{name}  ");
            }
        }
        return sb.ToString().TrimEnd();
    }

    #region Get info from MSFT_Disk
    /// <summary>
    /// Gets MSFT_Disk info for the specified device.
    /// </summary>
    /// <param name="number">The number of the disk.</param>
    /// <returns>Dictionary of values.</returns>
    private static Dictionary<string, string> GetMsftDisk(uint number)
    {
        const string scope = @"\\.\root\Microsoft\Windows\Storage";
        string query = $"SELECT IsBoot, IsSystem, PartitionStyle, Model FROM MSFT_Disk WHERE Number = {number}";

        try
        {
            using CimSession cim = CimSession.Create(null);
            IEnumerable<CimInstance> diskInfo = cim.QueryInstances(scope, "WQL", query);
            return diskInfo
                .Select(drive => new Dictionary<string, string>
                {
                    ["IsBoot"] = CimStringProperty(drive, "IsBoot"),
                    ["IsSystem"] = CimStringProperty(drive, "IsSystem"),
                    ["PartitionStyle"] = CimStringProperty(drive, "PartitionStyle") switch
                    {
                        "1" => "MBR",
                        "2" => "GPT",
                        _ => GetStringResource("DriveInfo_DriveType_Unknown")
                    }
                })
                .FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error retrieving MSFT_Disk data for device {number}");
            return new()
            {
                { "IsBoot", ""},
                { "IsSystem", ""},
                { "PartitionStyle", ""}
            };
        }
    }
    #endregion Get info from MSFT_Disk

    #region Get info from MSFT_PhysicalDisk
    /// <summary>
    /// Gets MSFT_PhysicalDisk info for the specified device.
    /// </summary>
    /// <param name="device">The device ID of the disk.</param>
    /// <returns>Dictionary of values.</returns>
    private static Dictionary<string, string> GetMsftPhysicalDisk(uint device)
    {
        const string scope = @"\\.\root\Microsoft\Windows\Storage";
        string query = $"SELECT MediaType, HealthStatus, BusType, SerialNumber, FriendlyName FROM MSFT_PhysicalDisk WHERE DeviceID = {device}";
        try
        {
            using CimSession cim = CimSession.Create(null);
            IEnumerable<CimInstance> diskInfo = cim.QueryInstances(scope, "WQL", query);
            return diskInfo
                .Select(drive => new Dictionary<string, string>
                {
                    ["SerialNumber"] = CimStringProperty(drive, "SerialNumber"),
                    ["FriendlyName"] = CimStringProperty(drive, "FriendlyName"),
                    // Be careful with the next one. There are two MediaType properties
                    // so we change this one to DiskType
                    ["DiskType"] = CimStringProperty(drive, "MediaType") switch
                    {
                        "3" => "HDD",
                        "4" => "SSD",
                        "5" => "SCM",
                        _ => GetStringResource("DriveInfo_MediaType_Unspecified")
                    },
                    ["HealthStatus"] = CimStringProperty(drive, "HealthStatus") switch
                    {
                        "0" => GetStringResource("DriveInfo_Health_Healthy"),
                        "1" => GetStringResource("DriveInfo_Health_Warning"),
                        "2" => GetStringResource("DriveInfo_Health_Unhealthy"),
                        _ => GetStringResource("DriveInfo_Health_Unknown")
                    },
                    ["BusType"] = CimStringProperty(drive, "BusType") switch
                    {
                        "1" => "SCSI",
                        "2" => "ATAPI",
                        "3" => "ATA",
                        "4" => "IEEE 1394",
                        "5" => "SSA",
                        "6" => GetStringResource("DriveInfo_BusType_FiberChannel"),
                        "7" => "USB",
                        "8" => "RAID",
                        "9" => "iSCSI",
                        "10" => GetStringResource("DriveInfo_BusType_SAS"),
                        "11" => GetStringResource("DriveInfo_BusType_SATA"),
                        "12" => GetStringResource("DriveInfo_BusType_SD"),
                        "13" => GetStringResource("DriveInfo_BusType_MMC"),
                        "14" => GetStringResource("Reserved"),
                        "15" => GetStringResource("DriveInfo_BusType_FileBackedVirtual"),
                        "16" => GetStringResource("DriveInfo_BusType_StorageSpaces"),
                        "17" => "NVMe",
                        "18" => GetStringResource("Reserved"),
                        _ => GetStringResource("DriveInfo_BusType_Unknown")
                    }
                })
                .FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error retrieving MSFT_PhysicalDisk data for device {device}");
            return new()
            {
                { "SerialNumber", ""},
                { "FriendlyName", ""},
                { "MediaType", ""},
                { "HealthStatus", ""},
                { "BusType", ""}
            };
        }
    }
    #endregion Get info from MSFT_PhysicalDisk

    #region Get string value from CimInstance property
    /// <summary>
    /// Gets string from CimInstance property.
    /// </summary>
    /// <param name="instance">The CimInstance instance.</param>
    /// <param name="name">Name of the property.</param>
    /// <returns>A string.</returns>
    private static string CimStringProperty(CimInstance instance, string name)
    {
        return instance.CimInstanceProperties[name].Value.ToString()!;
    }
    #endregion Get string value from CimInstance property
}
