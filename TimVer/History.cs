// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using CsvHelper.Configuration.Attributes;

namespace TimVer;

public class History
{
    [Index(0)]
    public string HDate { get; set; }

    [Index(1)]
    public string HBuild { get; set; }

    [Index(2)]
    public string HVersion { get; set; }

    [Index(3)]
    public string HBranch { get; set; }
}
