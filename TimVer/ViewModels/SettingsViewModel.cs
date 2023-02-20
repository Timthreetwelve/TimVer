// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private static bool _uIExpanderOpen;

    [ObservableProperty]
    private static bool _appExpanderOpen;

    public static List<NavPage> NavPages { get; private set; } = new();

    public static void ParseInitialPage()
    {
        foreach (NavPage page in Enum.GetValues<NavPage>())
        {
            if (!page.Equals(NavPage.Exit))
            {
                NavPages.Add(page);
            }
        }
    }
}
