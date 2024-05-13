// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

internal partial class VideoViewModel : ObservableObject
{
    public VideoViewModel()
    {
        if (VideoInfoList is null)
        {
            LoadData();
        }
    }

    private static void LoadData()
    {
        List<string?> gpuList = VideoHelpers.GetGPUList();
        if ( gpuList.Count == 1)
        {
            VideoInfoList = VideoHelpers.GetVideoInfo(gpuList[0]!);
        }
    }

    public static ObservableCollection<GpuInfo>? GPUList { get; set; }

    public static Dictionary<string, string>? VideoInfoList { get; set; }
}
