// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

#region Navigation
/// <summary>
/// Navigation Page
/// </summary>
public enum NavPage
{
    [Description("Windows Info")]
    WindowsInfo = 0,
    [Description("Computer Info")]
    ComputerInfo = 1,
    Environment = 2,
    [Description("Build History")]
    History = 3,
    Settings = 4,
    About = 5,
    Exit = 6
}
#endregion Navigation

#region Theme
/// <summary>
/// Theme type, Light, Dark, Darker, or System
/// </summary>
public enum ThemeType
{
    Light = 0,
    [Description("Material Dark")]
    Dark = 1,
    Darker = 2,
    System = 3
}
#endregion Theme

#region UI size
/// <summary>
/// Size of the UI, Smallest, Smaller, Small, Default, Large, Larger, or Largest
/// </summary>
public enum MySize
{
    Smallest = 0,
    Smaller = 1,
    Small = 2,
    Default = 3,
    Large = 4,
    Larger = 5,
    Largest = 6
}
#endregion UI size

#region Accent color
/// <summary>
/// One of the 19 predefined Material Design in XAML colors
/// </summary>
public enum AccentColor
{
    Red = 0,
    Pink = 1,
    Purple = 2,
    [Description("Deep Purple")]
    DeepPurple = 3,
    Indigo = 4,
    Blue = 5,
    [Description("Light Blue")]
    LightBlue = 6,
    Cyan = 7,
    Teal = 8,
    Green = 9,
    [Description("Light Green")]
    LightGreen = 10,
    Lime = 11,
    Yellow = 12,
    Amber = 13,
    Orange = 14,
    [Description("Deep Orange")]
    DeepOrange = 15,
    Brown = 16,
    Grey = 17,
    [Description("Blue Gray")]
    BlueGray = 18
}
#endregion Accent color

/// <summary>
/// Space between rows in the data grids
/// </summary>
public enum Spacing
{
    Compact = 0,
    Comfortable = 1,
    Wide = 2
}
