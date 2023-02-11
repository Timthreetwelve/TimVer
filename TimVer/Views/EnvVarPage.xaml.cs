// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Views;

/// <summary>
/// Interaction logic for EnvVarPage.xaml
/// </summary>
public partial class EnvVarPage : UserControl
{
    public EnvVarPage()
    {
        InitializeComponent();

        EnvDataGrid.ItemsSource = EnvVariable.EnvVariableList;
    }
}
