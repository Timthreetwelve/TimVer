// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Models;

class CommandLineOptions
{
    /// <summary>
    /// Option indicating whether or not to show the main window or record
    /// build history and quit without showing the window.
    /// </summary>
    [Option('h', "hide", Required = false)]
    public bool Hide { get; set; }
}
