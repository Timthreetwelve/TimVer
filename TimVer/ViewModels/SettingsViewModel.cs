// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    public static List<NavPage> NavPages { get; } = new();

    public static void ParseInitialPage()
    {
        NavPages.AddRange(Enum.GetValues<NavPage>());
    }
}
