// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer.ViewModels;

public partial class AboutViewModel
{
    #region Constructor
    public AboutViewModel()
    {
        if (AnnotatedLanguageList.Count == 0)
        {
            AddNote();
        }
    }
    #endregion Constructor

    #region Relay Commands
    [RelayCommand]
    private static void ViewLicense()
    {
        string dir = AppInfo.AppDirectory;
        TextFileViewer.ViewTextFile(Path.Combine(dir, "License.txt"));
    }

    [RelayCommand]
    private static void ViewReadMe()
    {
        string dir = AppInfo.AppDirectory;
        TextFileViewer.ViewTextFile(Path.Combine(dir, "ReadMe.txt"));
    }

    [RelayCommand]
    private static void GoToGitHub(string url)
    {
        Process p = new();
        p.StartInfo.FileName = url;
        p.StartInfo.UseShellExecute = true;
        p.Start();
    }

    [RelayCommand]
    private static async Task CheckReleaseAsync()
    {
        await GitHubHelpers.CheckRelease();
    }
    #endregion Relay Commands

    #region Annotated Language translation list
    public List<UILanguage> AnnotatedLanguageList { get; } = [];
    #endregion Annotated Language translation list

    #region Add note to list of languages
    private void AddNote()
    {
        try
        {
            foreach (UILanguage item in UILanguage.DefinedLanguages)
            {
                // en-GB is a special case and therefore should not be listed.
                // See the comments in Languages\Strings.en-GB.xaml.
                if (item.LanguageCode is not "en-GB")
                {
                    item.Note = GetLanguagePercent(item.LanguageCode!);
                    AnnotatedLanguageList.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Error in AddNote");
        }
    }
    #endregion Add note to list of languages
}
