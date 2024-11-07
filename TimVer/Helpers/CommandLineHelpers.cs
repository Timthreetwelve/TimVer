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
        CommandLineParser.CommandLineParser parser = new();
        SwitchArgument hideArgument = new('h',
                                                      "hide",
                                                      "To hide or not to hide, that is the question.",
                                                      false);
        parser.Arguments.Add(hideArgument);
        parser.AcceptSlash = true;
        parser.IgnoreCase = true;

        try
        {
            parser.ParseCommandLine(App.Args);
        }
        catch (UnknownArgumentException e)
        {
            CommandLineParserError = e.Message + e.StackTrace;
        }
        catch (Exception e)
        {
            CommandLineParserError = e.Message + e.StackTrace;
        }

        // Check options
        if (hideArgument.Value)
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

    /// <summary>
    /// Holds exception message if there is a parser error.
    /// </summary>
    public static string? CommandLineParserError { get; private set; }
    #endregion Properties
}
