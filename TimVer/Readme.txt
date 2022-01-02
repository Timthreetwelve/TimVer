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
Use the bar on the left for page navigation.

The Windows Info page shows Windows operating system information such as the version, build number
and architecture.

The Computer Info page shows information about the computer such as the computer make, model and name.
It also shows information about the CPU, memory and optionally the disk drive letters.

The information on these first two pages can be copied to the clipboard by clicking on the icon on
the right end if the page header (blue bar).

The History page shows history information from the previous times TimVer was run. This can be used
to compare build numbers before and after running Windows Update. Only one history record will be
recorded per build number. The history file is in CSV format.

The Options page has options that determine how the application looks and runs.

The About page displays version information and a link to the GitHib repository.

These last two pages have a three-dot menu at the right end of the page header that has options to
view the log file, open the readme file and exit the application.

These keyboard shortcuts are available:

	Ctrl + Tab = Select the next screen
	Ctrl + Comma = Go to the Settings screen
	Ctrl + M = Change the theme
	Ctrl + Numpad Plus = Increase size
	Ctrl + Numpad Minus = Decrease size
	Ctrl + Mouse Wheel = Increase/Decrease size
	F1 = Go to the About screen

Command Line Option
===================
Specifying the /hide option on the command line will start TimVer, update the history file and then
shutdown without showing the window. This option is intended to be used in automation after reboot
to keep a more accurate history file. When checked, the "Update history on Windows startup"" option
on the Options page will add a key to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\ that will
run TimVer with the /hide option every time Windows starts. Unchecking this option will remove the key.


Uninstalling
============
To uninstall use the regular Windows add/remove programs feature.


Notices and License
===================
TimVer was written in C# by Tim Kennedy and now requires .Net 6.

TimVer uses the following NuGet packages:

* Material Design in XAML Toolkit https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

* NLog https://nlog-project.org/

* CSVHelper https://joshclose.github.io/CsvHelper/

* Microsoft.Management.Infrastructure https://github.com/PowerShell/MMI

* Inno Setup was used to create the installer. https://jrsoftware.org/isinfo.php



MIT License
Copyright (c) 2019 - 2022 Tim Kennedy

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
