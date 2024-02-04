using CommunityToolkit.Mvvm.Input;
using KaddaOK.AvaloniaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(KaraokeProcess karaokeProcess) : base(karaokeProcess)
        {
        }

        public string? AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString();

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
