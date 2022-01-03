// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace TimVer;

/// <summary>
/// Class to open text files in the default application for the file type.
/// If there isn't a default, open the file in notepad.exe
/// </summary>
internal static class TextFileViewer
{
    #region Text file viewer
    /// <summary>
    /// Open the file in the default application
    /// </summary>
    /// <param name="txtfile">File to open</param>
    public static async Task<bool> ViewTextFile(string txtfile)
    {
        if (File.Exists(txtfile))
        {
            try
            {
                using Process p = new();
                p.StartInfo.FileName = txtfile;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.ErrorDialog = false;
                _ = p.Start();
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {
                    using Process p = new();
                    p.StartInfo.FileName = "notepad.exe";
                    p.StartInfo.Arguments = txtfile;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.ErrorDialog = false;
                    _ = p.Start();
                }
                else
                {
                    ErrorDialog ed = new();
                    ed.Message = $"Error reading \n{txtfile}\n{ex.Message}";
                    _ = await DialogHost.Show(ed, "dh1").ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog ed = new();
                ed.Message = $"Unable to start default application used to open\n{txtfile}\n{ex.Message}";
                _ = await DialogHost.Show(ed, "dh1").ConfigureAwait(true);
            }
        }
        else
        {
            Debug.WriteLine($">>> File not found: {txtfile}");
            ErrorDialog ed = new();
            ed.Message = $"File not found:\n{txtfile}";
            _ = await DialogHost.Show(ed, "dh1").ConfigureAwait(true);
        }

        return true;
    }
    #endregion Text file viewer
}
