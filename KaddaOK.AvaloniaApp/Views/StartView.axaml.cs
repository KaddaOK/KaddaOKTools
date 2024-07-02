using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
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
    }
}
