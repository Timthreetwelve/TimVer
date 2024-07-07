// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal partial class VideoViewModel : ObservableObject
{
    #region Constructor
    public VideoViewModel()
    {
        if (VideoInfoCollection == null)
        {
            LoadData();
        }
    }
    #endregion Constructor

    #region Load data for video properties
    private static void LoadData()
    {
        ControllerList = VideoHelpers.GetGPUList()!;
        VideoInfoCollection = VideoHelpers.GetVideoInfo(ControllerList[0]!);
    }
    #endregion Load data for video properties

    #region Relay command
    /// <summary>
    /// For the video controller combo box.
    /// </summary>
    /// <param name="e">Selection changed event arguments.</param>
    [RelayCommand]
    private static void SelectVideoController(SelectionChangedEventArgs e)
    {
        if (e.Source is ComboBox box)
        {
            VideoPage.Instance!.VideoGrid.ItemsSource = VideoHelpers.GetVideoInfo(ControllerList![box.SelectedIndex]!);
        }
    }
    #endregion Relay command

    #region Collections
    public static List<string>? ControllerList { get; private set; }

    public static Dictionary<string, string>? VideoInfoCollection { get; set; }
    #endregion Collections
}
