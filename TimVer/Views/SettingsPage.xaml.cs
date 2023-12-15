// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Views;

/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    #region Language ComboBox loaded event
    /// <summary>
    /// Handles the Loaded event of the language ComboBox.
    /// </summary>
    private void CbxLanguage_Loaded(object sender, RoutedEventArgs e)
    {
        cbxLanguage.SelectedIndex = LocalizationHelpers.GetLanguageIndex();
    }
    #endregion Language ComboBox loaded event
}
