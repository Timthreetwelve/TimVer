// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.Helpers;

static class CommandLineHelpers
{
    #region Process the command line
    /// <summary>
    /// Parse any command line options.
    /// </summary>
    /// <returns><c>true</c> if "hide" option was specified.</returns>
    public static bool ProcessCommandLine()
    {
        // Since this is not a console app, get the command line args from Environment
        string[] args = Environment.GetCommandLineArgs();

        // Parser settings
        Parser parser = new(s =>
        {
            s.CaseSensitive = false;
            s.IgnoreUnknownArguments = true;
        });

        // parses the command line. result object will hold the arguments
        ParserResult<CommandLineOptions> result = parser.ParseArguments<CommandLineOptions>(args);

        // Check options
        if (result?.Value.Hide == true)
        {
            _log.Debug("Command line argument \"hide\" specified.");
            UpdateHistoryOnly = true;
            return true;
        }
        return false;
    }
    #endregion Process the command line

    #region Properties
    public static bool UpdateHistoryOnly { get; private set; }
    #endregion Properties
}
