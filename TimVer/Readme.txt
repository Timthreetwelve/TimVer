The TimVer ReadMe file


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


About Page
==========
Selecting About will display the About dialog which shows information about the app such as the version
number and has a link to the GitHub repository. There is also a link to this read me file. You can also
check for new releases of this application by clicking the link at the bottom of the About page. At the
bottom of the About page there is a scrollable list of the people that contributed a language to help
make Windows Update Viewer available to more users.


Three-Dot Menu
==============
The three-dot menu at the right end of the page header that has options to view the log file, open the
readme file (this file) and exit the application.


Keyboard Shortcuts
==================
These keyboard shortcuts are available:

    Ctrl + Comma = Go to Settings
    Ctrl + C = Copy the current page to the clipboard
    Ctrl + Numpad Plus = Increase size
    Ctrl + Numpad Minus = Decrease size
    Ctrl + Shift + C = Change the Accent Color
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
To uninstall use the regular Windows add/remove programs feature.


Notices and License
===================
TimVer was written by Tim Kennedy.

TimVer uses the following packages:

    * Material Design in XAML Toolkit https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

    * Command Line Parser https://github.com/commandlineparser/commandline

    * Community Toolkit MVVM https://github.com/CommunityToolkit/dotnet

    * NLog https://nlog-project.org/

    * Microsoft.Management.Infrastructure https://www.nuget.org/packages/Microsoft.Management.Infrastructure/2.0.0

    * System.Management https://www.nuget.org/packages/System.Management/8.0.0

    * GitVersion https://github.com/GitTools/GitVersion

    * OctoKit https://github.com/octokit/octokit.net

    * Inno Setup was used to create the installer. https://jrsoftware.org/isinfo.php


MIT License
Copyright (c) 2019 - 2023 Tim Kennedy

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
