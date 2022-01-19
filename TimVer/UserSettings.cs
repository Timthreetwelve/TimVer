// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using MaterialDesignColors;

namespace TimVer;

public class UserSettings : SettingsManager<UserSettings>, INotifyPropertyChanged
{
    #region Constructor
    public UserSettings()
    {
        // Set defaults
        DarkMode = 0;
        IncludeDebug = false;
        InitialPage = 0;
        KeepOnTop = false;
        Page1Alt = false;
        ShowDrives = true;
        ShowLabels = false;
        ShowUser = true;
        UISize = 2;
        WindowHeight = 480;
        WindowLeft = 100;
        WindowTop = 100;
        WindowWidth = 645;
    }
    #endregion Constructor

    #region Properties
    public int DarkMode
    {
        get => darkmode;
        set
        {
            darkmode = value;
            OnPropertyChanged();
        }
    }

    public bool IncludeDebug
    {
        get => includeDebug;
        set
        {
            includeDebug = value;
            OnPropertyChanged();
        }
    }

    public int InitialPage
    {
        get => initialPage;
        set
        {
            initialPage = value;
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

    public bool Page1Alt
    {
        get => page1alt;
        set
        {
            page1alt = value;
            OnPropertyChanged();
        }
    }

    public int PrimaryColor
{
        get => primaryColor;
set
{
            primaryColor = value;
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

    public int UISize
    {
        get => uiSize;
        set
        {
            uiSize = value;
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
    private int darkmode;
    private bool includeDebug;
    private int initialPage;
    private bool keepOnTop;
    private bool page1alt;
    private bool showDrives;
    private bool showLabels;
    private bool showUser;
    private int primaryColor = 5;
    private int uiSize;
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
