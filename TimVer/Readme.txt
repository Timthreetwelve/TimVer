The TimVer ReadMe file


Introduction
============
TimVer will display the same Windows version related information as the built-in Winver command,
with additional information that Winver doesn't display.


How TimVer Works
================
The information TimVer displayed is obtained from Windows Management Information (WMI) and from the
Windows registry at HKLM\Software\Microsoft\Windows NT\CurrentVersion.


The Pages
=========
When TimVer is started it will display information about the Windows operating system such as
the version and build number.

The second page shows information about the computer such as the computer name and the last time
it booted up.

The third page shows history information from the previous times TimVer was run. This can be used
to compare build numbers before and after running Windows Update. Only one history record will be
recorded per build number. The history file is in CSV format.

The pages can be selected either from the Views menu or by pressing Ctrl + 1, 2 or 3.

The information on the first two pages can be copied to the clipboard by pressing Ctrl + C or by
selecting Copy to Clipboard from the File menu.

The window can be zoomed in and out by using the NumPad + and NumPad - keys or by holding the Ctrl
key and using the mouse wheel. The zoom can be reset by pressing the NumPad 0 key


Command Line Option
===================
Specifying the /hide option on the command line will start TimVer, update the history file and then
shutdown without showing the window. This option is intended to be used in automation after reboot
to keep a more accurate history file. When checked, the Update History on Windows Startup option on
the File menu will add a key to HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\ that will run
TimVer with the /hide option everytime Windows starts. Unchecking this option will remove the key.


Uninstalling
============
To uninstall use the regular Windows add/remove programs feature.


Notices and License
===================
TimVer was written in C# by Tim Kennedy. TimVer requires .Net Framework version 4.8.

TimVer uses the following icons & packages:

Icons are from the Fugue Icons set https://p.yusukekamiyamane.com/

NLog 4.7.12 https://nlog-project.org/

Json.net 13.0.1 from Newtonsoft https://www.newtonsoft.com/json

TinyCsvParser 2.6.1 from Philipp Wagner https://github.com/bytefish/TinyCsvParser

Inno Setup 6.2.0 was used to create the installer. https://jrsoftware.org/isinfo.php

MIT License
Copyright (c) 2019 - 2021 Tim Kennedy

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
