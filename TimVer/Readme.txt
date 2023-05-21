The TimVer ReadMe file


Introduction
============
TimVer will display the same Windows version related information as the built-in Winver command,
with additional information that Winver doesn't display.


How TimVer Works
================
The information TimVer displays is obtained from the Environment, Windows Management Information
(WMI) and from the Windows registry at HKLM\Software\Microsoft\Windows NT\CurrentVersion.


The Pages
=========
Use the menu on the left for page navigation.

The Windows Info page shows Windows operating system information such as the version, build number
and architecture.

The Computer Info page shows information about the computer such as the computer make, model and name.
It also shows information about the CPU, memory and optionally the disk drive letters.

The Environment page shows the current environmental variables.

The History page shows history information from the previous times TimVer was run. This can be used
to compare build numbers before and after running Windows Update. Only one history record will be
recorded per build number. The history file is now in JSON format.

The Setting page has options that determine how the application looks and runs.

The About page displays version information and a link to the GitHib repository.

The three-dot menu at the right end of the page header that has options to
view the log file, open the readme file and exit the application.

These keyboard shortcuts are available:

	Ctrl + Comma = Go to Settings
	Ctrl + C = Copy the current page to the clipboard
	Ctrl + M = Change the theme
	Ctrl + N = Change the accent color
	Ctrl + Numpad Plus = Increase size
	Ctrl + Numpad Minus = Decrease size
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

    * Microsoft.Management.Infrastructure https://github.com/PowerShell/MMI

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
