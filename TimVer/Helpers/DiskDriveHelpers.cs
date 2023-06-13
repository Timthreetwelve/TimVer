// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

/// <summary>
/// Class for gathering information from disk drives.
/// </summary>
public static class DiskDriveHelpers
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

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
            _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            _ = MessageBox.Show($"I/O error\n{ex.Message}",
                                "DriveInfo Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error)));
        }
        catch (UnauthorizedAccessException ex)
        {
            _log.Error(ex, "Unauthorized Access");
            _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            _ = MessageBox.Show($"Unauthorized Access\n{ex.Message}",
                                "DriveInfo Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error)));
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Unknown error");
            _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            _ = MessageBox.Show($"Unknown error\n{ex.Message}",
                                "DriveInfo Error",
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
        List<LogicalDrives> logicalDrives = new();
        if (GetLogicalDrives() != null)
        {
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
                    _ = Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    _ = MessageBox.Show($"Error processing drive {drive}\n{ex.Message}",
                        "DriveInfo Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error)));
                    LogicalDrives error = new()
                    {
                        Name = drive.Name,
                        Label = "Error - See log file"
                    };
                    logicalDrives.Add(error);
                }
            }
            watch.Stop();
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
                Label = "Error - See log file"
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
                DriveType = d.DriveType.ToString(),
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
                Label = "Not Ready"
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
            List<PhysicalDrives> physicalDrives = new();
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
            List<PhysicalDrives> physicalDrives = new();
            PhysicalDrives emptyList = new()
            {
                DiskType = "Collection of Physical Drive information is disabled in Settings"
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

    #region Get additional info from MSFT_PhysicalDisk
    /// <summary>
    /// Gets MSFT_PhysicalDisk info for the specified device.
    /// </summary>
    /// <param name="device">The device ID of the disk</param>
    /// <returns>Dictionary of values.</returns>
    private static Dictionary<string, string> GetWin32DiskDrive(uint device)
    {
        Dictionary<string, string> driveInfo = new();

        const string scope = @"\\.\root\Microsoft\Windows\Storage";
        string query = $"SELECT MediaType, HealthStatus, BusType FROM MSFT_PhysicalDisk WHERE DeviceID = {device}";
        string health = "Not Available";
        string mediaType = "Not Available";
        string busType = "Not Available";

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
                            mediaType = "Unspecified";
                            break;
                    }
                }
                if (drive["HealthStatus"] != null)
                {
                    switch (drive["HealthStatus"].ToString())
                    {
                        case "0":
                            health = "Healthy";
                            break;
                        case "1":
                            health = "Warning";
                            break;
                        case "2":
                            health = "Unhealthy";
                            break;
                        default:
                            health = "Unknown";
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
                            busType = "Fiber Channel";
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
                            busType = "Serial Attached SCSI (SAS)";
                            break;
                        case "11":
                            busType = "Serial ATA (SATA)";
                            break;
                        case "12":
                            busType = "Secure Digital (SD)";
                            break;
                        case "13":
                            busType = "Multimedia Card (MMC)";
                            break;
                        case "15":
                            busType = "File-Backed Virtual";
                            break;
                        case "16":
                            busType = "Storage Spaces";
                            break;
                        case "17":
                            busType = "NVMe";
                            break;
                        case "14":
                        case "18":
                            busType = "Reserved";
                            break;
                        default:
                            busType = "Unknown";
                            break;
                    }
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
        }
        return driveInfo;
    }
    #endregion Get additional info from MSFT_PhysicalDisk
}
