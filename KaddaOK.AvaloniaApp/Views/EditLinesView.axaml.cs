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
    public interface IEditLinesView
    {
        bool Focus(NavigationMethod method = 0, KeyModifiers keyModifiers = 0);
    }

    public partial class EditLinesView : UserControl, IEditLinesView
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
                HandleWordEditKeys(e, false);
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
            if (HandleWordEditKeys(e, true))
            {
                var menuItemButton = menuItem?.FindLogicalAncestorOfType<Button>();
                menuItemButton?.Flyout?.Hide();
            }

            this.Focus();
        }

        public bool HandleWordEditKeys(KeyEventArgs keyArgs, bool isFromFlyout)
        {
            switch (keyArgs.Key)
            {
                case Key.Down:
                case Key.Right:
                case Key.Left:
                case Key.Up:
                    if (!isFromFlyout)
                    {
                        SwitchWordSelection(keyArgs);
                        return true;
                    }

                    return false;
                default:
                    return _viewModel.HandleWordEditKeys(keyArgs, isFromFlyout);
            }
        }

        private void SwitchWordSelection(KeyEventArgs keyArgs)
        {
            // this is just gonna change focus and let the handler set it back... wacky I know, but it makes
            // sense because of what we can and can't control around tabbing and such.
            // Also, apparently the order in which it finds these isn't reliable, so we'll have to sort it by the order in
            // memory...
            var allWordButtonsByPhrases = this.GetVisualDescendants().Where(x => x.Name == "PhraseWordsItemsControl")
                .Select(s => new
                {
                    Buttons = s.GetVisualDescendants().Where(y => y.Name == "LyricWordButton").Select(y => (Button)y).ToList(),
                    SortOrder = _viewModel.CurrentProcess.ChosenLines.IndexOf(s.DataContext as LyricLine)
                }).OrderBy(s => s.SortOrder)
                .Select(s => s.Buttons.OrderBy(x => _viewModel.CurrentProcess.ChosenLines[s.SortOrder].Words.IndexOf(x.CommandParameter as LyricWord)).ToList())
                .Where(s => s.Count > 0) // I'm not sure why this even happened; virtualization maybe
                .ToList();


            Button? destinationButton = null;
            if (_viewModel.CursorWord == null)
            {
                switch (keyArgs.Key)
                {
                    case Key.Down:
                    case Key.Right:
                        destinationButton = allWordButtonsByPhrases.FirstOrDefault()?.FirstOrDefault();
                        break;
                    case Key.Left:
                        destinationButton = allWordButtonsByPhrases.FirstOrDefault()?.LastOrDefault();
                        break;
                    case Key.Up:
                        destinationButton = allWordButtonsByPhrases.LastOrDefault()?.FirstOrDefault();
                        break;
                }
            }
            else
            {
                var currentPhrase = allWordButtonsByPhrases.SingleOrDefault(phrase => phrase.Any(word => word.CommandParameter == _viewModel.CursorWord));
                if (currentPhrase != null)
                {
                    var currentPhraseIndex = allWordButtonsByPhrases.IndexOf(currentPhrase);
                    var currentWordButton = currentPhrase.FirstOrDefault(word => word.CommandParameter == _viewModel.CursorWord);
                    var currentWordIndex = currentPhrase.IndexOf(currentWordButton);
                    switch (keyArgs.Key)
                    {
                        case Key.Down:
                            if (allWordButtonsByPhrases.Count > currentPhraseIndex + 1)
                            {
                                var nextPhrase = allWordButtonsByPhrases[currentPhraseIndex + 1];
                                var nextWordIndex = nextPhrase.Count > currentWordIndex
                                    ? currentWordIndex
                                    : nextPhrase.Count - 1;
                                destinationButton = nextPhrase[nextWordIndex];
                            }

                            break;
                        case Key.Right:
                            if (currentWordIndex < currentPhrase.Count - 1)
                            {
                                destinationButton = currentPhrase[currentWordIndex + 1];
                            }

                            break;
                        case Key.Left:
                            if (currentWordIndex > 0)
                            {
                                destinationButton = currentPhrase[currentWordIndex - 1];
                            }

                            break;
                        case Key.Up:
                            if (currentPhraseIndex > 0)
                            {
                                var nextPhrase = allWordButtonsByPhrases[currentPhraseIndex - 1];
                                var nextWordIndex = nextPhrase.Count > currentWordIndex
                                    ? currentWordIndex
                                    : nextPhrase.Count - 1;
                                destinationButton = nextPhrase[nextWordIndex];
                            }

                            break;
                    }

                }
            }

            destinationButton?.BringIntoView();
            destinationButton?.Focus();
        }
    }
}
