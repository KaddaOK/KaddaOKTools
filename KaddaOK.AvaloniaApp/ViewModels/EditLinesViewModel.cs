using System;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.Library;
using NAudio.Wave;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using DialogHostAvalonia;
using KaddaOK.AvaloniaApp.Models;
using NAudio.Utils;
using NAudio.Wave.SampleProviders;
using Avalonia.Controls;
using KaddaOK.AvaloniaApp.Controls;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class EditLinesViewModel : TickableBase
    {
        private ILineSplitter Splitter { get; }
        private CancellationTokenSource AudioPlayingSource { get; }
        private IMinMaxFloatWaveStreamSampler Sampler { get; }
        public EditLinesViewModel(KaraokeProcess karaokeProcess, ILineSplitter splitter, IMinMaxFloatWaveStreamSampler sampler) : base(karaokeProcess)
        {
            FullLengthVocalsDraw = new WaveformDraw
            {
                DesiredPeakHeight = 100,
                VerticalImage = true
            };
            Splitter = splitter;
            Sampler = sampler;
            UndoStack = new ObservableStack<string?>();
            RedoStack = new ObservableStack<string?>();
            if ((CurrentProcess!.DetectedLinePossibilities?.Any(lp => lp.HasSelected) ?? false)
                && (!CurrentProcess.ChosenLines?.Any() ?? false))
            {
                CurrentProcess.SetChosenLinesToSelectedPossibilities();
            }
            AudioPlayingSource = new CancellationTokenSource();
            LineClipWaveform = new WaveformDraw();
        }

        private WaveStream? lineClipWaveStream;
        public WaveStream? LineClipWaveStream
        {
            get => lineClipWaveStream;
            set => SetProperty(ref lineClipWaveStream, value);
        }

        private (float, float)[]? lineClipWaveFloats;
        public (float, float)[]? LineClipWaveFloats
        {
            get => lineClipWaveFloats;
            set => SetProperty(ref lineClipWaveFloats, value);
        }

        private WaveformDraw? lineClipWaveform;
        public WaveformDraw? LineClipWaveform
        {
            get => lineClipWaveform;
            set => SetProperty(ref lineClipWaveform, value);
        }

        private WaveOutEvent? playAudio;
        public WaveOutEvent? PlayAudio
        {
            get => playAudio;
            set => SetProperty(ref playAudio, value);
        }

        private double? currentLineOffsetSeconds;
        public double? CurrentLineOffsetSeconds
        {
            get => currentLineOffsetSeconds;
            set => SetProperty(ref currentLineOffsetSeconds, value);
        }

        private double? _currentCaretPositionSecondsInClip;
        public double? CurrentCaretPositionSecondsInClip
        {
            get => _currentCaretPositionSecondsInClip;
            set => SetProperty(ref _currentCaretPositionSecondsInClip, value);
        }

        private double? _currentCaretPositionSecondsInFile;
        public double? CurrentCaretPositionSecondsInFile
        {
            get => _currentCaretPositionSecondsInFile;
            set => SetProperty(ref _currentCaretPositionSecondsInFile, value);
        }

        private double? currentLineRightEdgeSeconds;
        public double? CurrentLineRightEdgeSeconds
        {
            get => currentLineRightEdgeSeconds;
            set => SetProperty(ref currentLineRightEdgeSeconds, value);
        }

        private int? desiredOverlayRectangleHeight;
        public int? DesiredOverlayRectangleHeight
        {
            get => desiredOverlayRectangleHeight;
            set => SetProperty(ref desiredOverlayRectangleHeight, value);
        }

        private EditingLine? lineBeingEdited;
        public EditingLine? LineBeingEdited
        {
            get => lineBeingEdited;
            set => SetProperty(ref lineBeingEdited, value);
        }

        private bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set => SetProperty(ref isRecording, value);
        }

        private ObservableStack<string?> undoStack = null!;
        public ObservableStack<string?> UndoStack
        {
            get => undoStack;
            set => SetProperty(ref undoStack, value);
        }

        private ObservableStack<string?> redoStack = null!;
        public ObservableStack<string?> RedoStack
        {
            get => redoStack;
            set => SetProperty(ref redoStack, value);
        }

        private LyricWord? editingTextOfWord;
        public LyricWord? EditingTextOfWord
        {
            get => editingTextOfWord;
            set => SetProperty(ref editingTextOfWord, value);
        }

        [RelayCommand]
        private void NewLineBefore(object? parameter)
        {
            if (parameter is LyricWord newLineHere)
            {
                NewLineAt(newLineHere, true);
            }
        }

        [RelayCommand]
        private void NewLineAfter(object? parameter)
        {
            if (parameter is LyricWord newLineHere)
            {
                NewLineAt(newLineHere, false);
            }
        }

        private void AddUndoSnapshot()
        {
            if (CurrentProcess?.ChosenLines != null)
            {
                var currentChosenLinesString = JsonConvert.SerializeObject(CurrentProcess.ChosenLines.ToList(),
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                UndoStack.Push(currentChosenLinesString);

                // clear the redo stack because now we've done something different
                if (RedoStack.Any())
                {
                    RedoStack.Clear();
                }
            }
        }

        private void AddRedoSnapshot()
        {
            if (CurrentProcess?.ChosenLines != null)
            {
                var currentChosenLinesString = JsonConvert.SerializeObject(CurrentProcess.ChosenLines.ToList(),
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                RedoStack.Push(currentChosenLinesString);
            }
        }

        private void NewLineAt(LyricWord newLineHere, bool isBefore)
        {
            AddUndoSnapshot();
            Splitter.SplitLineAt(CurrentProcess?.ChosenLines, newLineHere, isBefore);
        }

        [RelayCommand]
        private void Undo(object? parameter)
        {
            var restoreChosenLinesString = UndoStack.Pop();
            if (restoreChosenLinesString != null)
            {
                var restoredChosenLines = JsonConvert.DeserializeObject<List<LyricLine>>(restoreChosenLinesString);
                if (restoredChosenLines != null)
                {
                    AddRedoSnapshot();
                    CurrentProcess!.ChosenLines = new ObservableCollection<LyricLine>(restoredChosenLines);
                }
            }
        }

        [RelayCommand]
        private void Redo(object? parameter)
        {
            var redoChosenLinesString = RedoStack.Pop();
            if (redoChosenLinesString != null)
            {
                var redoChosenLines = JsonConvert.DeserializeObject<List<LyricLine>>(redoChosenLinesString);
                if (redoChosenLines != null)
                {
                    AddUndoSnapshot();
                    CurrentProcess!.ChosenLines = new ObservableCollection<LyricLine>(redoChosenLines);
                }
            }
        }

        [RelayCommand]
        private void DeleteWord(object? parameter)
        {
            if (parameter is LyricWord newLineHere)
            {
                AddUndoSnapshot();
                Splitter.DeleteWord(CurrentProcess!.ChosenLines!, newLineHere);
            }
        }

        [RelayCommand]
        private async Task EditWordText(object? parameter)
        {
            if (parameter is LyricWord editThisWord)
            {
                EditingTextOfWord = editThisWord;
                if (await DialogHost.Show(this, "EditLinesViewDialogHost") is string newText)
                {
                    var lineNeedsTiming = ApplyEditWordText(newText);
                    if (lineNeedsTiming != null)
                    {
                        // go straight to the edit dialog for it since new timing exists now
                        await EditLine(lineNeedsTiming);
                    }
                };
            }
        }

        public LyricLine? ApplyEditWordText(string newText)
        {
            if (!string.IsNullOrWhiteSpace(newText)
                && EditingTextOfWord != null)
            {
                var newSyllables =
                    GetLyricWordsAcrossTime(newText, EditingTextOfWord.StartSecond, EditingTextOfWord.EndSecond);

                EditingTextOfWord.Text = newSyllables[0].Text;
                EditingTextOfWord.EndSecond = newSyllables[0].EndSecond;
                if (newSyllables.Count > 1)
                {
                    var line = CurrentProcess.ChosenLines?.SingleOrDefault(l => l.Words?.Contains(EditingTextOfWord) ?? false);
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

                    return line;
                }
            }

            return null;
        }

        [RelayCommand]
        public void ApplyLineTimingEdit(object? parameter)
        {
            if (LineBeingEdited != null)
            {
                LineBeingEdited.OriginalLine.Words = new ObservableCollection<LyricWord>(LineBeingEdited.NewTiming);
            }
        }

        [RelayCommand]
        public void MoveLineToPrevious(object? parameter)
        {
            if (parameter is LyricLine movingLine)
            {
                var movingLineIndex = CurrentProcess!.ChosenLines!.IndexOf(movingLine);
                if (movingLineIndex > 0)
                {
                    AddUndoSnapshot();

                    var previousLine = CurrentProcess!.ChosenLines![movingLineIndex - 1];
                    // add a space to the previous last word if it doesn't have one
                    var previousLastWord = previousLine.Words!.Last();
                    if (!previousLastWord.Text?.EndsWith(" ") ?? false)
                    {
                        previousLastWord.Text += " ";
                    }
                    var newWords = previousLine.Words!.Union(movingLine.Words!);
                    previousLine.Words = new ObservableCollection<LyricWord>(newWords);
                    CurrentProcess.ChosenLines.Remove(movingLine);
                }
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
                }
                else
                {
                    PlayAudio = new WaveOutEvent();
                    double? stopAfterSeconds = null;
                    if (parameter is EditingLine line)
                    {
                        // if we're view-editing a line right now, we should jump to the start of the line
                        var seekTimeInSeconds = line.OriginalLine.StartSecond;
                        var newLineMinSecond = line.NewTiming.DefaultIfEmpty().Min(w => w?.StartSecond) ?? 0;
                        if (newLineMinSecond > 0)
                        {
                            seekTimeInSeconds = Math.Min(seekTimeInSeconds, newLineMinSecond);
                        }

                        if (line.IsRecording)
                        {
                            // if we're recording the line right now, seek time should be 3 seconds before the first word
                            seekTimeInSeconds -= 3;
                        }

                        // unless that's less than 0, if so 0
                        if (seekTimeInSeconds < 0)
                        {
                            seekTimeInSeconds = 0;
                        }
                        (CurrentProcess.UnseparatedAudioStream ??
                            CurrentProcess.VocalsAudioStream)!.CurrentTime = TimeSpan.FromSeconds(seekTimeInSeconds);

                        if (!line.IsRecording)
                        {
                            // if we're not recording, we should stop playing at the end of the last word
                            var lastWordEndSeconds = line.OriginalLine.EndSecond;
                            var newLineMaxSecond = line.NewTiming.DefaultIfEmpty().Max(w => w?.EndSecond) ?? 0;
                            if (newLineMaxSecond > 0)
                            {
                                lastWordEndSeconds = Math.Max(lastWordEndSeconds, newLineMaxSecond);
                            }

                            if (lastWordEndSeconds > 0)
                            {
                                stopAfterSeconds = lastWordEndSeconds - seekTimeInSeconds;
                            }
                        }
                    }

                    PlayAudio.Init(CurrentProcess.UnseparatedAudioStream ??
                                   CurrentProcess.VocalsAudioStream);
                    PlayAudio.PlaybackStopped += PlayAudio_PlaybackStopped;
                    if (parameter is EditingLine lineBeingPlayed)
                    {
                        lineBeingPlayed.IsPlaying = true;
                        if (LineBeingEdited != null)
                        {
                            LineBeingEdited.IsRecording = false;
                            LineBeingEdited.WordTimingQueue = new ObservableQueue<TimingWord>(LineBeingEdited.NewTiming);
                        }
                    }
                    Play();
                    if (stopAfterSeconds != null)
                    {
                        try
                        {
                            await Task.Delay((int)stopAfterSeconds! * 1000, AudioPlayingSource.Token);
                            if (PlayAudio != null && PlayAudio.PlaybackState != PlaybackState.Stopped)
                            {
                                Stop(parameter);
                            }
                        }
                        catch (TaskCanceledException)
                        {
                            // we probably don't actually care, as this likely just means they pushed stop before the clip ended.
                        }
                    }
                }
            }
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task Record(object? parameter)
        {
            if (PlayAudio == null || PlayAudio?.PlaybackState == PlaybackState.Stopped)
            {

                PlayAudio = new WaveOutEvent();
                IsRecording = true;
                if (LineBeingEdited != null)
                {
                    LineBeingEdited.IsRecording = true;

                    await CalculateClipWindowAsync();

                    (CurrentProcess.UnseparatedAudioStream ??
                            CurrentProcess.VocalsAudioStream)!
                        .CurrentTime = TimeSpan.FromSeconds(CurrentLineOffsetSeconds!.Value);

                    PlayAudio.Init(CurrentProcess.UnseparatedAudioStream ??
                                   CurrentProcess.VocalsAudioStream);
                    PlayAudio.PlaybackStopped += PlayAudio_PlaybackStopped;
                    LineBeingEdited.IsPlaying = true;
                    LineBeingEdited.IsRecording = true;
                    LineBeingEdited.WordTimingQueue = new ObservableQueue<TimingWord>(LineBeingEdited.NewTiming);
                    if (LineBeingEdited.WordTimingQueue.TryPeek(out var firstWord))
                    {
                        WordIsNext(firstWord);
                    }
                }

                Play();
            }
        }

        private async Task CalculateClipWindowAsync()
        {
            var lineStartSeconds = LineBeingEdited!.OriginalLine.StartSecond;
            var newLineMinSecond = LineBeingEdited.NewTiming.DefaultIfEmpty().Min(w => w?.StartSecond) ?? 0;
            if (newLineMinSecond > 0)
            {
                lineStartSeconds = Math.Min(lineStartSeconds, newLineMinSecond) - 3;
            }

            // unless that's less than 0, if so 0
            if (lineStartSeconds < 0)
            {
                lineStartSeconds = 0;
            }

            CurrentLineOffsetSeconds = lineStartSeconds;

            var currentLineEndSeconds = LineBeingEdited.OriginalLine.EndSecond;
            var newLineMaxSecond = LineBeingEdited.NewTiming.DefaultIfEmpty().Max(w => w?.EndSecond) ?? 0;
            if (newLineMaxSecond > 0)
            {
                currentLineEndSeconds = Math.Max(currentLineEndSeconds, newLineMinSecond) + 3;
            }

            var maxLength = CurrentProcess.VocalsAudioStream?.TotalTime.TotalSeconds;
            // unless that's more than the length of the file, if so, that
            if (maxLength != null && currentLineEndSeconds > maxLength)
            {
                currentLineEndSeconds = maxLength.Value;
            }

            CurrentLineRightEdgeSeconds = currentLineEndSeconds;

            var lineOffsetTimespan = TimeSpan.FromSeconds(CurrentLineOffsetSeconds.Value);
            var lineEndTimespan = TimeSpan.FromSeconds(CurrentLineRightEdgeSeconds.Value);
            var lineLengthTimespan = lineEndTimespan - lineOffsetTimespan;

            if (LineClipWaveform != null)
            {
                LineClipWaveStream = CutAudio(CurrentProcess.VocalsAudioStream, lineOffsetTimespan, lineLengthTimespan);
                LineClipWaveform.DesiredImageWidth = 800;
                LineClipWaveform.DesiredPeakHeight = 100;
                DesiredOverlayRectangleHeight = 48;
                LineClipWaveFloats = await Sampler.GetAllFloatsAsync(LineClipWaveStream, 20); // TODO: set sampling factor by lineLengthTimespan?
                await LineClipWaveform.RedrawWaveform(LineClipWaveStream);
            }

        }

        public static WaveStream? CutAudio(WaveStream? wave,
            TimeSpan startPosition,
            TimeSpan lengthToTake)
        {
            if (wave == null) return null;

            if (wave.CanSeek)
            {
                wave.Seek(0, SeekOrigin.Begin);
            }

            ISampleProvider sourceProvider = wave.ToSampleProvider();

            OffsetSampleProvider offset = new OffsetSampleProvider(sourceProvider)
            {
                SkipOver = startPosition,
                Take = lengthToTake
            };

            var offsetWaveProvider = offset.ToWaveProvider();

            var croppedWaveStream = CopyAudioStream(offsetWaveProvider);
            if (croppedWaveStream.CanSeek)
            {
                croppedWaveStream.Seek(0, SeekOrigin.Begin);
            }
            return croppedWaveStream;
        }

        private static RawSourceWaveStream CopyAudioStream(IWaveProvider fromStream)
        {
            var memoryStream = new MemoryStream();
            byte[] readBytes = new byte[3200];
            int totalBytesRead = 0;
            int bytesLastRead = 0;
            do
            {
                bytesLastRead = fromStream.Read(readBytes, 0, 3200);
                totalBytesRead += bytesLastRead;
                memoryStream.Write(readBytes, 0, bytesLastRead);
            } while (bytesLastRead > 0);
            return new RawSourceWaveStream(memoryStream, fromStream.WaveFormat);
        }

        private void Play()
        {
            if (PlayAudio != null)
            {
                PlayAudio.Play();
                StartTicking();
            }

        }

        [RelayCommand]
        private void Stop(object? parameter)
        {
            AudioPlayingSource.Cancel();
            StopTicking();
            CurrentCaretPositionSecondsInClip = null;
            CurrentCaretPositionSecondsInFile = null;
            IsRecording = false;
            if (LineBeingEdited != null)
            {
                LineBeingEdited.IsRecording = false;
                LineBeingEdited.IsPlaying = false;
                LineBeingEdited.WordCurrentlyTiming = null;
                if (LineBeingEdited.WordTimingQueue != null)
                {
                    LineBeingEdited.WordTimingQueue.Clear();
                }

                foreach (var word in LineBeingEdited.NewTiming)
                {
                    word.IsNext = false;
                    word.IsRunning = false;
                    word.HasFinished = false;
                }
            }

            if (PlayAudio != null && PlayAudio.PlaybackState != PlaybackState.Stopped)
            {
                PlayAudio.Stop();
            }
        }

        private void PlayAudio_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (PlayAudio?.PlaybackState == PlaybackState.Stopped)
            {
                PlayAudio.Dispose();
                PlayAudio = null;
            }
        }

        [RelayCommand]
        private async Task EditLine(object? parameter)
        {
            if (parameter is LyricLine editThisLine)
            {
                LineBeingEdited = new EditingLine(editThisLine);
                await CalculateClipWindowAsync();
                await DialogHost.Show(LineBeingEdited, "EditLinesViewDialogHost");
            }
        }

        [RelayCommand]
        private async Task DeleteLine(object? parameter)
        {
            if (parameter is LyricLine deleteThisLine)
            {
                AddUndoSnapshot();
                CurrentProcess.ChosenLines!.Remove(deleteThisLine);
            }
        }

        [RelayCommand]
        private async Task AddNewLine(object? parameter)
        {
            if (parameter is LyricLine addLineAfter)
            {
                var addLineAfterIndex = CurrentProcess.ChosenLines!.IndexOf(addLineAfter);

                if (addLineAfterIndex >= 0)
                {
                    var addingLine = new AddingLine
                    {
                        PreviousLine = addLineAfter
                    };
                    if (CurrentProcess.ChosenLines.Count > addLineAfterIndex + 1)
                    {
                        addingLine.NextLine = CurrentProcess.ChosenLines[addLineAfterIndex + 1];
                    }

                    // show a dialog to enter the words
                    if (await DialogHost.Show(addingLine, "EditLinesViewDialogHost") is AddingLine newWords)
                    {
                        // create and add a new line
                        var newLine = new LyricLine
                        {
                            IsSelected = true,
                        };
                        var enteredText = newWords.EnteredText;
                        var startTime = newWords.PreviousLine?.EndSecond ?? 0;
                        var endTime = newWords.NextLine?.StartSecond
                                      ?? CurrentProcess.VocalsAudioStream!.TotalTime.TotalSeconds;

                        var listOfNewWords = GetLyricWordsAcrossTime(enteredText, startTime, endTime);
                        newLine.Words = new ObservableCollection<LyricWord>(listOfNewWords);

                        CurrentProcess.ChosenLines.Insert(addLineAfterIndex + 1, newLine);

                        // go straight to the edit dialog for it
                        await EditLine(newLine);
                    }
                }
            }
        }

        private static List<LyricWord> GetLyricWordsAcrossTime(string? enteredText, double startTime, double endTime)
        {
            var fullWords = enteredText?.Split(' ') ?? new[] { enteredText };
            var syllables = fullWords
                .SelectMany(f => (f + " ")
                    .Split('|'))
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .ToList();
            var availableTime = endTime - startTime;
            var eachSyllableGets = Math.Round(availableTime / (double)syllables.Count, 2);
            var currentTime = Math.Round(startTime, 2);
            var listOfNewWords = new List<LyricWord>();
            for (int i = 0; i < syllables.Count; i++)
            {
                var newWord = new LyricWord
                {
                    Text = syllables[i],
                    StartSecond = currentTime
                };
                currentTime = Math.Round(currentTime + eachSyllableGets, 2);
                newWord.EndSecond = currentTime;
                listOfNewWords.Add(newWord);
            }

            return listOfNewWords;
        }

        public void KeyPressed(KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Escape)
            {
                switch (modeBeingDragged)
                {
                    case WordDraggingMode.LeftSide:
                        wordBeingDragged!.StartSecond = OriginalValue ?? 0;
                        break;
                    case WordDraggingMode.RightSide:
                        wordBeingDragged!.EndSecond = OriginalValue ?? 0;
                        break;
                }
                wordBeingDragged = null;
                modeBeingDragged = WordDraggingMode.None;
                keyArgs.Handled = true;
            }

            if (IsRecording)
            {
                var positionOffsetSeconds = PlayAudio?.GetPositionTimeSpan().TotalSeconds;
                if (positionOffsetSeconds == null)
                {
                    // TODO: this shouldn't happen; log it
                    return;
                }

                var positionTotalSeconds = Math.Round(positionOffsetSeconds.Value + CurrentLineOffsetSeconds ?? 0, 2);
                switch (keyArgs.Key)
                {
                    case Key.Right:
                        if (LineBeingEdited!.WordCurrentlyTiming == null)
                        {
                            if (LineBeingEdited!.WordTimingQueue!.Count == 0)
                            {
                                // TODO: this shouldn't happen; log it
                            }
                            else
                            {
                                // this is the first word; start it up
                                StartNextWord(positionTotalSeconds);
                            }
                        }
                        else
                        {
                            if (LineBeingEdited.WordCurrentlyTiming.IsRunning)
                            {
                                // this word is going right into the next one, so end it here
                                StopWord(LineBeingEdited.WordCurrentlyTiming, positionTotalSeconds);
                            }

                            // if there aren't any more words, stop running
                            if (LineBeingEdited!.WordTimingQueue!.Count == 0)
                            {
                                Stop(null);
                            }
                            else
                            {
                                // whether the current word was running or not, the next one should start here
                                StartNextWord(positionTotalSeconds);
                            }
                        }
                        break;
                    case Key.Down:
                        if (LineBeingEdited!.WordCurrentlyTiming?.IsRunning ?? false)
                        {
                            // just stop the current word
                            StopWord(LineBeingEdited.WordCurrentlyTiming, positionTotalSeconds);
                            // if there aren't any more words, stop running
                            if (LineBeingEdited!.WordTimingQueue!.Count == 0)
                            {
                                Stop(null);
                            }
                        }
                        break;
                }
            }
        }

        private void StartNextWord([DisallowNull] double? positionTotalSeconds)
        {
            LineBeingEdited!.WordCurrentlyTiming = LineBeingEdited.WordTimingQueue!.Dequeue();
            StartWord(LineBeingEdited.WordCurrentlyTiming!, positionTotalSeconds.Value);

            if (LineBeingEdited.WordTimingQueue.TryPeek(out var nextWord))
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
            }
        }

        private void WordIsNext(TimingWord word)
        {
            word.IsRunning = false;
            word.IsNext = true;
            word.HasFinished = false;
        }

        protected override void Tick()
        {
            if (PlayAudio?.PlaybackState == PlaybackState.Playing)
            {
                CurrentCaretPositionSecondsInClip = Math.Round(PlayAudio.GetPositionTimeSpan().TotalSeconds, 2);
                CurrentCaretPositionSecondsInFile = CurrentCaretPositionSecondsInClip;

                if (LineBeingEdited != null)
                {
                    if (LineBeingEdited.IsPlaying)
                    {
                        CurrentCaretPositionSecondsInFile += Math.Round(CurrentLineOffsetSeconds ?? 0);
                        if (!LineBeingEdited.IsRecording)
                        {
                            // if you're not recording you started 3 seconds later than CurrentLineOffsetSeconds actually says
                            // TODO: this is a nasty hack arising from a stupid problem; rewrite all of that junk to do better.
                            CurrentCaretPositionSecondsInFile += 3;

                            void SetUpNextOrStop()
                            {
                                if (LineBeingEdited.WordTimingQueue!.TryDequeue(out var nextWord))
                                {
                                    LineBeingEdited.WordCurrentlyTiming = nextWord;
                                    if (LineBeingEdited.WordCurrentlyTiming!.StartSecond <
                                        CurrentCaretPositionSecondsInFile)
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

                            if (LineBeingEdited.WordCurrentlyTiming != null)
                            {
                                if (LineBeingEdited.WordCurrentlyTiming.StartSecond < CurrentCaretPositionSecondsInFile)
                                {
                                    if (LineBeingEdited.WordCurrentlyTiming.EndSecond <
                                        CurrentCaretPositionSecondsInFile)
                                    {
                                        StopWord(LineBeingEdited.WordCurrentlyTiming, null);
                                        SetUpNextOrStop();
                                    }
                                    else
                                    {
                                        StartWord(LineBeingEdited.WordCurrentlyTiming, null);
                                        if (LineBeingEdited.WordTimingQueue!.TryPeek(out var nextWord))
                                        {
                                            //WordIsNext(nextWord);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                SetUpNextOrStop();
                            }
                        }
                    }
                }

            }
            if (PlayAudio == null || PlayAudio.PlaybackState == PlaybackState.Stopped)
            {
                CurrentCaretPositionSecondsInClip = null;
                CurrentCaretPositionSecondsInFile = null;
            }
        }


        public enum WordDraggingMode
        {
            None,
            LeftSide,
            RightSide
        }

        private TimingWord? wordBeingDragged = null;
        private WordDraggingMode modeBeingDragged = WordDraggingMode.None;
        private double? dragTotalSeconds = null;
        private double? dragOffsetSeconds = null;
        private double? dragWidth = null;
        private double? OriginalValue = null;

        public void EditLineTimingDialogPointerPressed(object? sender, PointerPressedEventArgs args)
        {
            var control = sender as Control;
            var point = args.GetCurrentPoint(control);
            var hitControl = control?.InputHitTest(point.Position);

            if (hitControl is DragHandleLine { DataContext: TimingWord word } line)
            {
                wordBeingDragged = word;
                dragOffsetSeconds = line.ContainerOffsetSeconds;
                dragTotalSeconds = line.ContainerTotalSeconds;
                dragWidth = line.ContainerWidth;
                switch (line.Name)
                {
                    case "LeftBounds":
                        modeBeingDragged = WordDraggingMode.LeftSide;
                        OriginalValue = word.StartSecond;
                        break;
                    case "RightBounds":
                        modeBeingDragged = WordDraggingMode.RightSide;
                        OriginalValue = word.EndSecond;
                        break;
                    default:
                        modeBeingDragged = WordDraggingMode.None;
                        break;
                }
            }
        }

        public void EditLineTimingDialogPointerMoved(object? sender, PointerEventArgs args)
        {
            if (modeBeingDragged != WordDraggingMode.None
                && wordBeingDragged != null
                && dragWidth != null && dragTotalSeconds != null && dragOffsetSeconds != null)
            {
                var control = sender as Control;
                var point = args.GetCurrentPoint(control);

                var newValue = Math.Round(point.Position.X / dragWidth.Value * dragTotalSeconds.Value + dragOffsetSeconds.Value, 2);
                switch (modeBeingDragged)
                {
                    case WordDraggingMode.LeftSide:
                        wordBeingDragged.StartSecond = newValue;
                        break;
                    case WordDraggingMode.RightSide:
                        wordBeingDragged.EndSecond = newValue;
                        break;
                }
            }
        }

        public void EditLineTimingDialogPointerReleased(object? sender, PointerReleasedEventArgs args)
        {
            wordBeingDragged = null;
            modeBeingDragged = WordDraggingMode.None;
        }
    }
}
