// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using TimVer.ViewModels;
using TinyCsvParser.Mapping;

namespace TimVer
{
    internal class CsvHistoryMapping : CsvMapping<HistoryViewModel>
    {
        public CsvHistoryMapping()
        {
            MapProperty(0, x => x.HDate);
            MapProperty(1, x => x.HBuild);
            MapProperty(2, x => x.HVersion);
            MapProperty(3, x => x.HBranch);
        }
    }
}
