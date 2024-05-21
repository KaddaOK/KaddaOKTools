using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using DialogHostAvalonia;
using KaddaOK.AvaloniaApp.ViewModels;
using Avalonia.Input;
using Avalonia.LogicalTree;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.ViewModels.DesignTime;
using KaddaOK.Library;
using Microsoft.Extensions.DependencyInjection;
using Avalonia.VisualTree;

namespace KaddaOK.AvaloniaApp.Views
{
    public partial class EditLinesView : UserControl
    {
        private readonly EditLinesViewModel _viewModel;
        private ItemsRepeater? _phrasesRepeater;
        public EditLinesView()
        {
            InitializeComponent();
            _viewModel =
                Design.IsDesignMode
                ? new DesignTimeEditLinesViewModel()
                : App.ServiceProvider.GetRequiredService<EditLinesViewModel>();
            DataContext = _viewModel;
            _viewModel.EditLinesView = this;
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
                this.Focus();
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
            if (!this.FindDescendantOfType<DialogHost>()?.IsOpen ?? false) // leave it alone if dialogs are open
            {
                _viewModel.HandleWordEditKeys(e, false);
            }
        }

        private void EditLinesView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            this.Focus();
            _phrasesRepeater = this.FindNameScope().Find<ItemsRepeater>("PhrasesRepeater");
        }

        private void WordButton_GotFocus(object? sender, GotFocusEventArgs e)
        {
            var lyricWordButton = sender as Button;
            _viewModel.CursorWord = lyricWordButton?.CommandParameter as LyricWord;
            var itemsControl = lyricWordButton?.FindLogicalAncestorOfType<ItemsControl>();
            _viewModel.CursorLine = itemsControl?.DataContext as LyricLine;
        }

        private void LyricWordButtonFlyoutMenuItem_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            // it's a bit strange to have to do it this way, but these MenuItems are refusing to respond to HotKeys 
            var menuItem = sender as MenuItem;
            if (_viewModel.HandleWordEditKeys(e, true))
            {
                var menuItemButton = menuItem?.FindLogicalAncestorOfType<Button>();
                menuItemButton?.Flyout?.Hide();
            }

            this.Focus();
        }
    }
}
