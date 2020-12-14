// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System.Windows.Controls;
using TimVer.ViewModels;

namespace TimVer.Views
{
    public partial class HistoryView : UserControl
    {
        public HistoryView()
        {
            InitializeComponent();

            HistoryGrid.ItemsSource = HistoryViewModel.ReadHistory();
        }
    }
}
