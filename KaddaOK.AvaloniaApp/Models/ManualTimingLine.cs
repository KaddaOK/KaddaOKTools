using KaddaOK.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace KaddaOK.AvaloniaApp.Models
{
    public class ManualTimingLine : ObservableBase, ILyricLine<TimingWord>
    {
        public string? Text => string.Concat(Words.Select(w => w.Text));

        private ObservableCollection<TimingWord> words;
        public ObservableCollection<TimingWord> Words
        {
            get => words;
            set
            {
                if (words?.Any() ?? false)
                {
                    foreach (var word in words)
                    {
                        word.PropertyChanged -= Word_PropertyChanged;
                    }
                }

                SetProperty(ref words, value);

                if (words != null)
                {
                    foreach (var word in words)
                    {
                        word.PropertyChanged += Word_PropertyChanged;
                    }
                }
                RaisePropertyChanged(nameof(ManualStartSecond));
                RaisePropertyChanged(nameof(ManualEndSecond));
            }
        }

        public ManualTimingLine(IEnumerable<TimingWord> words)
        {
            Words = new ObservableCollection<TimingWord>(words);
            foreach (var word in Words)
            {
                word.PropertyChanged += Word_PropertyChanged;
            }
        }

        public ManualTimingLine()
        {
            words = new ObservableCollection<TimingWord>();
        }

        private void Word_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ManualStartSecond));
            RaisePropertyChanged(nameof(ManualEndSecond));
        }

        public double? ManualStartSecond
        {
            get
            {
                var manualWords = Words?.Where(w => w.StartHasBeenManuallySet).ToList();
                if (manualWords?.Any() ?? false)
                {
                    return Math.Round(manualWords.Select(w => w.StartSecond).Min(), 2);
                }

                return null;
            }
        }

        public double? ManualEndSecond
        {
            get
            {
                var manualWords = Words?.Where(w => w.EndHasBeenManuallySet).ToList();
                if (manualWords?.Any() ?? false)
                {
                    return Math.Round(manualWords.Select(w => w.EndSecond).Max(), 2);
                }

                return null;
            }
        }

        public static LyricLine ToLyricLine(ManualTimingLine manualTimingLine)
        {
            return new LyricLine
            {
                Words = new ObservableCollection<LyricWord>(manualTimingLine.Words)
            };
        }
    }
}
