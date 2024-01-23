// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class for gathering information from disk drives.
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
        foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
        {
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
            _ = MessageBox.Show($"Unknown error\n{ex.Message}",
                                GetStringResource("DriveInfo_Error"),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error)));
        }
        return null;
    }
    #endregion Get array of logical drives

    #region Get logical drive information
    public static List<LogicalDrives> GetLogicalDriveInfo()
    {
        int count = 0;
        List<LogicalDrives> logicalDrives = [];
        if (GetLogicalDrives() != null)
        {
            MainWindowUIHelpers.MainWindowWaitPointer();
            Stopwatch watch = Stopwatch.StartNew();
            foreach (DriveInfo drive in GetLogicalDrives())
            {
                try
                {
                    if (drive != null && GetDriveDetails(drive) != null)
                    {
                        logicalDrives.Add(GetDriveDetails(drive));
                        count++;
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
            MainWindowUIHelpers.MainWindowNormalPointer();
            string drv = count != 1 ? "drives" : "drive";
            string msg = $"Found {count} logical {drv} in {watch.Elapsed.TotalMilliseconds:N2} ms";
            _log.Debug(msg);
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
            && !UserSettings.Setting.IncludeNetwork)
        {
            return null;
        }
        // If the drive is removable check settings to see if it needs to be included
        if (string.Equals(d.DriveType.ToString(), "removable", StringComparison.OrdinalIgnoreCase)
            && !UserSettings.Setting.IncludeRemovable)
        {
            return null;
        }
        // If the drive is ready
        if (d.IsReady)
        {
            int GBPref = UserSettings.Setting.Use1024 ? 1024 : 1000;

            // Add the information for each drive to the list
            return new()
            {
                Name = d.Name,
                DriveType = ConvertDriveType(d.DriveType),
                Format = d.DriveFormat,
                Label = d.VolumeLabel,
                TotalSize = Math.Round(d.TotalSize / Math.Pow(GBPref, 3), 2),
                GBFree = Math.Round(d.AvailableFreeSpace / Math.Pow(GBPref, 3), 2),
                PercentFree = (double)d.AvailableFreeSpace / d.TotalSize
            };
        }
        // If the drive is not ready
        else if (UserSettings.Setting.IncludeNotReady)
        {
            return new()
            {
                Name = d.Name,
                DriveType = d.DriveType.ToString(),
                Label = GetStringResource("DriveInfo_NotReady")
            };
        }
        return null;
    }
    #endregion Get details for an individual drive

    #region Get Physical disk info from WMI
    /// <summary>
    /// Gets the physical disk information.
    /// </summary>
    public static List<PhysicalDrives> GetPhysicalDriveInfo()
    {
        if (UserSettings.Setting.GetPhysicalDrives)
        {
            MainWindowUIHelpers.MainWindowWaitPointer();
            Stopwatch watch = Stopwatch.StartNew();
            List<PhysicalDrives> physicalDrives = [];
            const string query = "SELECT InterfaceType, MediaType, Model, Name, Status, Index, Partitions, Size FROM Win32_DiskDrive";
            using (CimSession cimSession = CimSession.Create(null))
            {
                IEnumerable<CimInstance> win32Drives = cimSession.QueryInstances("root/CIMV2", "WQL", query);
                int gbPref = UserSettings.Setting.Use1024 ? 1024 : 1000;
                foreach (var drive in win32Drives)
                {
                    PhysicalDrives pDisk = new()
                    {
                        Interface = CimStringProp(drive, "InterfaceType"),
                        MediaType = CimStringProp(drive, "MediaType").Replace("media", "", StringComparison.OrdinalIgnoreCase),
                        Model = CimStringProp(drive, "Model"),
                        Name = CimStringProp(drive, "Name"),
                        Status = CimStringProp(drive, "Status"),
                        Index = (uint)drive.CimInstanceProperties["Index"].Value,
                        Partitions = (uint)drive.CimInstanceProperties["Partitions"].Value,
                        Size = Math.Round(Convert.ToDouble(drive.CimInstanceProperties["Size"].Value) / Math.Pow(gbPref, 3), 2)
                    };
                    Dictionary<string, string> win32DiskDrive = GetWin32DiskDrive(pDisk.Index);
                    pDisk.BusType = win32DiskDrive["BusType"];
                    pDisk.DiskType = win32DiskDrive["MediaType"];
                    pDisk.Health = win32DiskDrive["Health"];
                    pDisk.PartitionStyle = win32DiskDrive["PartitionStyle"];
                    pDisk.IsBoot = win32DiskDrive["IsBoot"];
                    physicalDrives.Add(pDisk);
                }
            }
            watch.Stop();
            string drv = physicalDrives.Count != 1 ? "drives" : "drive";
            _log.Debug($"Found {physicalDrives.Count} physical {drv} in {watch.Elapsed.TotalMilliseconds:N2} ms");
            MainWindowUIHelpers.MainWindowNormalPointer();
            return physicalDrives.OrderBy(i => i.Index).ToList();
        }
        else
        {
            List<PhysicalDrives> physicalDrives = [];
            PhysicalDrives emptyList = new()
            {
                Message = GetStringResource("DriveInfo_PhysicalDisabled")
            };
            physicalDrives.Add(emptyList);
            return physicalDrives;
        }
    }

    private static string CimStringProp(CimInstance instance, string name)
    {
        return instance.CimInstanceProperties[name].Value.ToString();
    }
    #endregion Get Physical disk info from WMI

    private static Dictionary<string, string> GetMSFTDisk(uint number)
    {
        Dictionary<string, string> driveInfo = [];

        const string scope = @"\\.\root\Microsoft\Windows\Storage";
        string query = $"SELECT IsBoot, IsSystem, PartitionStyle, Model FROM MSFT_Disk WHERE Number = {number}";

        using ManagementObjectSearcher searcher = new(scope, query);
        foreach (ManagementBaseObject drive in searcher.Get())
        {
            driveInfo.Add("IsBoot", drive["IsBoot"].ToString());
            driveInfo.Add("IsSystem", drive["IsSystem"].ToString());
            driveInfo.Add("PartitionStyle", drive["PartitionStyle"].ToString());
        }
        return driveInfo;
    }

    #region Get additional info from MSFT_PhysicalDisk
    /// <summary>
    /// Gets MSFT_PhysicalDisk info for the specified device.
    /// </summary>
    /// <param name="device">The device ID of the disk</param>
    /// <returns>Dictionary of values.</returns>
    private static Dictionary<string, string> GetWin32DiskDrive(uint device)
    {
        Dictionary<string, string> driveInfo = [];

        const string scope = @"\\.\root\Microsoft\Windows\Storage";
        string query = $"SELECT MediaType, HealthStatus, BusType, SerialNumber, FriendlyName FROM MSFT_PhysicalDisk WHERE DeviceID = {device}";
        string health = GetStringResource("MsgText_NotAvailable");
        string mediaType = GetStringResource("MsgText_NotAvailable");
        string busType = GetStringResource("MsgText_NotAvailable");
        string isBoot = GetStringResource("MsgText_NotAvailable");
        string isSystem = GetStringResource("MsgText_NotAvailable");
        string partitionStyle = GetStringResource("MsgText_NotAvailable");

        try
        {
            using ManagementObjectSearcher searcher = new(scope, query);
            foreach (ManagementBaseObject drive in searcher.Get())
            {
                if (drive["MediaType"] != null)
                {
                    switch (drive["MediaType"].ToString())
                    {
                        case "3":
                            mediaType = "HDD";
                            break;
                        case "4":
                            mediaType = "SSD";
                            break;
                        case "5":
                            mediaType = "SCM";
                            break;
                        default:
                            mediaType = GetStringResource("DriveInfo_MediaType_Unspecified");
                            break;
                    }
                }
                if (drive["HealthStatus"] != null)
                {
                    switch (drive["HealthStatus"].ToString())
                    {
                        case "0":
                            health = GetStringResource("DriveInfo_Health_Healthy");
                            break;
                        case "1":
                            health = GetStringResource("DriveInfo_Health_Warning");
                            break;
                        case "2":
                            health = GetStringResource("DriveInfo_Health_Unhealthy");
                            break;
                        default:
                            health = GetStringResource("DriveInfo_Health_Unknown");
                            break;
                    }
                }
                if (drive["BusType"] != null)
                {
                    switch (drive["BusType"].ToString())
                    {
                        case "1":
                            busType = "SCSI";
                            break;
                        case "2":
                            busType = "ATAPI";
                            break;
                        case "3":
                            busType = "ATA";
                            break;
                        case "4":
                            busType = "IEEE 1394";
                            break;
                        case "5":
                            busType = "SSA";
                            break;
                        case "6":
                            busType = GetStringResource("DriveInfo_BusType_FiberChannel");
                            break;
                        case "7":
                            busType = "USB";
                            break;
                        case "8":
                            busType = "RAID";
                            break;
                        case "9":
                            busType = "iSCSI";
                            break;
                        case "10":
                            busType = GetStringResource("DriveInfo_BusType_SAS");
                            break;
                        case "11":
                            busType = GetStringResource("DriveInfo_BusType_SATA");
                            break;
                        case "12":
                            busType = GetStringResource("DriveInfo_BusType_SD");
                            break;
                        case "13":
                            busType = GetStringResource("DriveInfo_BusType_MMC");
                            break;
                        case "15":
                            busType = GetStringResource("DriveInfo_BusType_FileBackedVirtual");
                            break;
                        case "16":
                            busType = GetStringResource("DriveInfo_BusType_StorageSpaces");
                            break;
                        case "17":
                            busType = "NVMe";
                            break;
                        case "14":
                        case "18":
                            busType = GetStringResource("Reserved");
                            break;
                        default:
                            busType = GetStringResource("DriveInfo_BusType_Unknown");
                            break;
                    }
                }
                isBoot = GetMSFTDisk(device)["IsBoot"];
                isSystem = GetMSFTDisk(device)["IsSystem"];
                partitionStyle = GetMSFTDisk(device)["PartitionStyle"];
                switch (partitionStyle)
                {
                    case "1":
                        partitionStyle = "MBR";
                        break;
                    case "2":
                        partitionStyle = "GPT";
                        break;
                    default:
                        partitionStyle = "unknown";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error retrieving MSFT_PhysicalDisk data for device {device}");
        }
        finally
        {
            driveInfo.Add("MediaType", mediaType);
            driveInfo.Add("Health", health);
            driveInfo.Add("BusType", busType);
            driveInfo.Add("IsBoot", isBoot);
            driveInfo.Add("IsSystem", isSystem);
            driveInfo.Add("PartitionStyle", partitionStyle);
        }
        return driveInfo;
    }
    #endregion Get additional info from MSFT_PhysicalDisk

    private static string ConvertDriveType(DriveType driveType)
    {
        switch (driveType)
        {
            case DriveType.NoRootDirectory:
                return GetStringResource("DriveInfo_DriveType_NoRootDirectory");
            case DriveType.Fixed:
                return GetStringResource("DriveInfo_DriveType_Fixed");
            case DriveType.Network:
                return GetStringResource("DriveInfo_DriveType_Network");
            case DriveType.Removable:
                return GetStringResource("DriveInfo_DriveType_Removable");
            case DriveType.Ram:
                return GetStringResource("DriveInfo_DriveType_Ram");
            case DriveType.CDRom:
                return GetStringResource("DriveInfo_DriveType_CDRom");
            default:
                return GetStringResource("DriveInfo_DriveType_Unknown");
        }
    }
}
