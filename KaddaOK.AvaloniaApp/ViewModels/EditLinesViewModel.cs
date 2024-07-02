using System;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.Library;
using NAudio.Wave;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using KaddaOK.AvaloniaApp.Views;
using Avalonia.VisualTree;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public class ChosenLinesAction : ObservableBase
    {
        private string? serializedLines;
        public string? SerializedLines
        {
            get => serializedLines;
            set => SetProperty(ref serializedLines, value);
        }
        private string? changeLabel;
        public string? ChangeLabel
        {
            get => changeLabel;
            set => SetProperty(ref changeLabel, value);
        }

        public ChosenLinesAction(string? serializedLines, string? changeLabel)
        {
            SerializedLines = serializedLines;
            ChangeLabel = changeLabel;
        }
    }
    public partial class EditLinesViewModel : TickableBase
    {
        private string dialogHostName = "EditLinesViewDialogHost";

        private ILineSplitter Splitter { get; }
        private IWordMerger WordMerger { get; }
        private CancellationTokenSource AudioPlayingSource { get; }
        private IMinMaxFloatWaveStreamSampler Sampler { get; }
        public EditLinesView EditLinesView { get; set; } // TODO: are you sure you wanna do this? It's paradigm-breakingly bad practice...
        public EditLinesViewModel(KaraokeProcess karaokeProcess, ILineSplitter splitter, IWordMerger merger, IMinMaxFloatWaveStreamSampler sampler) : base(karaokeProcess)
        {
            FullLengthVocalsDraw = new WaveformDraw
            {
                DesiredPeakHeight = 100,
                VerticalImage = true
            };
            Splitter = splitter;
            WordMerger = merger;
            Sampler = sampler;
            UndoStack = new ObservableStack<ChosenLinesAction>();
            RedoStack = new ObservableStack<ChosenLinesAction>();
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

        private LyricWord? cursorWord;
        public LyricWord? CursorWord
        {
            get => cursorWord;
            set => SetProperty(ref cursorWord, value);
        }

        private LyricLine? cursorLine;
        public LyricLine? CursorLine
        {
            get => cursorLine;
            set => SetProperty(ref cursorLine, value);
        }

        private bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set => SetProperty(ref isRecording, value);
        }

        private ObservableStack<ChosenLinesAction> undoStack = null!;
        public ObservableStack<ChosenLinesAction> UndoStack
        {
            get => undoStack;
            set => SetProperty(ref undoStack, value);
        }

        private ObservableStack<ChosenLinesAction> redoStack = null!;
        public ObservableStack<ChosenLinesAction> RedoStack
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

        [RelayCommand]
        private void MergeWithPrev(object? parameter)
        {
            if (parameter is LyricWord mergeHere)
            {
                MergeWord(mergeHere, true);
            }
        }

        [RelayCommand]
        private void MergeWithNext(object? parameter)
        {
            if (parameter is LyricWord mergeHere)
            {
                MergeWord(mergeHere, false);
            }
        }

        private void AddUndoSnapshot(string changeLabel, bool isRedo = false)
        {
            if (CurrentProcess?.ChosenLines != null)
            {
                var currentChosenLinesString = JsonConvert.SerializeObject(CurrentProcess.ChosenLines.ToList(),
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                UndoStack.Push(new ChosenLinesAction(currentChosenLinesString, changeLabel));

                // clear the redo stack because now we've done something different
                if (RedoStack.Any() && !isRedo)
                {
                    RedoStack.Clear();
                }
            }
        }

        private void AddRedoSnapshot(string changeLabel)
        {
            if (CurrentProcess?.ChosenLines != null)
            {
                var currentChosenLinesString = JsonConvert.SerializeObject(CurrentProcess.ChosenLines.ToList(),
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                RedoStack.Push(new ChosenLinesAction(currentChosenLinesString, changeLabel));
            }
        }

        private void NewLineAt(LyricWord newLineHere, bool isBefore)
        {
            AddUndoSnapshot($"Split line {(isBefore ? "before" : "after")} \"{newLineHere.Text}\"");
            Splitter.SplitLineAt(CurrentProcess?.ChosenLines, newLineHere, isBefore);
            EditLinesView.Focus();
        }

        private void MergeWord(LyricWord mergeHere, bool isBefore)
        {
            AddUndoSnapshot($"Merge \"{mergeHere.Text}\" with {(isBefore ? "previous" : "next")} syllable");
            var mergeResult = WordMerger.MergeWord(CurrentProcess?.ChosenLines, mergeHere, isBefore);
            if (mergeResult.resultingWord != null)
            {
                CursorLine = mergeResult.resultingLine;
                CursorWord = mergeResult.resultingWord;
                EditLinesView.Focus();
            }
        }

        [RelayCommand]
        private void Undo(object? parameter)
        {
            var lastCommand = UndoStack.Pop();
            if (lastCommand?.SerializedLines != null)
            {
                var restoredChosenLines = JsonConvert.DeserializeObject<List<LyricLine>>(lastCommand.SerializedLines);
                if (restoredChosenLines != null)
                {
                    AddRedoSnapshot(lastCommand.ChangeLabel);
                    CurrentProcess!.ChosenLines = new ObservableCollection<LyricLine>(restoredChosenLines);
                }
            }

            // it seems to lose keyboard focus afterwards for some reason...
            EditLinesView.Focus();
            // and the word cursor no longer works, unfortunately...
            CursorLine = null;
            CursorWord = null;
        }

        [RelayCommand]
        private void Redo(object? parameter)
        {
            var nextCommand = RedoStack.Pop();
            if (nextCommand.SerializedLines != null)
            {
                var redoChosenLines = JsonConvert.DeserializeObject<List<LyricLine>>(nextCommand.SerializedLines);
                if (redoChosenLines != null)
                {
                    AddUndoSnapshot(nextCommand.ChangeLabel, true);
                    CurrentProcess!.ChosenLines = new ObservableCollection<LyricLine>(redoChosenLines);
                }
            }

            // it seems to lose keyboard focus afterwards for some reason...
            EditLinesView.Focus();
            // and the word cursor no longer works, unfortunately...
            CursorLine = null;
            CursorWord = null;
        }

        [RelayCommand]
        private void DeleteWord(object? parameter)
        {
            if (parameter is LyricWord deleteThis)
            {
                AddUndoSnapshot($"Delete syllable \"{deleteThis.Text}\"");

                var deletingFromLine = CurrentProcess!.ChosenLines!.SingleOrDefault(l => l.Words != null && l.Words.Contains(deleteThis));
                var deletedWordIndex = deletingFromLine.Words.IndexOf(deleteThis);

                Splitter.DeleteWord(CurrentProcess!.ChosenLines!, deleteThis);

                // find new focus word
                var newFocusedWord = deletingFromLine.Words.Count > deletedWordIndex
                    ? deletingFromLine.Words[deletedWordIndex]
                    : deletingFromLine.Words.LastOrDefault();

                CursorLine = deletingFromLine;
                CursorWord = newFocusedWord;

                EditLinesView.Focus();
            }
        }

        [RelayCommand]
        private void DeleteEntireLine(object? parameter)
        {
            LyricLine deletingLine;
            if (parameter is LyricWord wordInMovingLine)
            {
                deletingLine = CurrentProcess!.ChosenLines!.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordInMovingLine));
            }
            else if (parameter is LyricLine line)
            {
                deletingLine = line;
            }
            else
            {
                return;
            }
            AddUndoSnapshot($"Delete line \"{deletingLine.Text}\"");
            var deletedLineIndex = CurrentProcess!.ChosenLines.IndexOf(deletingLine);
            CurrentProcess!.ChosenLines.Remove(deletingLine);

            // find new focus word
            var newFocusedLine = CurrentProcess.ChosenLines.Count > deletedLineIndex
                                ? CurrentProcess.ChosenLines[deletedLineIndex] 
                                : CurrentProcess.ChosenLines.LastOrDefault();
            if (newFocusedLine?.Words?.Any() ?? false)
            {
                CursorLine = newFocusedLine;
                CursorWord = newFocusedLine.Words.First();
            }

            EditLinesView.Focus();
        }

        [RelayCommand]
        private async Task EditWordText(object? parameter)
        {
            if (parameter is LyricWord editThisWord)
            {
                EditingTextOfWord = editThisWord;
                if (await DialogHost.Show(this, dialogHostName) is string newText)
                {
                    AddUndoSnapshot($"Change syllable \"{editThisWord.Text}\" to \"{newText}\"");
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
                var newSyllables = LyricWord.GetLyricWordsAcrossTime(newText, EditingTextOfWord.StartSecond, EditingTextOfWord.EndSecond);

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
                AddUndoSnapshot($"Edited timing for \"{LineBeingEdited.OriginalLine.Text}\"");
                LineBeingEdited.OriginalLine.Words = new ObservableCollection<LyricWord>(LineBeingEdited.NewTiming);
                CursorLine = LineBeingEdited.OriginalLine;
                CursorWord = LineBeingEdited.OriginalLine.Words.FirstOrDefault();
            }

            EditLinesView.Focus();
        }

        [RelayCommand]
        public void MoveLineToPrevious(object? parameter)
        {
            LyricLine movingLine;
            if (parameter is LyricWord wordInMovingLine)
            {
                movingLine = CurrentProcess!.ChosenLines!.SingleOrDefault(l => l.Words != null && l.Words.Contains(wordInMovingLine));
            }
            else if (parameter is LyricLine line)
            {
                movingLine = line;
            }
            else
            {
                return;
            }

            var movingLineIndex = CurrentProcess!.ChosenLines!.IndexOf(movingLine);
            if (movingLineIndex > 0)
            {
                AddUndoSnapshot($"Moved \"{movingLine.Text}\" onto previous line");

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
            EditLinesView.Focus();
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
                await DialogHost.Show(LineBeingEdited, dialogHostName);
            }
        }

        [RelayCommand]
        private async Task AddNewLine(object? parameter)
        {
            LyricLine? addLineAfter;
            int addLineAfterIndex;
            if (parameter is LyricLine line)
            {
                addLineAfter = line;
                addLineAfterIndex = CurrentProcess.ChosenLines!.IndexOf(addLineAfter);
            }
            else if (parameter?.ToString() == "top")
            {
                addLineAfter = null;
                addLineAfterIndex = -1;
            }
            else
            {
                return;
            }

            var addingLine = new AddingLine
            {
                PreviousLine = addLineAfter
            };
            if (CurrentProcess.ChosenLines.Count > addLineAfterIndex + 1)
            {
                addingLine.NextLine = CurrentProcess.ChosenLines[addLineAfterIndex + 1];
            }

            // show a dialog to enter the words
            if (await DialogHost.Show(addingLine, dialogHostName) is AddingLine newWords
                && !string.IsNullOrWhiteSpace(newWords.EnteredText))
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

                var listOfNewWords = LyricWord.GetLyricWordsAcrossTime(enteredText, startTime, endTime);
                newLine.Words = new ObservableCollection<LyricWord>(listOfNewWords);

                AddUndoSnapshot($"Added new line \"{newLine.Text}\"");
                CurrentProcess.ChosenLines.Insert(addLineAfterIndex + 1, newLine);

                // go straight to the edit dialog for it
                await EditLine(newLine);
            }
                
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
                case Key.A:
                    if (CursorWord != null)
                    {
                        NewLineAfter(CursorWord);
                    }
                    return true;
                case Key.S:
                    if (CursorWord != null)
                    {
                        NewLineBefore(CursorWord);
                    }
                    return true;
                case Key.W:
                    if (CursorWord != null)
                    {
                        MergeWithPrev(CursorWord);
                    }
                    return true;
                case Key.Q:
                    if (CursorWord != null)
                    {
                        MergeWithNext(CursorWord);
                    }
                    return true;
                case Key.E:
                    if (CursorWord != null)
                    {
                        EditWordText(CursorWord);
                    }
                    return true;
                case Key.D:
                    if (CursorWord != null)
                    {
                        DeleteWord(CursorWord);
                    }
                    return true;
                case Key.Delete:
                    if (CursorWord != null)
                    {
                        DeleteEntireLine(CursorWord);
                    }
                    return true;
                case Key.Z:
                    if (CursorWord != null)
                    {
                        MoveLineToPrevious(CursorWord);
                    }
                    return true;
                default:
                    return false;
            }
        }

        private void SwitchWordSelection(KeyEventArgs keyArgs)
        {
            // this is just gonna change focus and let the handler in the view set it back... wacky I know, but it makes
            // sense because of what we can and can't control around tabbing and such.
            // Also, apparently the order in which it finds these isn't reliable, so we'll have to sort it by the order in
            // memory...
            var allWordButtonsByPhrases = EditLinesView.GetVisualDescendants().Where(x => x.Name == "PhraseWordsItemsControl")
                .Select(s => new
                {
                    Buttons = s.GetVisualDescendants().Where(y => y.Name == "LyricWordButton").Select(y => (Button)y).ToList(),
                    SortOrder = CurrentProcess.ChosenLines.IndexOf(s.DataContext as LyricLine)
                }).OrderBy(s => s.SortOrder)
                .Select(s => s.Buttons.OrderBy(x => CurrentProcess.ChosenLines[s.SortOrder].Words.IndexOf(x.CommandParameter as LyricWord)).ToList())
                .Where(s => s.Count > 0) // I'm not sure why this even happened; virtualization maybe
                .ToList();


            Button? destinationButton = null;
            if (CursorWord == null)
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
                var currentPhrase = allWordButtonsByPhrases.SingleOrDefault(phrase => phrase.Any(word => word.CommandParameter == CursorWord));
                if (currentPhrase != null)
                {
                    var currentPhraseIndex = allWordButtonsByPhrases.IndexOf(currentPhrase);
                    var currentWordButton = currentPhrase.FirstOrDefault(word => word.CommandParameter == CursorWord);
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

        private void StartNextWord(double? positionTotalSeconds)
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

        [RelayCommand]
        private async Task NudgeAllTimings(object? parameter)
        {
            var result = await DialogHost.Show(new NudgeTimingsViewModel(), dialogHostName);
            if (result is NudgeTimingsViewModel appliedModel && appliedModel.NudgeBy != 0)
            {
                AddUndoSnapshot($"Nudge all timings by {appliedModel.NudgeBy}s");
                foreach (var line in CurrentProcess.ChosenLines)
                {
                    foreach (var word in line.Words)
                    {
                        word.StartSecond += appliedModel.NudgeBy;
                        word.EndSecond += appliedModel.NudgeBy;
                        line.RaiseTimingChanged();
                    }
                }
            };
        }

        [RelayCommand]
        private async Task UpperCaseAllText(object? parameter)
        {
            AddUndoSnapshot($"Change all text to UPPER CASE");
            foreach (var line in CurrentProcess.ChosenLines)
            {
                foreach (var word in line.Words)
                {
                    word.Text = word.Text?.ToUpperInvariant();
                }
            }
        }

        [RelayCommand]
        private async Task LowerCaseAllText(object? parameter)
        {
            AddUndoSnapshot($"Change all text to lower case");
            foreach (var line in CurrentProcess.ChosenLines)
            {
                foreach (var word in line.Words)
                {
                    word.Text = word.Text?.ToLowerInvariant();
                }
            }
        }

        [RelayCommand]
        private async Task SentenceCaseAllLines(object? parameter)
        {
            AddUndoSnapshot($"Change all text to Sentence case");
            foreach (var line in CurrentProcess.ChosenLines)
            {
                foreach (var word in line.Words)
                {
                    word.Text = word.Text?.ToLowerInvariant();

                    if (word.Text is { Length: >= 1 } && line.Words.FirstOrDefault() == word)
                    {
                        word.Text = word.Text[0].ToString().ToUpperInvariant() 
                                    + word.Text.Substring(1);
                    }
                }
            }
        }

        [RelayCommand]
        private async Task TitleCaseAllWords(object? parameter)
        {
            AddUndoSnapshot($"Change all text to Title Case");
            foreach (var line in CurrentProcess.ChosenLines)
            {
                foreach (var word in line.Words)
                {
                    word.Text = word.Text?.ToLowerInvariant();
                    var wordIndex = line.Words.IndexOf(word);
                    if (word.Text is { Length: >= 1 }
                        && (wordIndex == 0 || line.Words[wordIndex-1].Text.EndsWith(" ")))
                    {
                        word.Text = word.Text[0].ToString().ToUpperInvariant()
                                    + word.Text.Substring(1);
                    }
                }
            }
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

        public void EditLineTimingDialogKeyDown(object? sender, KeyEventArgs keyArgs)
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

                        keyArgs.Handled = true;
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

                            keyArgs.Handled = true;
                        }
                        break;
                }
            }
        }

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

    public class NudgeTimingsViewModel : ObservableBase
    {
        private double nudgeBy;
        public double NudgeBy
        {
            get => nudgeBy;
            set => SetProperty(ref nudgeBy, value);
        }
    }
}
