using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class AudioView : UserControl
    {
        private AudioViewModel _viewModel;
        public AudioView()
        {
            InitializeComponent();

            _viewModel = Design.IsDesignMode
                ? new DesignTimeAudioViewModel()
                : App.ServiceProvider.GetRequiredService<AudioViewModel>();
            DataContext = _viewModel;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _viewModel.NotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this)!);
        }
    }
}
