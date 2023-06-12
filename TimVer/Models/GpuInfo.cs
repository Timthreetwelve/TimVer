// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

public partial class GpuInfo : ObservableObject
{
    [ObservableProperty]
    private string _gpuName;

    [ObservableProperty]
    private string _gpuDeviceID;

    [ObservableProperty]
    private string _gpuDescription;

    [ObservableProperty]
    private string _gpuNumberOfColors;

    [ObservableProperty]
    private string _gpuHorizontalResolution;

    [ObservableProperty]
    private string _gpuVerticalResolution;

    [ObservableProperty]
    private string _gpuCurrentRefresh;

    [ObservableProperty]
    private string _gpuMinRefresh;

    [ObservableProperty]
    private string _gpuMaxRefresh;

    [ObservableProperty]
    private string _gpuAdapterRam;

    [ObservableProperty]
    private string _gpuBitsPerPixel;

    [ObservableProperty]
    private string _gpuVideoProcessor;

    [ObservableProperty]
    private string _gpuVideoDescription;
}
