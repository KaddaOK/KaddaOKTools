using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using KaddaOK.Library;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class LyricsView : UserControl
    {
        private readonly LyricsViewModel _viewModel;

        public LyricsView()
        {
            InitializeComponent();
            _viewModel = Design.IsDesignMode
                        ? new DesignTimeLyricsViewModel() 
                        : App.ServiceProvider.GetRequiredService<LyricsViewModel>();
            DataContext = _viewModel;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _viewModel.NotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this)!);
        }

        private void LyricInputEditor_TextChanged(object sender, TextChangedEventArgs e)
        {

            _viewModel.CurrentProcess.KnownOriginalLyrics = KnownOriginalLyrics.FromText(LyricInputEditor.Text);
            _viewModel.PhrasesText = _viewModel.CurrentProcess.KnownOriginalLyrics.DistinctLinesAsText;
        }
    }
}
