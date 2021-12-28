// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Dialogs;

public partial class ErrorDialog : UserControl
{
    public string Message { get; set; }

    public ErrorDialog()
    {
        InitializeComponent();
        DataContext = this;
    }
}
