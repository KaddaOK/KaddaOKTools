using Avalonia.Controls;
using DialogHostAvalonia;
using KaddaOK.AvaloniaApp.ViewModels;
using Avalonia.Input;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class EditLinesView : UserControl
    {
        private readonly EditLinesViewModel _viewModel;
        public EditLinesView()
        {
            InitializeComponent();
            _viewModel =
                Design.IsDesignMode
                ? new DesignTimeEditLinesViewModel()
                : App.ServiceProvider.GetRequiredService<EditLinesViewModel>();
            DataContext = _viewModel;
        }

        private void LayoutSizeChanged(object? sender, SizeChangedEventArgs args)
        {
            if (args.HeightChanged)
            {
                _viewModel.FullLengthVocalsDraw.DesiredImageWidth = (int)args.NewSize.Height;
                if (_viewModel.CurrentProcess?.VocalsAudioStream != null)
                {
                    _viewModel.FullLengthVocalsDraw.RedrawWaveform(_viewModel.CurrentProcess?.VocalsAudioStream);
                }
            }
        }

        private void EditLinesViewDialog_Closing(object? sender, DialogClosingEventArgs e)
        {
            if (e.Parameter == null)
            {
                return;
            }

            // returning from timing dialog
            if (e.Parameter is EditingLine)
            {
                _viewModel.ApplyLineTimingEdit(e.Parameter);
            }
        }

        private void EditLinesView_OnKeyDown(object? sender, KeyEventArgs e)
        {
            _viewModel.KeyPressed(e);
        }
    }
}
