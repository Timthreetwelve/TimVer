// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Dialogs;

public partial class OkDialog : UserControl
{
    public string Message { get; set; }

    public OkDialog()
    {
        InitializeComponent();
        DataContext = this;
    }
}
