using System;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.AvaloniaApp.Services;
using System.Reflection;
using KaddaOK.AvaloniaApp.Models;
using System.Diagnostics;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(KaraokeProcess karaokeProcess) : base(karaokeProcess)
        {
        }

        public string? AssemblyVersion
        {
            get
            {
                // Getting the informational version has proven surprisingly difficult when published in single file mode. 
                // It seems to want to just be the same as the numbers-only version, which is infuriating and not as designed.
                // So we have to resort to FileVersionInfo, which will not be very cross-platform I'm sure. ☹

                // First, we will try getting the KaddaOK.AvaloniaApp assembly, and see if it has a location.
                // In single-file mode, it won't.
                var avaloniaAppAssembly = Assembly.GetAssembly(typeof(AboutViewModel));

                // but we'll set a fallback value from it at least, marked with an asterisk so we know it's not complete
                var returnValue = @$"{avaloniaAppAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion 
                                        ?? avaloniaAppAssembly?.GetName().Version?.ToString()}*";

                var locationToLoad = avaloniaAppAssembly?.Location;
                if (string.IsNullOrWhiteSpace(locationToLoad))
                {
                    // we resort to asking nicely for "KaddaOKTools.exe", which probably isn't even the right thing on other platforms
                    locationToLoad = "KaddaOKTools.exe";
                }


                try
                {
                    // this would throw a FileNotFoundException so we'll have to do it in a try
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(locationToLoad);
                    if (!string.IsNullOrWhiteSpace(fileVersionInfo?.ProductVersion))
                    {
                        returnValue = fileVersionInfo.ProductVersion;
                    }
                }
                catch (Exception e)
                {
                    // actually don't care about this but we could log it for more information I guess
                }

                return returnValue;
            }
        }
            
            

        [RelayCommand]
        private void LinkToLantern()
        {
            UrlOpener.OpenUrl("https://lanternstudios.com/");
        }

        [RelayCommand]
        private void LinkToGithub()
        {
            UrlOpener.OpenUrl("https://github.com/KaddaOK/KaddaOKTools/");
        }
    }
}
