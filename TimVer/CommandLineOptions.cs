// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

class CommandLineOptions
{
    [Option('h', "hide", Required = false)]
    public bool Hide { get; set; }
}
