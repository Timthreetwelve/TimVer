// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using TKUtils;

namespace TimVer;

public class UserSettings : SettingsManager<UserSettings>, INotifyPropertyChanged
{
    #region Constructor
    public UserSettings()
    {
        // Set defaults
        GridZoom = 1;
        KeepOnTop = false;
        ShowUser = true;
        ShowUser = true;
        WindowLeft = 100;
        WindowTop = 100;
    }
    #endregion Constructor

    #region Properties

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
    public bool ShowUser
    {
        get => showUser;
        set
        {
            showUser = value;
            OnPropertyChanged();
        }
    }

    public double WindowLeft
    {
        get
        {
            if (windowLeft < 0)
            {
                windowLeft = 100;
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
                windowTop = 100;
            }
            return windowTop;
        }
        set => windowTop = value;
    }
    #endregion Properties

    #region Private backing fields
    private bool keepOnTop;
    private double gridZoom;
    private bool showDrives;
    private bool showUser;
    private double windowLeft;
    private double windowTop;
    #endregion Private backing fields

    #region Handle property change event
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion Handle property change event
}
