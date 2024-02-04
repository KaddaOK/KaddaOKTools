using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Media;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class ExportView : UserControl
    {
        private readonly ExportViewModel _viewModel;
        public ExportView()
        {
            InitializeComponent();
            _viewModel = Design.IsDesignMode 
                        ? new DesignTimeExportViewModel()   
                        : App.ServiceProvider.GetRequiredService<ExportViewModel>();
            DataContext = _viewModel;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _viewModel.NotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this)!);
        }

        private void UnsungTextColorButton_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty && e.NewValue is Color color)
            {
                _viewModel.CurrentProcess.UnsungTextColor = color;
            }
        }

        private void UnsungOutlineColorButton_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty && e.NewValue is Color color)
            {
                _viewModel.CurrentProcess.UnsungOutlineColor = color;
            }
        }

        private void SungTextColorButton_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty && e.NewValue is Color color)
            {
                _viewModel.CurrentProcess.SungTextColor = color;
            }
        }

        private void SungOutlineColorButton_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty && e.NewValue is Color color)
            {
                _viewModel.CurrentProcess.SungOutlineColor = color;
            }
        }

        private void BackgroundColorButton_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty && e.NewValue is Color color)
            {
                _viewModel.CurrentProcess.BackgroundColor = color;
            }
        }

        private void ProgressBarFillColorButton_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty && e.NewValue is Color color)
            {
                _viewModel.CurrentProcess.ProgressBarFillColor = color;
            }
        }

        private void ProgressBarOutlineColorButton_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == AvaloniaColorPicker.ColorButton.ColorProperty && e.NewValue is Color color)
            {
                _viewModel.CurrentProcess.ProgressBarOutlineColor = color;

                // I shouldn't have to do this, but without it, the preview doesn't update
                ProgressBarOutlinePreviewRect.InvalidateVisual();
            }
        }
    }
}
