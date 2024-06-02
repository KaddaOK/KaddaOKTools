using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.Library;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using KaddaOK.AvaloniaApp.Controls;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;
using KaddaOK.AvaloniaApp.Views;
using NAudio.Wave;
using NAudio.Utils;
using NAudio.Wave.SampleProviders;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class ManualAlignViewModel : TickableBase
    {
        private CancellationTokenSource AudioPlayingSource { get; }

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

        private bool movingCaret;
        public bool MovingCaret
        {
            get => movingCaret;
            set => SetProperty(ref movingCaret, value);
        }

        private double? caretPositionSeconds;
        public double? CaretPositionSeconds
        {
            get => caretPositionSeconds;
            set => SetProperty(ref caretPositionSeconds, value);
        }

        private string caretPositionText;
        public string CaretPositionText
        {
            get => caretPositionText;
            set => SetProperty(ref caretPositionText, value);
        }

        private double? currentOffsetSeconds;
        public double? CurrentOffsetSeconds
        {
            get => currentOffsetSeconds;
            set => SetProperty(ref currentOffsetSeconds, value);
        }

        private WaveOutEvent? playAudio;
        public WaveOutEvent? PlayAudio
        {
            get => playAudio;
            set => SetProperty(ref playAudio, value);
        }

        private double? _currentPlaybackPositionSeconds;
        public double? CurrentPlaybackPositionSeconds
        {
            get => _currentPlaybackPositionSeconds;
            set => SetProperty(ref _currentPlaybackPositionSeconds, value);
        }

        private string _currentPlaybackPositionText;
        public string CurrentPlaybackPositionText
        {
            get => _currentPlaybackPositionText;
            set => SetProperty(ref _currentPlaybackPositionText, value);
        }

        private TimingWord? wordCurrentlyTiming;
        public TimingWord? WordCurrentlyTiming
        {
            get => wordCurrentlyTiming;
            set => SetProperty(ref wordCurrentlyTiming, value);
        }

        private TimingWord? editingTextOfWord;
        public TimingWord? EditingTextOfWord
        {
            get => editingTextOfWord;
            set => SetProperty(ref editingTextOfWord, value);
        }

        private bool instructionsVisible;
        public bool InstructionsVisible
        {
            get => instructionsVisible;
            set => SetProperty(ref instructionsVisible, value);
        }

        [RelayCommand]
        public void HideInstructions(object? parameter)
        {
            InstructionsVisible = false;
        }
        [RelayCommand]
        public void ShowInstructions(object? parameter)
        {
            InstructionsVisible = true;
        }

        private ILineSplitter Splitter { get; }
        private IWordMerger WordMerger { get; }
        public ManualAlignViewModel(KaraokeProcess karaokeProcess, ILineSplitter splitter, IWordMerger merger) : base(karaokeProcess)
        {
            CurrentPlaybackPositionText = "0:00.00";
            FullLengthVocalsDraw = new WaveformDraw
            {
                DesiredPeakHeight = 75
            };
            Splitter = splitter;
            WordMerger = merger;
            Dispatcher.UIThread.Invoke(() => FullLengthVocalsDraw.RedrawWaveform(CurrentProcess!.VocalsAudioStream));
            AudioPlayingSource = new CancellationTokenSource();
        }

        [RelayCommand]
        private async Task EditWordText(object? parameter)
        {
            if (parameter is TimingWord editThisWord)
            {
                EditingTextOfWord = editThisWord;
                if (await DialogHost.Show(this, "ManualAlignViewDialogHost") is string newText)
                {
                    // TODO: AddUndoSnapshot($"Change syllable \"{editThisWord.Text}\" to \"{newText}\"");
                    ApplyEditWordText(newText);
                };
            }
        }

        public void ApplyEditWordText(string newText)
        {
            if (!string.IsNullOrWhiteSpace(newText)
                && EditingTextOfWord != null)
            {
                var newSyllables = TimingWord.GetTimingWordsAcrossTime(newText, EditingTextOfWord.StartSecond, EditingTextOfWord.EndSecond);

                EditingTextOfWord.Text = newSyllables[0].Text;
                EditingTextOfWord.EndSecond = newSyllables[0].EndSecond;
                if (newSyllables.Count > 1)
                {
                    var line = CurrentProcess.ManualTimingLines?.SingleOrDefault(l => l.Words?.Contains(EditingTextOfWord) ?? false);
                    var index = line?.Words?.IndexOf(EditingTextOfWord);
                    if (index == null)
                    {
                        // TODO: error message
                        throw new InvalidOperationException("Couldn't find the word we were editing.");
                    }
                    for (var i = 1; i < newSyllables.Count; i++)
                    {
                        line!.Words!.Insert(index.Value + i, newSyllables[i]);
                    }
                }
            }
        }

        [RelayCommand]
        private void DeleteWord(object? parameter)
        {
            if (parameter is TimingWord deleteThis)
            {
                // TODO: AddUndoSnapshot($"Delete syllable \"{deleteThis.Text}\"");

                CurrentProcess!.ManualTimingLines!.SingleOrDefault(l => l.Words != null && l.Words.Contains(deleteThis));

                Splitter.DeleteWord(CurrentProcess!.ManualTimingLines!, deleteThis);
            }
        }

        [RelayCommand]
        private void MergeWithPrev(object? parameter)
        {
            if (parameter is TimingWord mergeHere)
            {
                MergeWord(mergeHere, true);
            }
        }

        [RelayCommand]
        private void MergeWithNext(object? parameter)
        {
            if (parameter is TimingWord mergeHere)
            {
                MergeWord(mergeHere, false);
            }
        }

        private void MergeWord(TimingWord mergeHere, bool isBefore)
        {
            // TODO: AddUndoSnapshot($"Merge \"{mergeHere.Text}\" with {(isBefore ? "previous" : "next")} syllable");
            // TODO: WordMerger.MergeWord(CurrentProcess?.ManualTimingLines, mergeHere, isBefore);
        }

        [RelayCommand]
        public void GoToNextStep(object? parameter)
        {
            CurrentProcess.ChosenLines = new ObservableCollection<LyricLine>(CurrentProcess.ManualTimingLines.Select(ManualTimingLine.ToLyricLine));
            CurrentProcess!.SelectedTabIndex = (int)TabIndexes.Edit;
            CurrentProcess.CanExportFactorsChanged();
        }

        [RelayCommand]
        private void MoveHead(object? parameter)
        {
            if (parameter is TimingWord word)
            {
                // Find the ManualTimingLine that contains the TimingWord
                var line = CurrentProcess.ManualTimingLines?.FirstOrDefault(l => l.Words.Contains(word));

                if (line != null)
                {
                    // Find the index of the TimingWord in the Words collection
                    int wordIndex = line.Words.IndexOf(word);

                    // Find the index of the ManualTimingLine in ManualTimingLines
                    int lineIndex = CurrentProcess.ManualTimingLines.IndexOf(line);

                    // Create a new ObservableQueue and add the TimingWord and every word after it in the current line,
                    // and all words in the lines that come after the current line, to the queue
                    var queue = new ObservableQueue<TimingWord>();
                    for (int i = wordIndex; i < line.Words.Count; i++)
                    {
                        queue.Enqueue(line.Words[i]);
                    }

                    for (int i = lineIndex + 1; i < CurrentProcess.ManualTimingLines.Count; i++)
                    {
                        foreach (var timingWord in CurrentProcess.ManualTimingLines[i].Words)
                        {
                            queue.Enqueue(timingWord);
                        }
                    }

                    // clear old IsNext flag
                    if (CurrentProcess.ManualTimingQueue != null 
                        && CurrentProcess.ManualTimingQueue.TryPeek(out var oldIsNext))
                    {
                        oldIsNext.IsNext = false;
                    }

                    // Set ManualTimingQueue to the new queue
                    CurrentProcess.ManualTimingQueue = queue;

                    // Set new IsNext 
                    word.IsNext = true;

                    // We also want to move playback to a few seconds before the last recorded position of a word
                    double newPlaybackPosition = 0;
                    for (int i = lineIndex; i >= 0; i--)
                    {
                        var searchingLine = CurrentProcess.ManualTimingLines[i];
                        for (int w = searchingLine == line ? wordIndex : searchingLine.Words.Count - 1; w >= 0; w--)
                        {
                            var searchingWord = searchingLine.Words[w];
                            if (searchingWord.EndHasBeenManuallySet)
                            {
                                newPlaybackPosition = Math.Max(searchingWord.EndSecond - 3, 0);
                                break;
                            }
                        }

                        if (newPlaybackPosition > 0)
                        {
                            break;
                        }
                    }

                    Seek(newPlaybackPosition);
                }
            }
        }

        private void Play()
        {
            if (PlayAudio != null)
            {
                PlayAudio.Play();
                StartTicking();
            }
        }

        private void StartNextWord(double? positionTotalSeconds)
        {
            WordCurrentlyTiming = CurrentProcess.ManualTimingQueue.Dequeue();
            StartWord(WordCurrentlyTiming!, positionTotalSeconds.Value);

            if (CurrentProcess.ManualTimingQueue.TryPeek(out var nextWord))
            {
                WordIsNext(nextWord);
            }
        }

        private void StopWord(TimingWord word, double? endSecond)
        {
            word.IsRunning = false;
            word.IsNext = false;
            word.HasFinished = true;
            if (endSecond != null)
            {
                word.EndSecond = endSecond.Value;
                word.EndHasBeenManuallySet = true;
                CurrentProcess.ManualAlignmentCompletenessChanged();
            }
        }

        private void StartWord(TimingWord word, double? startSecond)
        {
            word.IsRunning = true;
            word.IsNext = false;
            word.HasFinished = false;
            if (startSecond != null)
            {
                word.StartSecond = startSecond.Value;
                word.StartHasBeenManuallySet = true;
            }
        }

        private void WordIsNext(TimingWord word)
        {
            word.IsRunning = false;
            word.IsNext = true;
            WeakReferenceMessenger.Default.Send(new ScrollIntoViewMessage(word));
        }

        protected override void Tick()
        {
            if (PlayAudio?.PlaybackState == PlaybackState.Playing)
            {
                var currentTimespan = PlayAudio.GetPositionTimeSpan();
                if ((CurrentOffsetSeconds ?? 0) > 0)
                {
                    currentTimespan += TimeSpan.FromSeconds(CurrentOffsetSeconds.Value);
                }

                CurrentPlaybackPositionSeconds = currentTimespan.TotalSeconds;
                CurrentPlaybackPositionText = currentTimespan.ToString("m\\:ss\\.ff");
                if (!MovingCaret)
                {
                    CaretPositionSeconds = CurrentPlaybackPositionSeconds;
                    CaretPositionText = CurrentPlaybackPositionText;
                }

                if (IsPlaying)
                {
                    if (!IsRecording)
                    {
                        void SetUpNextOrStop()
                        {
                            if (CurrentProcess.ManualTimingQueue.TryDequeue(out var nextWord))
                            {
                                WordCurrentlyTiming = nextWord;
                                if (WordCurrentlyTiming!.StartSecond <
                                    CurrentPlaybackPositionSeconds)
                                {
                                    StartWord(nextWord!, null);
                                }
                                else
                                {
                                    //WordIsNext(nextWord);
                                }
                            }
                            else
                            {
                                Stop(null);
                            }
                        }

                        if (WordCurrentlyTiming != null)
                        {
                            if (WordCurrentlyTiming.StartSecond < CurrentPlaybackPositionSeconds)
                            {
                                if (WordCurrentlyTiming.EndSecond <
                                    CurrentPlaybackPositionSeconds)
                                {
                                    StopWord(WordCurrentlyTiming, null);
                                    SetUpNextOrStop();
                                }
                                else
                                {
                                    StartWord(WordCurrentlyTiming, null);
                                    if (CurrentProcess.ManualTimingQueue.TryPeek(out var nextWord))
                                    {
                                        //WordIsNext(nextWord);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // TODO: SetUpNextOrStop();
                        }
                    }
                }

            }
            if (PlayAudio == null || PlayAudio.PlaybackState == PlaybackState.Stopped)
            {
                CurrentPlaybackPositionSeconds = null;
                CurrentOffsetSeconds = null;
            }
        }

        [RelayCommand]
        private void Pause(object? parameter)
        {
            AudioPlayingSource.Cancel();
            StopTicking();
            IsPlaying = false;

            if (PlayAudio != null && PlayAudio.PlaybackState == PlaybackState.Playing)
            {
                PlayAudio.Pause();
            }
        }

        [RelayCommand]
        private void Stop(object? parameter)
        {
            if (WordCurrentlyTiming != null && WordCurrentlyTiming.IsRunning)
            {
                if (WordCurrentlyTiming.StartHasBeenManuallySet && !WordCurrentlyTiming.EndHasBeenManuallySet)
                {
                    StopWord(WordCurrentlyTiming, CurrentPlaybackPositionSeconds);
                }

                WordCurrentlyTiming.IsRunning = false;
                if (WordCurrentlyTiming.StartHasBeenManuallySet && WordCurrentlyTiming.EndHasBeenManuallySet)
                {
                    WordCurrentlyTiming.HasFinished = true;
                }
            }

            AudioPlayingSource.Cancel();
            StopTicking();
            CurrentPlaybackPositionSeconds = null;
            CurrentOffsetSeconds = null;
            CaretPositionSeconds = null;
            CaretPositionText = "0:00.00";
            CurrentPlaybackPositionText = "0:00.00";
            IsRecording = false;
            IsRecording = false;
            IsPlaying = false;

            if (PlayAudio != null && PlayAudio.PlaybackState != PlaybackState.Stopped)
            {
                PlayAudio.Stop();
            }
        }

        [RelayCommand]
        private async Task Play(object? parameter)
        {
            if (PlayAudio?.PlaybackState != PlaybackState.Playing)
            {
                if (PlayAudio?.PlaybackState == PlaybackState.Paused)
                {
                    Play();
                    IsPlaying = true;
                }
                else
                {
                    (CurrentProcess.UnseparatedAudioStream ??
                     CurrentProcess.VocalsAudioStream)!.CurrentTime = TimeSpan.FromSeconds(CurrentPlaybackPositionSeconds ?? 0);

                    PlayAudio = new WaveOutEvent();

                    PlayAudio.Init(CurrentProcess.UnseparatedAudioStream ??
                                   CurrentProcess.VocalsAudioStream);
                    IsPlaying = true;
                    if (parameter is EditingLine lineBeingPlayed)
                    {
                        lineBeingPlayed.IsPlaying = true;
                        IsRecording = false;
                        // TODO: WordTimingQueue = new ObservableQueue<TimingWord>(LineBeingEdited.NewTiming);
                    }
                    Play();
                }
            }
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task Record(object? parameter)
        {
            if (!IsRecording)
            {
                /* TODO:  WordTimingQueue = new ObservableQueue<TimingWord>(LineBeingEdited.NewTiming);*/
                if (CurrentProcess.ManualTimingQueue.TryPeek(out var firstWord))
                {
                    WordIsNext(firstWord);
                }
                IsRecording = true;
            }


            if (PlayAudio == null || PlayAudio?.PlaybackState == PlaybackState.Stopped)
            {

                PlayAudio = new WaveOutEvent();

                (CurrentProcess.UnseparatedAudioStream ??
                 CurrentProcess.VocalsAudioStream)!.CurrentTime = TimeSpan.FromSeconds(CurrentPlaybackPositionSeconds ?? 0);

                PlayAudio.Init(CurrentProcess.UnseparatedAudioStream ?? CurrentProcess.VocalsAudioStream);
                IsPlaying = true;
                Play();
            }
        }

        public void SeekBarPointerPressed(object? sender, PointerPressedEventArgs args)
        {
            var control = sender as Control;
            var point = args.GetCurrentPoint(control);
            MovingCaret = true;
            SetCaretPosition(point.Position.X, control?.Bounds.Width ?? 0, FullLengthVocalsDraw.WaveformLengthSeconds ?? 0);
        }

        private void SetCaretPosition(double pointX, double widthX, double widthSeconds)
        {
            var positionSeconds = pointX / widthX * widthSeconds;
            CaretPositionSeconds = positionSeconds;
            CaretPositionText = TimeSpan.FromSeconds(positionSeconds).ToString("m\\:ss\\.ff");
        }

        public void SeekBarPointerMoved(object? sender, PointerEventArgs args)
        {
            if (MovingCaret)
            {
                var control = sender as Control;
                var point = args.GetCurrentPoint(control);
                SetCaretPosition(point.Position.X, control?.Bounds.Width ?? 0, FullLengthVocalsDraw.WaveformLengthSeconds ?? 0);
            }
        }

        public void SeekBarPointerReleased(object? sender, PointerReleasedEventArgs args)
        {
            if (MovingCaret)
            {
                var control = sender as Control;
                var point = args.GetCurrentPoint(control);
                SetCaretPosition(point.Position.X, control?.Bounds.Width ?? 0, FullLengthVocalsDraw.WaveformLengthSeconds ?? 0);

                Seek(CaretPositionSeconds);

                MovingCaret = false;
            }
        }

        private void Seek(double? seekToSeconds)
        {
            if (seekToSeconds == null) seekToSeconds = 0;
            StopTicking();

            CurrentPlaybackPositionSeconds = seekToSeconds;
            CurrentPlaybackPositionText = TimeSpan.FromSeconds(seekToSeconds.Value).ToString("m\\:ss\\.ff");
            CurrentOffsetSeconds = seekToSeconds;
            CaretPositionSeconds = seekToSeconds;

            if (PlayAudio != null)
            {
                PlayAudio.Stop();
                PlayAudio.Dispose();
            }

            PlayAudio = new WaveOutEvent();
            (CurrentProcess.UnseparatedAudioStream ??
             CurrentProcess.VocalsAudioStream)!.CurrentTime = TimeSpan.FromSeconds(CurrentOffsetSeconds ?? 0);
            PlayAudio.Init(CurrentProcess.UnseparatedAudioStream ?? CurrentProcess.VocalsAudioStream);
            if (IsPlaying)
            {
                Play();
                StartTicking();
            }
        }

        public void ManualAlignKeyDown(object? sender, KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Escape)
            {
                // TODO: cancel stuff maybe?
                //keyArgs.Handled = true;
            }

            if (IsRecording)
            {

                switch (keyArgs.Key)
                {
                    case Key.Right:
                        if (WordCurrentlyTiming == null)
                        {
                            if (CurrentProcess.ManualTimingQueue.Count == 0)
                            {
                                // TODO: this shouldn't happen; log it
                            }
                            else
                            {
                                // this is the first word; start it up
                                StartNextWord(CurrentPlaybackPositionSeconds);
                            }
                        }
                        else
                        {
                            if (WordCurrentlyTiming.IsRunning)
                            {
                                // this word is going right into the next one, so end it here
                                StopWord(WordCurrentlyTiming, CurrentPlaybackPositionSeconds);
                            }

                            // if there aren't any more words, stop running
                            if (CurrentProcess.ManualTimingQueue!.Count == 0)
                            {
                                Stop(null);
                            }
                            else
                            {
                                // whether the current word was running or not, the next one should start here
                                StartNextWord(CurrentPlaybackPositionSeconds);
                            }
                        }

                        keyArgs.Handled = true;
                        break;
                    case Key.Down:
                        if (WordCurrentlyTiming?.IsRunning ?? false)
                        {
                            // just stop the current word
                            StopWord(WordCurrentlyTiming, CurrentPlaybackPositionSeconds);
                            // if there aren't any more words, stop running
                            if (CurrentProcess.ManualTimingQueue!.Count == 0)
                            {
                                Stop(null);
                            }

                            keyArgs.Handled = true;
                        }
                        break;
                }
            }
        }
    }
}
