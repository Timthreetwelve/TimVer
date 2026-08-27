// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Configuration;

/// <summary>
/// Class for the static Setting property
/// </summary>
/// <typeparam name="T">Class name of user settings</typeparam>
public abstract class ConfigManager<T> where T : ConfigManager<T>, new()
{
    public static T Setting
    {
        get => field ?? throw new InvalidOperationException($"{typeof(T).Name}.Setting is not initialized.");

        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }
}
