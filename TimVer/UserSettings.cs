// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

public class UserSettings : SettingsManager<UserSettings>, INotifyPropertyChanged
{
    #region Constructor
    public UserSettings()
    {
        // Set defaults
        IncludeDebug = false;
        KeepOnTop = false;
        ShowDrives = true;
        ShowLabels = false;
        ShowUser = true;
        windowHeight = 400;
        WindowLeft = 100;
        WindowTop = 100;
        WindowWidth = 600;
    }
    #endregion Constructor

    #region Properties

    public bool IncludeDebug
    {
        get => includeDebug;
        set
        {
            includeDebug = value;
            OnPropertyChanged();
        }
    }

    public bool KeepOnTop
    {
        get => keepOnTop;
        set
        {
            keepOnTop = value;
            OnPropertyChanged();
        }
    }

    public double GridZoom
    {
        get
        {
            if (gridZoom <= 0)
            {
                gridZoom = 1;
            }
            return gridZoom;
        }
        set
        {
            gridZoom = value;
            OnPropertyChanged();
        }
    }

    public bool ShowDrives
    {
        get => showDrives;
        set
        {
            showDrives = value;
            OnPropertyChanged();
        }
    }

    public bool ShowLabels
    {
        get => showLabels;
        set
        {
            showLabels = value;
            OnPropertyChanged();
        }
    }

    public bool ShowUser
    {
        get => showUser;
        set
        {
            showUser = value;
            OnPropertyChanged();
        }
    }

    public double WindowHeight
    {
        get
        {
            if (windowHeight < 100)
            {
                windowHeight = 100;
            }
            return windowHeight;
        }
        set => windowHeight = value;
    }

    public double WindowLeft
    {
        get
        {
            if (windowLeft < 0)
            {
                windowLeft = 0;
            }
            return windowLeft;
        }
        set => windowLeft = value;
    }

    public double WindowTop
    {
        get
        {
            if (windowTop < 0)
            {
                windowTop = 0;
            }
            return windowTop;
        }
        set => windowTop = value;
    }

    public double WindowWidth
    {
        get
        {
            if (windowWidth < 100)
            {
                windowWidth = 100;
            }
            return windowWidth;
        }
        set => windowWidth = value;
    }
    #endregion Properties

    #region Private backing fields
    private bool includeDebug;
    private bool keepOnTop;
    private double gridZoom;
    private bool showDrives;
    private bool showLabels;
    private bool showUser;
    private double windowHeight;
    private double windowLeft;
    private double windowTop;
    private double windowWidth;
    #endregion Private backing fields

    #region Handle property change event
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion Handle property change event
}
