// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal class WindowsInfoViewModel : ObservableObject
{
    #region Constructor
    public WindowsInfoViewModel()
    {
        if (WindowsInfoList == null)
        {
            LoadData();
        }
    }
    #endregion Constructor

    #region Collection of Windows information
    public static Dictionary<string, string>? WindowsInfoList { get; set; }
    #endregion Collection of Windows information

    #region Get data for all properties
    /// <summary>
    /// Get data for all properties.
    /// </summary>
    public static void LoadData()
    {
        WindowsInfoList = WindowsInfoHelpers.GetWindowsInfo();
    }
    #endregion Get data for all properties
}
