using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KaddaOK.AvaloniaApp.Services
{
    public class UrlOpener
    {
        /// <summary>
        /// Per https://github.com/AvaloniaUtils/HyperText.Avalonia/blob/master/HyperText.Avalonia/Extensions/OpenUrl.cs
        /// </summary>
        public static void OpenUrl(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //https://stackoverflow.com/a/2796367/241446
                using var proc = new Process { StartInfo = { UseShellExecute = true, FileName = url } };
                proc.Start();

                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("x-www-browser", url);
                return;
            }

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) throw new NotImplementedException("Platform not supported");
            Process.Start("open", url);
            return;
        }
    }
}
