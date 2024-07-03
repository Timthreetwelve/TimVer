// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

#region Navigation
/// <summary>
/// Navigation Page enumeration used by the Initial page ComboBox in Settings
/// </summary>
/// <remarks>
/// THe "Exit" nav page is not listed here
/// </remarks>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum NavPage
{
    [LocalizedDescription("SettingsEnum_Navigation_WindowsInfo")]
    WindowsInfo,
    [LocalizedDescription("SettingsEnum_Navigation_HardwareInfo")]
    ComputerInfo,
    [LocalizedDescription("SettingsEnum_Navigation_DriveInfo")]
    DriveInfo,
    [LocalizedDescription("SettingsEnum_Navigation_GraphicsInfo")]
    VideoInfo,
    [LocalizedDescription("SettingsEnum_Navigation_Environment")]
    Environment,
    [LocalizedDescription("SettingsEnum_Navigation_History")]
    History,
    [LocalizedDescription("SettingsEnum_Navigation_Settings")]
    Settings,
    [LocalizedDescription("SettingsEnum_Navigation_About")]
    About
}
#endregion Navigation

#region Theme
/// <summary>
/// Theme type
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum ThemeType
{
    /// <summary>
    /// Light theme, based on Material Design light theme.
    /// </summary>
    [LocalizedDescription("SettingsEnum_Theme_Light")]
    Light = 0,
    /// <summary>
    /// Dark theme, based on Material Design dark theme.
    /// </summary>
    [LocalizedDescription("SettingsEnum_Theme_Dark")]
    Dark = 1,
    /// <summary>
    /// A theme darker than the Material Design dark theme.
    /// </summary>
    [LocalizedDescription("SettingsEnum_Theme_Darker")]
    Darker = 2,
    /// <summary>
    /// Light or Darker theme based on the System theme.
    /// </summary>
    [LocalizedDescription("SettingsEnum_Theme_System")]
    System = 3
}
#endregion Theme

#region UI size
/// <summary>
/// Size of the UI
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum MySize
{
    [LocalizedDescription("SettingsEnum_Size_Smallest")]
    Smallest = 0,
    [LocalizedDescription("SettingsEnum_Size_Smaller")]
    Smaller = 1,
    [LocalizedDescription("SettingsEnum_Size_Small")]
    Small = 2,
    [LocalizedDescription("SettingsEnum_Size_Default")]
    Default = 3,
    [LocalizedDescription("SettingsEnum_Size_Large")]
    Large = 4,
    [LocalizedDescription("SettingsEnum_Size_Larger")]
    Larger = 5,
    [LocalizedDescription("SettingsEnum_Size_Largest")]
    Largest = 6
}
#endregion UI size

#region Accent color
/// <summary>
/// One of the 19 predefined Material Design in XAML colors plus Black & White
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum AccentColor
{
    [LocalizedDescription("SettingsEnum_AccentColor_Red")]
    Red = 0,
    [LocalizedDescription("SettingsEnum_AccentColor_Pink")]
    Pink = 1,
    [LocalizedDescription("SettingsEnum_AccentColor_Purple")]
    Purple = 2,
    [LocalizedDescription("SettingsEnum_AccentColor_DeepPurple")]
    Deep_Purple = 3,
    [LocalizedDescription("SettingsEnum_AccentColor_Indigo")]
    Indigo = 4,
    [LocalizedDescription("SettingsEnum_AccentColor_Blue")]
    Blue = 5,
    [LocalizedDescription("SettingsEnum_AccentColor_LightBlue")]
    Light_Blue = 6,
    [LocalizedDescription("SettingsEnum_AccentColor_Cyan")]
    Cyan = 7,
    [LocalizedDescription("SettingsEnum_AccentColor_Teal")]
    Teal = 8,
    [LocalizedDescription("SettingsEnum_AccentColor_Green")]
    Green = 9,
    [LocalizedDescription("SettingsEnum_AccentColor_LightGreen")]
    Light_Green = 10,
    [LocalizedDescription("SettingsEnum_AccentColor_Lime")]
    Lime = 11,
    [LocalizedDescription("SettingsEnum_AccentColor_Yellow")]
    Yellow = 12,
    [LocalizedDescription("SettingsEnum_AccentColor_Amber")]
    Amber = 13,
    [LocalizedDescription("SettingsEnum_AccentColor_Orange")]
    Orange = 14,
    [LocalizedDescription("SettingsEnum_AccentColor_DeepOrange")]
    Deep_Orange = 15,
    [LocalizedDescription("SettingsEnum_AccentColor_Brown")]
    Brown = 16,
    [LocalizedDescription("SettingsEnum_AccentColor_Gray")]
    Gray = 17,
    [LocalizedDescription("SettingsEnum_AccentColor_BlueGray")]
    Blue_Gray = 18,
    [LocalizedDescription("SettingsEnum_AccentColor_Black")]
    Black = 19,
    [LocalizedDescription("SettingsEnum_AccentColor_White")]
    White = 20,
}
#endregion Accent color

#region Spacing
/// <summary>
/// Space between rows in the data grids
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum Spacing
{
    /// <summary>
    /// Smallest datagrid row spacing.
    /// </summary>
    [LocalizedDescription("SettingsEnum_Spacing_Compact")]
    Compact = 0,
    /// <summary>
    /// Middle datagrid row spacing.
    /// </summary>
    [LocalizedDescription("SettingsEnum_Spacing_Comfortable")]
    Comfortable = 1,
    /// <summary>
    /// Widest datagrid row spacing.
    /// </summary>
    [LocalizedDescription("SettingsEnum_Spacing_Wide")]
    Wide = 2
}
#endregion Spacing
