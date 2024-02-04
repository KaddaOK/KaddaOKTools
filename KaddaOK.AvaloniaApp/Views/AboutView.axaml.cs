using Avalonia.Controls;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class AboutView : UserControl
    {
        private readonly AboutViewModel _viewModel;
        public AboutView()
        {
            InitializeComponent();
            _viewModel = Design.IsDesignMode
                ? new DesignTimeAboutViewModel()
                : App.ServiceProvider.GetRequiredService<AboutViewModel>();
            DataContext = _viewModel;
        }
    }
}
