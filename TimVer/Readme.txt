The TimVer ReadMe file

Introduction
============

TimVer will display the same Windows version related information as the built-in Winver command,
plus additional information. The information is displayed in a larger font, for those folks whose
eyes are just a little bit more experienced.


How TimVer Works
================

The information is obtained from Windows Management Information (WMI) and from the Windows
registry at HKLM\Software\Microsoft\Windows NT\CurrentVersion. The machine name is obtained from
the current environment.


The Pages
=========

When TimVer is started it will display the first of two pages of information about the Windows
operating system such as the version and build number.

The third page shows information from the previous time TimVer was run. This can be used to
compare build numbers before and after running Windows Update.

Pages can be selected either from the View menu or by pressing Ctrl + 1, 2 or 3.

The information on pages 1 and 2 can be copied to the clipboard by pressing Ctrl + C or by selecting
Copy to Clipboard from the File menu.

The font size may be adjusted by pressing Ctrl + NumPad+ or Ctrl + NumPad-.


Uninstalling
============

To uninstall use the regular Windows add/remove programs feature.  If you are not planning to reinstall
you may want to delete the files and folders in %localappdata%\T_K\TimVer*.


Notices and License
===================

TimVer was written in C# by Tim Kennedy. Graphics files were created by Tim Kennedy.


MIT License
Copyright (c) 2019 - 2020 Tim Kennedy

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