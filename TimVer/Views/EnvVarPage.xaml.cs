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
        EnvVarViewModel.StaticPropertyChanged += EnvVarViewModel_StaticPropertyChanged!;
    }

    private void EnvVarViewModel_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        FilterTheGrid(EnvVarViewModel.FilterText);
    }

    #region Filter the datagrid
    /// <summary>
    /// Filters the grid.
    /// </summary>
    private void FilterTheGrid(string filterText)
    {
        if (string.IsNullOrEmpty(filterText))
        {
            EnvDataGrid.Items.Filter = _ => true;
        }
        else if (filterText.StartsWith('!'))
        {
            filterText = filterText[1..].TrimStart(' ');
            EnvDataGrid.Items.Filter = o =>
            {
                EnvVariable? envVariable = o as EnvVariable;
                return !envVariable!.Variable!.Contains(filterText, StringComparison.CurrentCultureIgnoreCase);
            };
        }
        else
        {
            EnvDataGrid.Items.Filter = o =>
            {
                EnvVariable? envVariable = o as EnvVariable;
                return envVariable!.Variable!.Contains(filterText!, StringComparison.CurrentCultureIgnoreCase);
            };
        }

        if (EnvDataGrid.Items.Count == 1)
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_FilterOneRowShown"), 2000);
        }
        else
        {
            SnackbarMsg.ClearAndQueueMessage(string.Format(CultureInfo.InvariantCulture, MsgTextFilterRowsShown, EnvDataGrid.Items.Count), 2000);
        }
    }
    #endregion Filter the datagrid
}
