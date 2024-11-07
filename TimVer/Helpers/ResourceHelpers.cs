// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

internal static class ResourceHelpers
{
    #region Get count of strings in resource dictionary
    /// <summary>
    /// Gets the count of strings in the default resource dictionary.
    /// </summary>
    /// <returns>Count as int.</returns>
    public static int GetTotalDefaultLanguageCount()
    {
        ResourceDictionary dictionary = new()
        {
            Source = new Uri("Languages/Strings.en-US.xaml", UriKind.RelativeOrAbsolute)
        };
        return dictionary.Count;
    }
    #endregion Get count of strings in resource dictionary

    #region Get a resource string
    /// <summary>
    /// Gets the string resource for the key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>String</returns>
    /// <remarks>
    /// Want to throw here so that missing resource doesn't make it into a release.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Resource description is null.</exception>
    /// <exception cref="ArgumentException">Resource was not found.</exception>
    public static string GetStringResource(string key)
    {
        object description;
        try
        {
            description = Application.Current.TryFindResource(key);
        }
        catch (Exception ex)
        {
            if (Debugger.IsAttached)
            {
                throw new ArgumentException($"Resource not found: {key}");
            }
            else
            {
                _log.Error(ex, $"Resource not found: {key}");
                return $"Resource not found: {key}";
            }
        }

        if (description is null)
        {
            if (Debugger.IsAttached)
            {
                throw new ArgumentNullException($"Resource not found: {key}");
            }
            else
            {
                _log.Error($"Resource not found: {key}");
                return $"Resource not found: {key}";
            }
        }

        return description.ToString()!;
    }
    #endregion Get a resource string

    #region Get composite format for a resource string
    private static CompositeFormat GetCompositeResource(string key)
    {
        try
        {
            return CompositeFormat.Parse(GetStringResource(key));
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error creating composite format for key: {key}");
            return CompositeFormat.Parse($"Error creating composite format for key: {key}");
        }
    }
    #endregion Get composite format for a resource string

    #region Composite format properties
    internal static CompositeFormat HardwareInfoUptimeString { get; } = GetCompositeResource("HardwareInfo_UptimeString");
    internal static CompositeFormat MsgTextAppUpdateNewerFound { get; } = GetCompositeResource("MsgText_AppUpdateNewerFound");
    internal static CompositeFormat MsgTextErrorOpeningFile { get; } = GetCompositeResource("MsgText_ErrorOpeningFile");
    internal static CompositeFormat MsgTextErrorReadingFile { get; } = GetCompositeResource("MsgText_ErrorReadingFile");
    internal static CompositeFormat MsgTextFilterRowsShown { get; } = GetCompositeResource("MsgText_FilterRowsShown");
    internal static CompositeFormat MsgTextUIColorSet { get; } = GetCompositeResource("MsgText_UIColorSet");
    internal static CompositeFormat MsgTextUISizeSet { get; } = GetCompositeResource("MsgText_UISizeSet");
    internal static CompositeFormat MsgTextUIThemeSet { get; } = GetCompositeResource("MsgText_UIThemeSet");
    #endregion Composite format properties

    #region Compute percentage of language strings
    /// <summary>
    /// Compute percentage of strings by dividing the number of strings
    /// for the supplied language by the total of en-US strings.
    /// </summary>
    /// <param name="language">Language code</param>
    /// <returns>The percentage with no decimal places as a string. Includes the "%".</returns>
    public static string GetLanguagePercent(string language)
    {
        ResourceDictionary dictionary = new()
        {
            Source = new Uri($"Languages/Strings.{language}.xaml", UriKind.RelativeOrAbsolute)
        };
        double percent = (double)dictionary.Count / TotalCount;
        return percent.ToString("P0", CultureInfo.InvariantCulture);
    }
    #endregion Compute percentage of language strings

    #region Properties
    private static int _totalCount;
    private static int TotalCount
    {
        get
        {
            if (_totalCount == 0)
            {
                _totalCount = GetTotalDefaultLanguageCount();
            }
            return _totalCount;
        }
        set => _totalCount = value;
    }
    #endregion Properties
}
