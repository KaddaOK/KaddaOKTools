using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;

namespace KaddaOK.AvaloniaApp.ViewModels;

public abstract class ViewModelBase : ObservableBase
{
    private KaraokeProcess _currentProcess = null!;
    public KaraokeProcess CurrentProcess
    {
        get => _currentProcess;
        set => SetProperty(ref _currentProcess, value);
    }

    protected ViewModelBase(KaraokeProcess karaokeProcess)
    {
        CurrentProcess = karaokeProcess;
    }
}
