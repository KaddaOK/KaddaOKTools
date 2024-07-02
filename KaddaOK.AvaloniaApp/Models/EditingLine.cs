using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KaddaOK.Library;

namespace KaddaOK.AvaloniaApp.Models
{
    public class EditingLine : ObservableBase
    {
        private LyricLine originalLine = null!;
        public LyricLine OriginalLine
        {
            get => originalLine;
            set => SetProperty(ref originalLine, value);
        }

        private ObservableCollection<TimingWord> newTiming = null!;
        public ObservableCollection<TimingWord> NewTiming
        {
            get => newTiming;
            set => SetProperty(ref newTiming, value);
        }

        private bool isPlaying;
        public bool IsPlaying
        {
            get => isPlaying;
            set => SetProperty(ref isPlaying, value);
        }

        private bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set => SetProperty(ref isRecording, value);
        }

        private TimingWord? wordCurrentlyTiming;
        public TimingWord? WordCurrentlyTiming
        {
            get => wordCurrentlyTiming;
            set => SetProperty(ref wordCurrentlyTiming, value);
        }

        private ObservableQueue<TimingWord>? wordTimingQueue;
        public ObservableQueue<TimingWord>? WordTimingQueue
        {
            get => wordTimingQueue;
            set => SetProperty(ref wordTimingQueue, value);
        }

        public EditingLine(LyricLine line)
        {
            OriginalLine = line;
            NewTiming = new ObservableCollection<TimingWord>(
                line.Words?.Select(TimingWord.FromLyricWord) 
                ?? Array.Empty<TimingWord>());
        }
    }

    public class TimingWord : LyricWord
    {
        private bool hasFinished;
        public bool HasFinished
        {
            get => hasFinished;
            set => SetProperty(ref hasFinished, value);
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private bool isNext;
        public bool IsNext
        {
            get => isNext;
            set => SetProperty(ref isNext, value);
        }

        private bool startHasBeenManuallySet;
        public bool StartHasBeenManuallySet
        {
            get => startHasBeenManuallySet;
            set => SetProperty(ref startHasBeenManuallySet, value);
        }

        private bool endHasBeenManuallySet;
        public bool EndHasBeenManuallySet
        {
            get => endHasBeenManuallySet;
            set => SetProperty(ref endHasBeenManuallySet, value);
        }

        public static TimingWord FromLyricWord(LyricWord word)
        {
            return new TimingWord
            {
                Text = word.Text,
                EndSecond = Math.Round(word.EndSecond, 2),
                StartSecond = Math.Round(word.StartSecond, 2)
            };
        }

        public static List<TimingWord> GetTimingWordsAcrossTime(string? enteredText, double startTime, double endTime)
        {
            var lyricWords = LyricWord.GetLyricWordsAcrossTime(enteredText, startTime, endTime);
            return lyricWords.Select(TimingWord.FromLyricWord).ToList();
        }
    }
}
