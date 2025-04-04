﻿The TimVer ReadMe file


Introduction
============
TimVer will display the same Windows version related information as the built-in Winver command,
with additional information that Winver doesn't display. It can also display basic information
about the hardware including the CPU, disk drives, graphics, and memory.


How TimVer Works
================
The information TimVer displays is obtained from the Environment, Windows Management Information
(WMI) and from the Windows registry at HKLM\Software\Microsoft\Windows NT\CurrentVersion. All of
this information can be gathered and displayed without elevation.


Navigation
==========
Use the menu on the left for page navigation or to exit the application.


Windows Information
===================
The Windows Info page shows Windows operating system information such as the version, build number,
build branch, architecture and installation date.


Hardware Information
====================
The Hardware Info page shows information about the computer such as the computer make, model and name.
It also shows information about the processor, BIOS and physical memory. The last boot date and time
are also displayed.


Drive Information
=================
The Disk Drive Information pages shows information about logical drives and optionally physical drives.
Note that while obtaining logical drive information is usually quick, physical drive information can
take a significant amount of time. The time required for each display will depend on machine configuration
and options selected.


Graphics Information
====================
The Graphics (Video) Information page shows information about the graphics adapter (video card).
Information for each graphics adapter can be displayed if more than one graphics adapter is present.
Information displayed includes the adapter name, type and description. The current resolution, refresh
rate and adapter RAM are also displayed.


Environment Variables
=====================
The Environment page shows the current environmental variables. The Filter box at the top can be used
to filter the list by variable name.


Build History
=============
The Build History page shows history information from the previous times TimVer was run. This can
be used to compare build numbers before and after running Windows Update. Only one history record
will be recorded per build number. See the Command Line Option section below to automate this
process.

Build history is optional. It can be enabled or disabled in the Application Settings section on
the Settings page. If disabled, it will not appear on the Navigation menu.


Settings Page
=============
The Setting page has options that determine how the application looks and runs.

    Application Settings
    --------------------
    There are options to select the initial page shown, build history options, show the registered user on the Windows information page, and control the verbosity of the temp log file.

    Disk Drive Settings
    -------------------
    Here there are option for how disk space is displayed (GB or GiB), enabling the collection of physical drive information, drive types to display and columns to be displayed.

    UI settings
    -----------
    Here you will find options to set the theme (Light, Material Dark, Darker, or System), the UI size,
    the accent color, and row spacing. There are also options to start the application centered on the
    screen and to keep the window on top of other applications. You can choose to show or hide Exit in
    the navigation bar.

    Language Settings
    -----------------
    You can choose the language used for the user interface, provided that a translation has been
    contributed for that language. Checking the ""Use Windows display language"" check box will tell
    the app to use the language specified in the Windows settings, which will be used if a translation
    is available; otherwise, English (en-US) will be used. The drop-down allows you to choose a
    specific language from the list of defined languages. Changing the language will cause the
    application to restart.

    Settings File
    -------------
    This section includes options to Export, Import, Open and List the application's settings.
    
    The Export option saves the current settings to a JSON file in the location of your choice.
    
    The Import option will read and apply settings from a previous export file. Using this option
    will automatically restart Get My IP after importing.
    
    The Open option will open the current settings file with the default application associated with
    JSON files. Note that any modifications to the settings file made while Get My IP is running will
    be overwritten when the application is shut down,

    The List option will write the current settings to the log file. When making a bug report, choose
    this option prior to sending the application log.

About Page
==========
Selecting About will display the About dialog which shows information about the app such as the version
number and has a link to the GitHub repository. There is also a link to this read me file. You can also
check for new releases of this application by clicking the link at the bottom of the About page. At the
bottom of the About page there is a scrollable list of the people that contributed a translation to help
make TimVer available to more users.


Three-Dot Menu
==============
The three-dot menu at the right end of the page header that has options to view the log file, open the
readme file (this file), open the TimVer folder, check for an updated version of TimVer, and exit the application.


Keyboard Shortcuts
==================
These keyboard shortcuts are available:

    Ctrl + Comma = Go to Settings
    Ctrl + C = Copy the current page to the clipboard
    Ctrl + Numpad Plus or Ctrl + Plus = Increase size
    Ctrl + Numpad Minus or Ctrl + Minus = Decrease size
    Ctrl + Shift + C = Change the Accent Color
    Ctrl + Shift + F = Open File explorer in the application folder
    Ctrl + Shift + K = Compare current language to default (en-US)
    Ctrl + Shift + R = Change Row spacing
    Ctrl + Shift + S = Open the Settings file
    Ctrl + Shift + T = Change the Theme
    F1 = Go to the About screen


Command Line Option
===================
Specifying the --hide (or -h) option on the command line will start TimVer, update the history file
and then shutdown without showing the window. This option is intended to be used in automation after
reboot to keep a more accurate history file. When checked, the "Update history on Windows startup"
option on the Options page will add a key to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\
that will run TimVer with the --hide option every time Windows starts. Unchecking this option will
remove the key.


Uninstalling
============
If you have TimVer set to update history on Windows startup, disable that option before uninstalling.
To uninstall use the regular Windows add/remove programs feature.


Notices and License
===================
TimVer was written by Tim Kennedy.

TimVer uses the following packages:

    * Material Design in XAML Toolkit https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

    * CommandLineParser https://github.com/j-maly/CommandLineParser

    * Community Toolkit MVVM https://github.com/CommunityToolkit/dotnet

    * NLog https://nlog-project.org/

    * Microsoft.Management.Infrastructure https://www.nuget.org/packages/Microsoft.Management.Infrastructure/3.0.0

    * NerdBank.GitVersioning https://github.com/dotnet/Nerdbank.GitVersioning

    * OctoKit https://github.com/octokit/octokit.net

    * Vanara https://github.com/dahall/vanara

    * GitKraken was used for everything Git related. https://www.gitkraken.com/

    * Inno Setup was used to create the installer. https://jrsoftware.org/isinfo.php

    * Visual Studio Community was used throughout the development of TimVer. https://visualstudio.microsoft.com/vs/community/

    * XAML Styler is indispensable when working with XAML. https://github.com/Xavalon/XamlStyler

    * JetBrains ReSharper Command Line Tools were used for code analysis. https://www.jetbrains.com/resharper/features/command-line.html

    * And of course, the essential PowerToys https://github.com/microsoft/PowerToys



MIT License
Copyright (c) 2019 - 2024 Tim Kennedy

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
associated documentation files (the "Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject
to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
