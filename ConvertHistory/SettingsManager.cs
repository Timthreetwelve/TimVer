// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace ConvertHistory;

/// <summary>
/// A class and methods for reading, updating and saving user settings in a JSON file
/// </summary>
/// <typeparam name="T">Class name of user settings</typeparam>
public abstract class SettingsManager<T> where T : SettingsManager<T>, new()
{
    #region Properties
    public static T Setting { get; private set; }
    #endregion Properties

    #region Initialization
    /// <summary>
    ///  Initialization method. Gets the file name for settings file and creates it if it
    ///  doesn't exist. Optionally loads the settings file.
    /// </summary>
    /// <param name="folder">Folder name can be a path or one of the const values</param>
    /// <param name="fileName">File name can be a file name (without path) or DEFAULT</param>
    /// <param name="load">Read and load the settings file during initialization</param>
    public static void Init()
    {
        Setting = new T();
    }
    #endregion Initialization

}
