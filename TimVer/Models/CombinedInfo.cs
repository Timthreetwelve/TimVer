// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

/// <summary>
/// Class that works the TimVer magic
/// </summary>
public static class CombinedInfo
{
    #region OS Architecture
    private static string? _arch;
    public static string Arch
    {
        get
        {
            if (_arch != null)
            {
                return _arch;
            }
            _arch = OperatingSystemHelpers.OsArchitecture();
            return _arch;
        }
    }
    #endregion OS Architecture

    #region Build number
    private static string? _build;
    public static string Build
    {
        get
        {
            if (_build != null)
            {
                return _build;
            }
            string curBuild = RegistryHelpers.GetRegistryInfo("CurrentBuild");
            string ubr = RegistryHelpers.GetRegistryInfo("UBR");
            _build = string.Format($"{curBuild}.{ubr}");
            return _build;
        }
    }
    #endregion Build number

    #region Build branch
    private static string? _buildBranch;
    public static string BuildBranch
    {
        get
        {
            if (_buildBranch != null)
            {
                return _buildBranch;
            }
            _buildBranch = RegistryHelpers.GetRegistryInfo("BuildBranch");
            return _buildBranch;
        }
    }
    #endregion Build branch

    #region Edition
    private static string? _editionID;
    public static string EditionID
    {
        get
        {
            if (_editionID != null)
            {
                return _editionID;
            }
            _editionID = RegistryHelpers.GetRegistryInfo("EditionID");
            return _editionID;
        }
    }
    #endregion Edition

    #region Install date
    private static DateTime _installDate;
    public static DateTime InstallDate
    {
        get
        {
            if (_installDate != default)
            {
                return _installDate;
            }
            _installDate = RegistryHelpers.GetInstallDateDate();
            return _installDate;
        }
    }
    #endregion Install date

    #region Last boot up time
    private static DateTime _lastBoot;
    public static DateTime LastBoot
    {
        get
        {
            if (_lastBoot != default)
            {
                return _lastBoot;
            }
            _lastBoot = OperatingSystemHelpers.CimQueryOSDateTime("LastBootUpTime");
            return _lastBoot;
        }
    }
    #endregion Last boot up time

    #region Uptime
    public static TimeSpan Uptime => EnvironmentHelpers.GetUptime();
    #endregion Uptime

    #region Machine name
    private static string? _machineName;
    public static string MachName
    {
        get
        {
            if (_machineName != null)
            {
                return _machineName;
            }

            _machineName = ComputerSystemHelpers.CimQuerySys("Name");
            return _machineName;
        }
    }
    #endregion Machine name

    #region Manufacturer
    private static string? _manufacturer;
    public static string Manufacturer
    {
        get
        {
            if (_manufacturer != null)
            {
                return _manufacturer;
            }
            _manufacturer = ComputerSystemHelpers.CimQuerySys("Manufacturer");
            return _manufacturer;
        }
    }
    #endregion Manufacturer

    #region Computer Model
    private static string? _model;
    public static string Model
    {
        get
        {
            if (_model != null)
            {
                return _model;
            }
            _model = ComputerSystemHelpers.CimQuerySys("Model");
            return _model;
        }
    }
    #endregion Computer Model

    #region Processor architecture
    private static string? _procArch;
    public static string ProcArch
    {
        get
        {
            if (_procArch != null)
            {
                return _procArch;
            }
            string result = ProcessorHelpers.CimQueryProc("AddressWidth");
            _procArch = string.Format($"{result} bit");
            return _procArch;
        }
    }
    #endregion Processor architecture

    #region Processor cores
    private static string? _procCores;
    public static string ProcCores
    {
        get
        {
            if (_procCores != null)
            {
                return _procCores;
            }
            _procCores = ProcessorHelpers.CimQueryProc("NumberOfCores");
            return _procCores;
        }
    }
    #endregion Processor cores

    #region Processor threads
    private static string? _procThreads;
    public static string ProcThreads
    {
        get
        {
            if (_procThreads != null)
            {
                return _procThreads;
            }
            _procThreads = ProcessorHelpers.CimQueryProc("NumberOfLogicalProcessors");
            return _procThreads;
        }
    }
    #endregion Processor threads

    #region Processor name
    private static string? _procName;
    public static string ProcName
    {
        get
        {
            if (_procName != null)
            {
                return _procName;
            }
            _procName = ProcessorHelpers.CimQueryProc("Name");
            return _procName;
        }
    }
    #endregion Processor name

    #region Processor description
    private static string? _procDescription;
    public static string ProcDescription
    {
        get
        {
            if (_procDescription != null)
            {
                return _procDescription;
            }
            _procDescription = ProcessorHelpers.CimQueryProc("Description");
            return _procDescription;
        }
    }
    #endregion Processor speed

    #region Product name
    private static string? _prodName;
    public static string ProdName
    {
        get
        {
            if (_prodName != null)
            {
                return _prodName;
            }
            _prodName = OperatingSystemHelpers.GetWin32OperatingSystem("Caption");
            return _prodName;
        }
    }
    #endregion Product name

    #region Registered user
    private static string? _regUser;
    public static string RegUser
    {
        get
        {
            if (_regUser != null)
            {
                return _regUser;
            }
            if (UserSettings.Setting!.ShowUser)
            {
                _regUser = RegistryHelpers.GetRegistryInfo("RegisteredOwner");
            }
            else
            {
                _regUser = null;
            }
            return _regUser!;
        }
    }
    #endregion Registered user

    #region Registered organization
    private static string? _regOrg;
    public static string RegOrganization
    {
        get
        {
            if (_regOrg != null)
            {
                return _regOrg;
            }
            if (UserSettings.Setting!.ShowUser)
            {
                _regOrg = RegistryHelpers.GetRegistryInfo("RegisteredOrganization");
            }
            else
            {
                _regOrg = null;
            }
            return _regOrg!;
        }
    }
    #endregion Registered organization

    #region Temp folder
    private static string? _tempFolder;
    public static string TempFolder
    {
        get
        {
            if (_tempFolder != null)
            {
                return _tempFolder;
            }
            _tempFolder = Path.GetTempPath();
            return _tempFolder;
        }
    }
    #endregion Temp folder

    #region Total usable memory
    private static string? _totalMemory;
    public static string TotalMemory
    {
        get
        {
            if (_totalMemory != null)
            {
                return _totalMemory;
            }

            _totalMemory = MemoryHelpers.GetUsableRam();
            return _totalMemory;
        }
    }
    #endregion Total usable memory

    #region Total installed memory
    private static string? _installedMemory;
    public static string InstalledMemory
    {
        get
        {
            if (_installedMemory != null)
            {
                return _installedMemory;
            }

            _installedMemory = MemoryHelpers.GetInstalledRam();
            return _installedMemory;
        }
    }
    #endregion Total installed memory

    #region Version
    private static string? _version;
    public static string Version
    {
        get
        {
            if (_version != null)
            {
                return _version;
            }
            string result = RegistryHelpers.GetRegistryInfo("DisplayVersion");
            if (result == "no data")
            {
                result = RegistryHelpers.GetRegistryInfo("ReleaseID");
            }
            _version = result;
            return _version;
        }
    }
    #endregion Version

    #region Windows folder
    private static string? _windowsFolder;
    public static string WindowsFolder
    {
        get
        {
            if (_windowsFolder != null)
            {
                return _windowsFolder;
            }
            _windowsFolder = EnvironmentHelpers.GetSpecialFolder(Environment.SpecialFolder.Windows);
            return _windowsFolder;
        }
    }
    #endregion Windows folder

    #region Logical disk drives
    private static ObservableCollection<LogicalDrives>? _logicalDrivesList;
    public static ObservableCollection<LogicalDrives> LogicalDrivesList
    {
        get
        {
            if (_logicalDrivesList?.Count > 0)
            {
                return _logicalDrivesList;
            }
            _logicalDrivesList = new ObservableCollection<LogicalDrives>(DiskDriveHelpers.GetLogicalDriveInfo());
            return _logicalDrivesList;
        }
    }
    #endregion Logical disk drives

    #region Physical disk drives
    private static ObservableCollection<PhysicalDrives>? _physicalDrivesList;
    public static ObservableCollection<PhysicalDrives> PhysicalDrivesList
    {
        get
        {
            if (_physicalDrivesList?.Count > 0)
            {
                return _physicalDrivesList;
            }
            _physicalDrivesList = new ObservableCollection<PhysicalDrives>(DiskDriveHelpers.GetPhysicalDriveInfo());
            return _physicalDrivesList;
        }
    }
    #endregion Physical disk drives

    #region Video info
    private static ObservableCollection<GpuInfo>? _gpuList;
    public static ObservableCollection<GpuInfo> GPUList
    {
        get
        {
            if (_gpuList?.Count > 0)
            {
                return _gpuList;
            }
            _gpuList = new ObservableCollection<GpuInfo>((IEnumerable<GpuInfo>)VideoHelpers.GetGpuInfo());
            return _gpuList;
        }
    }
    #endregion Video info

    #region BIOS Information
    private static string? _biosManufacturer;
    public static string BiosManufacturer
    {
        get
        {
            if (_biosManufacturer != null)
            {
                return _biosManufacturer;
            }
            _biosManufacturer = BiosHelpers.GetBiosManufacturer();
            return _biosManufacturer;
        }
    }

    private static string? _biosName;
    public static string BiosName
    {
        get
        {
            if (_biosName != null)
            {
                return _biosName;
            }
            _biosName = BiosHelpers.GetBiosVersion();
            return _biosName;
        }
    }

    private static DateTime _biosDate;
    public static DateTime BiosDate
    {
        get
        {
            if (_biosDate != default)
            {
                return _biosDate;
            }
            _biosDate = BiosHelpers.GetBiosDate();
            return _biosDate;
        }
    }
    #endregion BIOS Information

    #region .NET info
    private static string? _dotNetVersion;
    public static string DotNetVersion
    {
        get
        {
            if (_dotNetVersion != default)
            {
                return _dotNetVersion;
            }
            _dotNetVersion = AppInfo.RuntimeVersion.Replace(".NET", "").Trim();
            return _dotNetVersion;
        }
    }
    #endregion

    #region UEFI
    private static string? _uefiMode;
    public static string UefiMode
    {
        get
        {
            if (_uefiMode != null)
            {
                return _uefiMode;
            }
            _uefiMode = UefiHelpers.UEFIEnabled();
            return _uefiMode;
        }
    }

    private static string? _uefiSecureBoot;
    public static string UefiSecureBoot
    {
        get
        {
            if (_uefiSecureBoot != null)
            {
                return _uefiSecureBoot;
            }
            _uefiSecureBoot = UefiHelpers.UEFISecureBoot();
            return _uefiSecureBoot;
        }
    }
    #endregion UEFI
}
