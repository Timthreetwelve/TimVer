// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Configuration;

/// <summary>
/// Class for the static Setting property
/// </summary>
/// <typeparam name="T">Class name of user settings</typeparam>
public abstract class ConfigManager<T> where T : ConfigManager<T>, new()
{
    private static readonly bool _isDesignMode =
        DesignerProperties.GetIsInDesignMode(new DependencyObject());

    public static T Setting
    {
        get
        {
            if (field is not null)
            {
                return field;
            }

            // If in design mode, create a new instance of T to avoid issues in the XAML designer.
            if (_isDesignMode)
            {
                field = new T();
                return field;
            }

            throw new InvalidOperationException($"ConfigManager: {typeof(T).Name}.Setting is not initialized.");
        }

        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }
}
