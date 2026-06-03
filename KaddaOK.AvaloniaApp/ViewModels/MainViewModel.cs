using System.ComponentModel;
using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string WindowTitle => CurrentProcess.HasUnsavedChanges
        ? "● KaddaOK Tools"
        : "KaddaOK Tools";

    public MainViewModel(KaraokeProcess karaokeProcess) : base(karaokeProcess)
    {
        CurrentProcess.PropertyChanged += OnProcessPropertyChanged;
    }

    private void OnProcessPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(KaraokeProcess.HasUnsavedChanges))
        {
            RaisePropertyChanged(nameof(WindowTitle));
        }
    }
}
