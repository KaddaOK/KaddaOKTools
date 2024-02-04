using KaddaOK.Library;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class NarrowingViewModel : DrawsFullLengthVocalsBase
    {
        public NarrowingViewModel(KaraokeProcess karaokeProcess) : base(karaokeProcess)
        {
            FullLengthVocalsDraw = new WaveformDraw
            {
                DesiredPeakHeight = 100,
                VerticalImage = true
            };
        }

        [RelayCommand]
        private void SetPossibleLineSelected(ReadOnlyCollection<object>? parameter)
        {
            if (parameter?[0] is LyricLine lyricLine)
            {
                if (lyricLine.IsSelected ?? false) // toggle off
                {
                    lyricLine.IsSelected = false;
                    if (lyricLine.InPossibilities?.SelectedLyric == lyricLine)
                    {
                        lyricLine.InPossibilities.SelectedLyric = null;
                    }
                }
                else // toggle on
                {
                    lyricLine.IsSelected = true;
                    if (lyricLine.InPossibilities != null)
                    {
                        lyricLine.InPossibilities.SelectedLyric = lyricLine;
                        lyricLine.InPossibilities.IsIgnored = false;
                        foreach (var otherLyric in lyricLine.InPossibilities.Lyrics.Where(l => l != lyricLine))
                        {
                            otherLyric.IsSelected = false;
                        }
                    }
                    // collapse the expander
                    if (parameter[1] is Expander expander) expander.IsExpanded = false;
                }

                // either way, we've made a change and need to update the collection
                CurrentProcess!.NarrowingStepCompletenessChanged();
            }
        }

        [RelayCommand]
        private void SetLineIgnored(ReadOnlyCollection<object>? parameter)
        {
            if (parameter?[0] is LinePossibilities possibilities)
            {
                if (possibilities.IsIgnored) // toggle off
                {
                    possibilities.IsIgnored = false;

                    // expand the expander
                    if (parameter[1] is Expander expander && !possibilities.HasSelected)
                    {
                        expander.IsExpanded = true;
                    }
                }
                else // toggle on
                {
                    possibilities.IsIgnored = true;
                    // collapse the expander
                    if (parameter[1] is Expander expander) expander.IsExpanded = false;
                }

                // either way, we've made a change and need to update the collection
                CurrentProcess!.NarrowingStepCompletenessChanged();
            }
        }

        [RelayCommand]
        public void GoToNextStep(object? parameter)
        {
            CurrentProcess!.SetChosenLinesToSelectedPossibilities();
            CurrentProcess!.SelectedTabIndex = 4;
        }
    }
}
