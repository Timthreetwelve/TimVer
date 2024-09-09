// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Views;

public partial class VideoPage : UserControl
{
    public static VideoPage? Instance { get; private set; }

    public VideoPage()
    {
        InitializeComponent();

        Instance = this;
    }
}
