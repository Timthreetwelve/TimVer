// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal sealed class ComputerInfoViewModel : ObservableObject
{
    #region Constructor
    public ComputerInfoViewModel()
    {
        if (ComputerInfoList is null)
        {
            LoadData();
        }
    }
    #endregion Constructor

    #region Collection of computer hardware information
    public static Dictionary<string, string>? ComputerInfoList { get; private set; }
    #endregion Collection of computer hardware information

    #region Load data
    /// <summary>
    /// Get data for all properties.
    /// </summary>
    private static void LoadData()
    {
        ComputerInfoList = ComputerSystemHelpers.GetComputerInfo();
    }
    #endregion Load data
}
