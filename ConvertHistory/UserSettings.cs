// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace ConvertHistory;

public class UserSettings : SettingsManager<UserSettings>, INotifyPropertyChanged
{
    #region Properties
    public bool DarkMode
    {
        get => _darkMode;
        set
        {
            _darkMode = value;
            OnPropertyChanged();
        }
    }

    public bool IncludeDebug
    {
        get => _includeDebug;
        set
        {
            _includeDebug = value;
            OnPropertyChanged();
        }
    }
    #endregion Properties

    #region Private backing fields
    private bool _darkMode;
    private bool _includeDebug = true;
    #endregion Private backing fields

    #region Handle property change event
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion Handle property change event
}
