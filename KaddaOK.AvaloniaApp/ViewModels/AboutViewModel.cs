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

        public string? AssemblyVersion => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

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
