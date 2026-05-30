using System.Collections.ObjectModel;
using Avalonia.Input;
using FluentAvalonia.Core;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.Views;
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

        public class DummyEditLinesView : IEditLinesView
        {
            public bool Focus(NavigationMethod method = NavigationMethod.Unspecified, KeyModifiers keyModifiers = KeyModifiers.None)
            {
                return true;
            }
        }
        public class MoveLineToPrevious
        {
            [Fact]
            public void ShouldMergeLines()
            {
                var currentProcess = GetNewCurrentProcess();

                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new WordMerger(), new MinMaxFloatWaveStreamSampler())
                    { EditLinesView = new DummyEditLinesView() };

                viewModel.MoveLineToPrevious(currentProcess.ChosenLines!.Last());

                Assert.Single(currentProcess.ChosenLines!);
                var line = currentProcess.ChosenLines!.First();
                Assert.Equal(10, line.Words.Count);
            }

            [Fact]
            public void ShouldAddSpaceToPreviousWordWhenMergingLines()
            {
                var currentProcess = GetNewCurrentProcess();

                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new WordMerger(), new MinMaxFloatWaveStreamSampler())
                    { EditLinesView = new DummyEditLinesView()};

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
                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new WordMerger(), new MinMaxFloatWaveStreamSampler())
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
                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new WordMerger(), new MinMaxFloatWaveStreamSampler())
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
                var viewModel = new EditLinesViewModel(currentProcess, new LineSplitter(), new WordMerger(), new MinMaxFloatWaveStreamSampler())
                    {
                        EditingTextOfWord = modelWord
                    };
                viewModel.ApplyEditWordText("what|ever ");
                Assert.Equal("I am the very whatever of", currentProcess.ChosenLines[0].Text);
                Assert.Equal(7, currentProcess.ChosenLines[0].Words.Count);
            }
        }

        // Pages: 0,0,1,2  (two lines on page 0, one on page 1, one on page 2)
        private static KaraokeProcess GetProcessWithPagedLines() =>
            new KaraokeProcess
            {
                ChosenLines = new ObservableCollection<LyricLine>
                {
                    MakeLineWithPage(3,  "one two three",    0),
                    MakeLineWithPage(10, "four five six",    0),
                    MakeLineWithPage(17, "seven eight nine", 1),
                    MakeLineWithPage(24, "ten eleven twelve", 2)
                }
            };

        private static LyricLine MakeLineWithPage(int startSecond, string sentence, int pageIndex)
        {
            var line = MakeLinePerSeconds(startSecond, sentence);
            line.PageIndex = pageIndex;
            return line;
        }

        private static EditLinesViewModel MakeViewModel(KaraokeProcess process) =>
            new EditLinesViewModel(process, new LineSplitter(), new WordMerger(), new MinMaxFloatWaveStreamSampler())
                { EditLinesView = new DummyEditLinesView() };

        public class CanStartNewPageWithThisLine
        {
            [Fact]
            public void WhenNoPageIndexes_ShouldAlwaysBeEnabled()
            {
                var process = GetNewCurrentProcess();
                var vm = MakeViewModel(process);
                Assert.True(vm.StartNewPageWithThisLineCommand.CanExecute(process.ChosenLines![1].Words[0]));
            }

            [Fact]
            public void WhenNoPageIndexes_FirstLine_ShouldAlsoBeEnabled()
            {
                var process = GetNewCurrentProcess();
                var vm = MakeViewModel(process);
                Assert.True(vm.StartNewPageWithThisLineCommand.CanExecute(process.ChosenLines![0].Words[0]));
            }

            [Fact]
            public void WhenPageIndexes_AndFirstLineOfSong_ShouldBeDisabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                Assert.False(vm.StartNewPageWithThisLineCommand.CanExecute(process.ChosenLines![0].Words[0]));
            }

            [Fact]
            public void WhenPageIndexes_AndFirstLineOfPage_ShouldBeDisabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                // Line 2 is the first line of page 1
                Assert.False(vm.StartNewPageWithThisLineCommand.CanExecute(process.ChosenLines![2].Words[0]));
            }

            [Fact]
            public void WhenPageIndexes_AndSecondLineOfPage_ShouldBeEnabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                // Line 1 is the second line of page 0
                Assert.True(vm.StartNewPageWithThisLineCommand.CanExecute(process.ChosenLines![1].Words[0]));
            }
        }

        public class StartNewPageWithThisLine
        {
            [Fact]
            public void WhenNoPageIndexes_AndFirstLine_ShouldSetAllToPage0()
            {
                var process = GetNewCurrentProcess();
                var vm = MakeViewModel(process);
                vm.StartNewPageWithThisLineCommand.Execute(process.ChosenLines![0].Words[0]);
                Assert.Equal(0, process.ChosenLines![0].PageIndex);
                Assert.Equal(0, process.ChosenLines![1].PageIndex);
            }

            [Fact]
            public void WhenNoPageIndexes_AndSecondLine_ShouldSplitIntoTwoPages()
            {
                var process = GetNewCurrentProcess();
                var vm = MakeViewModel(process);
                vm.StartNewPageWithThisLineCommand.Execute(process.ChosenLines![1].Words[0]);
                Assert.Equal(0, process.ChosenLines![0].PageIndex);
                Assert.Equal(1, process.ChosenLines![1].PageIndex);
            }

            [Fact]
            public void WhenPageIndexes_ShouldIncrementThisLineAndSubsequent()
            {
                var process = GetProcessWithPagedLines(); // pages: 0,0,1,2
                var vm = MakeViewModel(process);
                // Split after line 1, before line 2 (which is already page 1)
                // But we're doing "start new page" on line 1 (second of page 0)
                vm.StartNewPageWithThisLineCommand.Execute(process.ChosenLines![1].Words[0]);
                Assert.Equal(0, process.ChosenLines![0].PageIndex); // unchanged
                Assert.Equal(1, process.ChosenLines![1].PageIndex); // was 0, now 1
                Assert.Equal(2, process.ChosenLines![2].PageIndex); // was 1, now 2
                Assert.Equal(3, process.ChosenLines![3].PageIndex); // was 2, now 3
            }

            [Fact]
            public void WhenPageIndexes_ShouldSetIsFirstLineOfPage()
            {
                var process = GetNewCurrentProcess();
                var vm = MakeViewModel(process);
                vm.StartNewPageWithThisLineCommand.Execute(process.ChosenLines![1].Words[0]);
                Assert.True(process.ChosenLines![1].IsFirstLineOfPage);
            }
        }

        public class CanMoveLineToPreviousPage
        {
            [Fact]
            public void WhenNoPageIndexes_ShouldBeDisabled()
            {
                var process = GetNewCurrentProcess();
                var vm = MakeViewModel(process);
                Assert.False(vm.MoveLineToPreviousPageCommand.CanExecute(process.ChosenLines![1].Words[0]));
            }

            [Fact]
            public void WhenOnPage0_ShouldBeDisabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                Assert.False(vm.MoveLineToPreviousPageCommand.CanExecute(process.ChosenLines![0].Words[0]));
            }

            [Fact]
            public void WhenNotFirstOfPage_ShouldBeDisabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                // Line 1 is page 0 but not the first of its page
                Assert.False(vm.MoveLineToPreviousPageCommand.CanExecute(process.ChosenLines![1].Words[0]));
            }

            [Fact]
            public void WhenFirstLineOfPageAndPageGreaterThan0_ShouldBeEnabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                // Line 2 is page 1, first line of that page
                Assert.True(vm.MoveLineToPreviousPageCommand.CanExecute(process.ChosenLines![2].Words[0]));
            }
        }

        public class MoveLineToPreviousPage
        {
            [Fact]
            public void ShouldDecrementPageIndex()
            {
                var process = GetProcessWithPagedLines(); // pages: 0,0,1,2
                var vm = MakeViewModel(process);
                vm.MoveLineToPreviousPageCommand.Execute(process.ChosenLines![2].Words[0]); // line on page 1
                Assert.Equal(0, process.ChosenLines![2].PageIndex);
            }

            [Fact]
            public void WhenOnlyLineOnPage_ShouldCascadeDecrementToSubsequentLines()
            {
                var process = GetProcessWithPagedLines(); // pages: 0,0,1,2
                var vm = MakeViewModel(process);
                vm.MoveLineToPreviousPageCommand.Execute(process.ChosenLines![2].Words[0]); // only line on page 1
                // Line 3 was page 2, should now be page 1
                Assert.Equal(1, process.ChosenLines![3].PageIndex);
            }

            [Fact]
            public void WhenNotOnlyLineOnPage_ShouldNotCascade()
            {
                // Add a second line to page 1 so line 2 is not the only one
                var process = new KaraokeProcess
                {
                    ChosenLines = new ObservableCollection<LyricLine>
                    {
                        MakeLineWithPage(3,  "one two three",    0),
                        MakeLineWithPage(10, "four five six",    0),
                        MakeLineWithPage(17, "seven eight nine", 1),
                        MakeLineWithPage(24, "ten eleven twelve", 1), // second line on page 1
                        MakeLineWithPage(31, "thirteen fourteen",   2)
                    }
                };
                var vm = MakeViewModel(process);
                vm.MoveLineToPreviousPageCommand.Execute(process.ChosenLines![2].Words[0]); // first of page 1
                // Line 3 should stay on page 1, line 4 should stay on page 2
                Assert.Equal(1, process.ChosenLines![3].PageIndex);
                Assert.Equal(2, process.ChosenLines![4].PageIndex);
            }

            [Fact]
            public void ShouldUpdateIsFirstLineOfPage()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                vm.MoveLineToPreviousPageCommand.Execute(process.ChosenLines![2].Words[0]);
                // Line 2 moved to page 0, no longer starts a new page
                Assert.False(process.ChosenLines![2].IsFirstLineOfPage);
            }
        }

        public class CanMoveLineToNextPage
        {
            [Fact]
            public void WhenNoPageIndexes_ShouldBeDisabled()
            {
                var process = GetNewCurrentProcess();
                var vm = MakeViewModel(process);
                Assert.False(vm.MoveLineToNextPageCommand.CanExecute(process.ChosenLines![0].Words[0]));
            }

            [Fact]
            public void WhenAlreadyOnHighestPage_ShouldBeDisabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                // Line 3 is on page 2, which is the max
                Assert.False(vm.MoveLineToNextPageCommand.CanExecute(process.ChosenLines![3].Words[0]));
            }

            [Fact]
            public void WhenNotLastLineOfPage_ShouldBeDisabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                // Line 0 is page 0 but not the last of its page
                Assert.False(vm.MoveLineToNextPageCommand.CanExecute(process.ChosenLines![0].Words[0]));
            }

            [Fact]
            public void WhenLastLineOfPageAndNotHighestPage_ShouldBeEnabled()
            {
                var process = GetProcessWithPagedLines();
                var vm = MakeViewModel(process);
                // Line 1 is the last of page 0 (page 1 exists)
                Assert.True(vm.MoveLineToNextPageCommand.CanExecute(process.ChosenLines![1].Words[0]));
            }
        }

        public class MoveLineToNextPage
        {
            [Fact]
            public void WhenNotOnlyLineOnPage_ShouldIncrementPageIndex()
            {
                var process = GetProcessWithPagedLines(); // pages: 0,0,1,2
                var vm = MakeViewModel(process);
                vm.MoveLineToNextPageCommand.Execute(process.ChosenLines![1].Words[0]); // last of page 0
                Assert.Equal(1, process.ChosenLines![1].PageIndex); // was 0, now 1
                // Other lines unchanged
                Assert.Equal(0, process.ChosenLines![0].PageIndex);
                Assert.Equal(1, process.ChosenLines![2].PageIndex);
                Assert.Equal(2, process.ChosenLines![3].PageIndex);
            }

            [Fact]
            public void WhenOnlyLineOnPage_ShouldDecrementSubsequentInsteadToAvoidGap()
            {
                var process = GetProcessWithPagedLines(); // pages: 0,0,1,2
                var vm = MakeViewModel(process);
                vm.MoveLineToNextPageCommand.Execute(process.ChosenLines![2].Words[0]); // only line on page 1
                // Line 2 stays on page 1, line 3 decrements from 2 to 1 (no gap)
                Assert.Equal(1, process.ChosenLines![2].PageIndex);
                Assert.Equal(1, process.ChosenLines![3].PageIndex);
                // Pages 0 lines unchanged
                Assert.Equal(0, process.ChosenLines![0].PageIndex);
                Assert.Equal(0, process.ChosenLines![1].PageIndex);
            }

            [Fact]
            public void WhenOnlyLineOnPage_ShouldNotLeaveGapInPageNumbers()
            {
                var process = GetProcessWithPagedLines(); // pages: 0,0,1,2
                var vm = MakeViewModel(process);
                vm.MoveLineToNextPageCommand.Execute(process.ChosenLines![2].Words[0]);
                var usedPages = process.ChosenLines!.Select(l => l.PageIndex).Distinct().OrderBy(p => p).ToList();
                // Should be 0,1 with no gaps
                Assert.Equal(new List<int?> { 0, 1 }, usedPages);
            }

            [Fact]
            public void ShouldUpdateIsFirstLineOfPage()
            {
                var process = GetProcessWithPagedLines(); // pages: 0,0,1,2
                var vm = MakeViewModel(process);
                vm.MoveLineToNextPageCommand.Execute(process.ChosenLines![1].Words[0]); // last of page 0 moves to page 1
                // Line 1 moved to page 1, now starts a new page
                Assert.True(process.ChosenLines![1].IsFirstLineOfPage);
            }
        }
    }
}
