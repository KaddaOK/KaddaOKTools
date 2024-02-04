using System.Collections.ObjectModel;
using FluentAvalonia.Core;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.Library;
using NAudio.Wave;

namespace KaddaOK.AvaloniaApp.Tests.ViewModels
{
    public class MockSampler : IMinMaxFloatWaveStreamSampler
    {
        public Task<(float min, float max)[]?> GetAllFloatsAsync(WaveStream? waveStream, int dataSamplingFactor)
        {
            return Task.FromResult<(float min, float max)[]?>(Array.Empty<(float min, float max)>());
        }
    }
    public class EditLinesViewModelTests
    {
        private static KaraokeProcess GetNewCurrentProcess() =>
            new KaraokeProcess
            {
                ChosenLines = new ObservableCollection<LyricLine>
                {
                    MakeLinePerSeconds(3, "I am the very model of"),
                    MakeLinePerSeconds(10, "a modern major general")
                }
            };

        public class MoveLineToPrevious
        {
            [Fact]
            public void ShouldMergeLines()
            {
                var currentProcess = GetNewCurrentProcess();

                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new MinMaxFloatWaveStreamSampler());

                viewModel.MoveLineToPrevious(currentProcess.ChosenLines!.Last());

                Assert.Single(currentProcess.ChosenLines!);
                var line = currentProcess.ChosenLines!.First();
                Assert.Equal(10, line.Words.Count);
            }

            [Fact]
            public void ShouldAddSpaceToPreviousWordWhenMergingLines()
            {
                var currentProcess = GetNewCurrentProcess();

                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new MinMaxFloatWaveStreamSampler());

                viewModel.MoveLineToPrevious(currentProcess.ChosenLines!.Last());

                var line = currentProcess.ChosenLines!.First();
                foreach (var word in line.Words)
                {
                    if (word != line.Words.Last())
                    {
                        Assert.EndsWith(" ", word.Text!);
                    }
                }
            }
        }

        public static LyricLine MakeLinePerSeconds(int startSecond, string sentence)
        {
            return new LyricLine
            {
                IsSelected = true,
                Words = MakeWordsPerSecond(startSecond, sentence)
            };
        }

        public static ObservableCollection<LyricWord> MakeWordsPerSecond(int startSecond, string sentence)
        {
            var words = sentence.Split(' ');
            return new ObservableCollection<LyricWord>(
                words.Select(w => new LyricWord
                {
                    Text = $"{w}{(w == words.Last() ? "" : " ")}",
                    StartSecond = startSecond + words.IndexOf(w),
                    EndSecond = startSecond + words.IndexOf(w) + 1
                })
            );
        }

        public class ApplyEditWordText
        {
            [Fact]
            public void ShouldSetWordText()
            {
                var currentProcess = GetNewCurrentProcess();
                var modelWord = currentProcess.ChosenLines![0].Words[4];
                Assert.Equal("model ", modelWord.Text);
                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new MinMaxFloatWaveStreamSampler())
                    {
                        EditingTextOfWord = modelWord
                    };
                viewModel.ApplyEditWordText("whatever ");
                Assert.Equal(6, currentProcess.ChosenLines[0].Words.Count);
                Assert.Equal("I am the very whatever of", currentProcess.ChosenLines[0].Text);
            }

            [Fact]
            public void ShouldSplitCorrectlyOnSpaces()
            {
                var currentProcess = GetNewCurrentProcess();
                var modelWord = currentProcess.ChosenLines![0].Words[4];
                Assert.Equal("model ", modelWord.Text);
                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new MinMaxFloatWaveStreamSampler())
                    {
                        EditingTextOfWord = modelWord
                    };
                viewModel.ApplyEditWordText("what ever ");
                Assert.Equal(7, currentProcess.ChosenLines[0].Words.Count);
                Assert.Equal("I am the very what ever of", currentProcess.ChosenLines[0].Text);
            }

            [Fact]
            public void ShouldSplitCorrectlyOnPipes()
            {
                var currentProcess = GetNewCurrentProcess();
                var modelWord = currentProcess.ChosenLines![0].Words[4];
                Assert.Equal("model ", modelWord.Text);
                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new MinMaxFloatWaveStreamSampler())
                    {
                        EditingTextOfWord = modelWord
                    };
                viewModel.ApplyEditWordText("what|ever ");
                Assert.Equal("I am the very whatever of", currentProcess.ChosenLines[0].Text);
                Assert.Equal(7, currentProcess.ChosenLines[0].Words.Count);
            }
        }
    }
}
