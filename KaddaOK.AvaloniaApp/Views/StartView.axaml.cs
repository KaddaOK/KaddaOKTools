using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DialogHostAvalonia;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class StartView : UserControl
    {
        private StartViewModel _viewModel;
        public StartView()
        {
            InitializeComponent();

            _viewModel = Design.IsDesignMode
                ? new DesignTimeStartViewModel()
                : App.ServiceProvider.GetRequiredService<StartViewModel>();
            DataContext = _viewModel;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _viewModel.NotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this)!);
        }

        private async void LocateAudioButton_Click(object? sender, RoutedEventArgs e)
        {
            if (App.MainWindow == null) return;

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Locate the audio file",
                FileTypeFilter = new FilePickerFileType[] { new ("Audio files")
                {
                    Patterns = new[] { "*.wav", "*.flac", "*.mp3", "*.ogg", "*.wma" }
                } }
            };

            var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
            var result = results?.FirstOrDefault();
            if (result != null)
            {
                var filePath = result.TryGetLocalPath();
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    DialogHost.Close("StartViewDialogHost", filePath);
                }
            }
        }
    }
}
