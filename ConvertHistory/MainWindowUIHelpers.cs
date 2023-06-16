// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace ConvertHistory;

internal static class MainWindowUIHelpers
{
    /// <summary>
    /// Sets the theme
    /// </summary>
    /// <param name="mode">Light, Dark, Darker or System</param>
    internal static void SetBaseTheme(bool mode)
    {
        //Retrieve the app's existing theme
        PaletteHelper paletteHelper = new();
        ITheme theme = paletteHelper.GetTheme();

        switch (mode)
        {
            case false:
                theme.SetBaseTheme(Theme.Light);
                theme.Paper = Colors.WhiteSmoke;
                break;
            case true:
                // Set card and paper background colors a bit darker
                theme.SetBaseTheme(Theme.Dark);
                theme.CardBackground = (Color)ColorConverter.ConvertFromString("#FF141414");
                theme.Paper = (Color)ColorConverter.ConvertFromString("#FF202020");
                break;
        }

        //Change the app's current theme
        paletteHelper.SetTheme(theme);
    }
}
