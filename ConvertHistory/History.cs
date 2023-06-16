// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace ConvertHistory;
internal partial class History : ObservableObject
{
    [ObservableProperty]
    private string _hDate;

    [ObservableProperty]
    private string _hBuild;

    [ObservableProperty]
    private string _hVersion;

    [ObservableProperty]
    private string _hBranch;
}
