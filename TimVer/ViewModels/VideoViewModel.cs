// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal sealed partial class VideoViewModel : ObservableObject
{
    #region Constructor
    public VideoViewModel()
    {
        if (AllControllers == null || AllControllers.Count == 0)
        {
            LoadData();
        }
    }
    #endregion Constructor

    #region Load data for video properties
    private static void LoadData()
    {
        AllControllers = VideoHelpers.GetAllVideoControllers();
        ControllerList = [.. AllControllers.Select(c =>
            c.GetValueOrDefault( GetStringResource("GraphicsInfo_GraphicsAdapter"), GetStringResource("GraphicsInfo_Unknown")))];
        VideoInfoCollection = AllControllers.FirstOrDefault();
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
        if (e.Source is ComboBox box && AllControllers?.Count > 0
            && box.SelectedIndex >= 0 && box.SelectedIndex < AllControllers.Count)
        {
            VideoInfoCollection = AllControllers[box.SelectedIndex];
            VideoPage.Instance!.VideoGrid.ItemsSource = VideoInfoCollection;
            _log.Debug($"Selected video controller index: {box.SelectedIndex}");
        }
    }
    #endregion Relay command

    #region Collections
    private static List<Dictionary<string, string>>? AllControllers { get; set; }

    public static List<string>? ControllerList { get; private set; }

    public static Dictionary<string, string>? VideoInfoCollection { get; private set; }
    #endregion Collections
}
