using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class ManualAlignView : UserControl
    {
        private readonly ManualAlignViewModel _viewModel;
        private ItemsRepeater? _timingLinesRepeater;

        public ManualAlignView()
        {
            InitializeComponent();
            _viewModel =
                Design.IsDesignMode
                ? new DesignTimeManualAlignViewModel()
                : App.ServiceProvider.GetRequiredService<ManualAlignViewModel>();
            DataContext = _viewModel;

            _timingLinesRepeater = this.FindControl<ItemsRepeater>("TimingLinesRepeater");

            WeakReferenceMessenger.Default.Register<ScrollIntoViewMessage>(this, (recipient, message) =>
            {
                var sourceLines = (ObservableCollection<ManualTimingLine>?)_timingLinesRepeater?.ItemsSource;
                if (sourceLines != null)
                {
                    var lineToScrollTo = sourceLines.FirstOrDefault(l => l.Words.Contains(message.Item));
                    if (lineToScrollTo != null)
                    {
                        var indexToScrollTo = sourceLines.IndexOf(lineToScrollTo);
                        var correspondingWordsRepeater = _timingLinesRepeater?.TryGetElement(indexToScrollTo);
                        if (correspondingWordsRepeater != null)
                        {
                            correspondingWordsRepeater.BringIntoView();
                        }
                    }
                }
            });
        }

        private void LayoutSizeChanged(object? sender, SizeChangedEventArgs args)
        {
            if (args.WidthChanged)
            {
                _viewModel.FullLengthVocalsDraw.DesiredImageWidth = (int)args.NewSize.Width;
                if (_viewModel.CurrentProcess.VocalsAudioStream != null)
                {
                    Dispatcher.UIThread.Invoke(() =>
                        _viewModel.FullLengthVocalsDraw.RedrawWaveform(_viewModel.CurrentProcess?.VocalsAudioStream)
                        )
                        ;
                }
            }
        }

        private void SeekBar_PointerPressed(object? sender, PointerPressedEventArgs args)
        {
            _viewModel.SeekBarPointerPressed(sender, args);
        }

        private void SeekBar_PointerMoved(object? sender, PointerEventArgs args)
        {
            _viewModel.SeekBarPointerMoved(sender, args);
        }

        private void SeekBar_PointerReleased(object? sender, PointerReleasedEventArgs args)
        {
            _viewModel.SeekBarPointerReleased(sender, args);
        }

        private void ManualAlign_KeyDown(object? sender, KeyEventArgs e)
        {
            _viewModel?.ManualAlignKeyDown(sender, e);
        }
    }
}
