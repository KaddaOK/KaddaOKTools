using Avalonia.Controls;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class NarrowingView : UserControl
    {
        private readonly NarrowingViewModel _viewModel;

        public NarrowingView()
        {
            InitializeComponent();
            _viewModel = Design.IsDesignMode
                ? new DesignTimeNarrowingViewModel()
                : App.ServiceProvider.GetRequiredService<NarrowingViewModel>();
            DataContext = _viewModel;
        }

        private void LayoutSizeChanged(object? sender, SizeChangedEventArgs args)
        {
            if (args.HeightChanged)
            {
                _viewModel.FullLengthVocalsDraw.DesiredImageWidth = (int)args.NewSize.Height;
                if (_viewModel.CurrentProcess?.VocalsAudioStream != null)
                {
                    _viewModel.FullLengthVocalsDraw.RedrawWaveform(_viewModel.CurrentProcess.VocalsAudioStream);
                }
            }
        }
    }
}
