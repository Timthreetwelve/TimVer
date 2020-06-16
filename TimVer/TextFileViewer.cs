using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace TimVer
{
    // Class to open text files in the default application for the file type.
    // If there isn't a default, open the file in notepad.exe
    internal static class TextFileViewer
    {
        #region Text file viewer
        public static void ViewTextFile(string txtfile)
        {
            if (File.Exists(txtfile))
            {
                try
                {
                    using (Process p = new Process())
                    {
                        p.StartInfo.FileName = txtfile;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.ErrorDialog = false;
                        _ = p.Start();
                    }
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 1155)
                    {
                        using (Process p = new Process())
                        {
                            p.StartInfo.FileName = "notepad.exe";
                            p.StartInfo.Arguments = txtfile;
                            p.StartInfo.UseShellExecute = true;
                            p.StartInfo.ErrorDialog = false;
                            _ = p.Start();
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show($"Error reading file {txtfile}\n{ex.Message}", "Watcher Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"Unable to start default application used to open" +
                        $" {txtfile}\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Debug.WriteLine($">>> File not found: {txtfile}");
            }
        }
        #endregion
    }

}
