using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class RecognizeView : UserControl
    {
        private readonly RecognizeViewModel _viewModel;

        public RecognizeView()
        {
            InitializeComponent();
            _viewModel =
                Design.IsDesignMode
                ? new DesignTimeRecognizeViewModel()
                : App.ServiceProvider.GetRequiredService<RecognizeViewModel>();
            DataContext = _viewModel;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _viewModel.NotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this)!);
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

        private int itemCount = 0;

        private void LogContentsRepeater_SizeChanged(object? sender, SizeChangedEventArgs args)
        {
            if (args.HeightChanged)
            {
                var newItemCount = _viewModel.LogContents!.Count;
                if (newItemCount != itemCount)
                {
                    itemCount = newItemCount;
                    LogScrollViewer.ScrollToEnd();
                }

            }

        }
    }
}
