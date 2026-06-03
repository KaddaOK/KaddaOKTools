using System.Collections.ObjectModel;
using Avalonia.Media;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;
using KaddaOK.Library;
using NAudio.Wave;

namespace KaddaOK.AvaloniaApp.Tests;

public class KoktProjectServiceTests
{
    private MockSampler CreateMockSampler() => new();

    private MockAudioFromFile CreateMockAudioReader() => new();

    private KoktProjectService CreateService() => new(CreateMockAudioReader(), CreateMockSampler());

    public class MockAudioFromFile : IAudioFromFile
    {
        public WaveStream? GetAudioFromFile(string filePath) => null!;
    }

    public class MockSampler : IMinMaxFloatWaveStreamSampler
    {
        public Task<(float min, float max)[]?> GetAllFloatsAsync(WaveStream? waveStream, int dataSamplingFactor)
            => Task.FromResult<(float min, float max)[]?>(Array.Empty<(float min, float max)>());
    }

    private KaraokeProcess CreateProcessWithBasicState()
    {
        var process = new KaraokeProcess
        {
            KaraokeSource = InitialKaraokeSource.AzureSpeechService,
            VocalsAudioFilePath = @"C:\Music\Song (Vocals).flac",
            UnseparatedAudioFilePath = @"C:\Music\Song.flac",
            InstrumentalAudioFilePath = @"C:\Music\Song (Instrumental).flac",
            WaveformLengthSeconds = 234.5,
            WaveformLengthText = "3:54",
            LyricsFilePath = @"C:\Music\Song.txt",
            KnownOriginalLyrics = KnownOriginalLyrics.FromText("Line one\nLine two\nLine three"),
            SelectedTabIndex = 3,
            ExportToFilePath = @"C:\Music\Song.rzlrc",
            UnsungTextColor = Color.FromRgb(100, 200, 50),
            SungTextColor = Color.FromRgb(255, 0, 128)
        };
        return process;
    }

    private KaraokeProcess CreateProcessWithChosenLines()
    {
        var process = CreateProcessWithBasicState();
        process.ChosenLines = new ObservableCollection<LyricLine>
        {
            new LyricLine
            {
                Words = new ObservableCollection<LyricWord>
                {
                    new LyricWord { Text = "Hello ", StartSecond = 1.0, EndSecond = 1.5 },
                    new LyricWord { Text = "World", StartSecond = 1.5, EndSecond = 2.0 }
                },
                IsFirstLineOfPage = true
            },
            new LyricLine
            {
                Words = new ObservableCollection<LyricWord>
                {
                    new LyricWord { Text = "Second ", StartSecond = 3.0, EndSecond = 3.5 },
                    new LyricWord { Text = "line", StartSecond = 3.5, EndSecond = 4.0 }
                }
            }
        };
        return process;
    }

    public class SaveAndLoad : KoktProjectServiceTests
    {
        [Fact]
        public void RoundTrips_BasicState()
        {
            var service = CreateService();
            var original = CreateProcessWithBasicState();
            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(original, filePath);

                var loaded = new KaraokeProcess();
                service.Load(loaded, filePath);

                Assert.Equal(InitialKaraokeSource.AzureSpeechService, loaded.KaraokeSource);
                Assert.Equal(@"C:\Music\Song (Vocals).flac", loaded.VocalsAudioFilePath);
                Assert.Equal(@"C:\Music\Song.flac", loaded.UnseparatedAudioFilePath);
                Assert.Equal(@"C:\Music\Song (Instrumental).flac", loaded.InstrumentalAudioFilePath);
                Assert.Equal(234.5, loaded.WaveformLengthSeconds);
                Assert.Equal("3:54", loaded.WaveformLengthText);
                Assert.Equal(@"C:\Music\Song.txt", loaded.LyricsFilePath);
                Assert.Equal(3, loaded.SelectedTabIndex);
                Assert.Equal(@"C:\Music\Song.rzlrc", loaded.ExportToFilePath);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void RoundTrips_Colors()
        {
            var service = CreateService();
            var original = CreateProcessWithBasicState();
            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(original, filePath);

                var loaded = new KaraokeProcess();
                service.Load(loaded, filePath);

                Assert.Equal(Color.FromRgb(100, 200, 50), loaded.UnsungTextColor);
                Assert.Equal(Color.FromRgb(255, 0, 128), loaded.SungTextColor);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void RoundTrips_KnownOriginalLyrics()
        {
            var service = CreateService();
            var original = CreateProcessWithBasicState();
            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(original, filePath);

                var loaded = new KaraokeProcess();
                service.Load(loaded, filePath);

                Assert.NotNull(loaded.KnownOriginalLyrics);
                Assert.Equal(3, loaded.KnownOriginalLyrics!.UncleansedLines!.Count);
                Assert.Equal("Line one", loaded.KnownOriginalLyrics.UncleansedLines![0]);
                Assert.Equal("Line two", loaded.KnownOriginalLyrics.UncleansedLines![1]);
                Assert.Equal("Line three", loaded.KnownOriginalLyrics.UncleansedLines![2]);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void RoundTrips_ChosenLines()
        {
            var service = CreateService();
            var original = CreateProcessWithChosenLines();
            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(original, filePath);

                var loaded = new KaraokeProcess();
                service.Load(loaded, filePath);

                Assert.NotNull(loaded.ChosenLines);
                Assert.Equal(2, loaded.ChosenLines!.Count);

                var firstLine = loaded.ChosenLines[0];
                Assert.True(firstLine.IsFirstLineOfPage);
                Assert.Equal(2, firstLine.Words.Count);
                Assert.Equal("Hello ", firstLine.Words[0].Text);
                Assert.Equal(1.0, firstLine.Words[0].StartSecond);
                Assert.Equal(1.5, firstLine.Words[0].EndSecond);
                Assert.Equal("World", firstLine.Words[1].Text);

                var secondLine = loaded.ChosenLines[1];
                Assert.False(secondLine.IsFirstLineOfPage);
                Assert.Equal("Second ", secondLine.Words[0].Text);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void RoundTrips_DetectedLinePossibilities()
        {
            var service = CreateService();
            var process = CreateProcessWithBasicState();
            var line1 = new LyricLine
            {
                Words = new ObservableCollection<LyricWord>
                {
                    new LyricWord { Text = "Option A ", StartSecond = 0, EndSecond = 1 }
                }
            };
            var line2 = new LyricLine
            {
                Words = new ObservableCollection<LyricWord>
                {
                    new LyricWord { Text = "Option B ", StartSecond = 0, EndSecond = 1 }
                }
            };
            var lp = new LinePossibilities(new[] { line1, line2 })
            {
                SelectedLyric = line2,
                IsIgnored = false
            };
            process.DetectedLinePossibilities = new ObservableCollection<LinePossibilities> { lp };

            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(process, filePath);

                var loaded = new KaraokeProcess();
                service.Load(loaded, filePath);

                Assert.NotNull(loaded.DetectedLinePossibilities);
                Assert.Single(loaded.DetectedLinePossibilities!);

                var loadedLp = loaded.DetectedLinePossibilities![0];
                Assert.Equal(2, loadedLp.Lyrics.Count);
                Assert.False(loadedLp.IsIgnored);
                Assert.NotNull(loadedLp.SelectedLyric);
                Assert.Equal("Option B ", loadedLp.SelectedLyric!.Words[0].Text);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void RoundTrips_ManualTimingLines()
        {
            var service = CreateService();
            var process = CreateProcessWithBasicState();
            process.KaraokeSource = InitialKaraokeSource.ManualSync;

            var words = new[]
            {
                new TimingWord { Text = "He", StartSecond = 1.0, EndSecond = 1.2, StartHasBeenManuallySet = true, EndHasBeenManuallySet = true },
                new TimingWord { Text = "llo ", StartSecond = 1.2, EndSecond = 1.5, StartHasBeenManuallySet = true, EndHasBeenManuallySet = false }
            };
            process.ManualTimingLines = new ObservableCollection<ManualTimingLine>
            {
                new ManualTimingLine(words)
            };

            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(process, filePath);

                var loaded = new KaraokeProcess();
                service.Load(loaded, filePath);

                Assert.NotNull(loaded.ManualTimingLines);
                Assert.Single(loaded.ManualTimingLines!);

                var loadedLine = loaded.ManualTimingLines![0];
                Assert.Equal(2, loadedLine.Words.Count);
                Assert.Equal("He", loadedLine.Words[0].Text);
                Assert.True(loadedLine.Words[0].StartHasBeenManuallySet);
                Assert.True(loadedLine.Words[0].EndHasBeenManuallySet);
                Assert.Equal("llo ", loadedLine.Words[1].Text);
                Assert.True(loadedLine.Words[1].StartHasBeenManuallySet);
                Assert.False(loadedLine.Words[1].EndHasBeenManuallySet);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void Sets_ProjectFilePath_OnSave()
        {
            var service = CreateService();
            var process = CreateProcessWithBasicState();
            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(process, filePath);
                Assert.Equal(filePath, process.ProjectFilePath);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void Sets_ProjectFilePath_OnLoad()
        {
            var service = CreateService();
            var original = CreateProcessWithBasicState();
            var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.koktpj");

            try
            {
                service.Save(original, filePath);

                var loaded = new KaraokeProcess();
                service.Load(loaded, filePath);
                Assert.Equal(filePath, loaded.ProjectFilePath);
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }

    public class GetAutoSaveFilePath : KoktProjectServiceTests
    {
        [Fact]
        public void Returns_ExistingProjectFilePath_WhenSet()
        {
            var service = CreateService();
            var process = new KaraokeProcess
            {
                ProjectFilePath = @"C:\Projects\MySong.koktpj"
            };

            var result = service.GetAutoSaveFilePath(process);
            Assert.Equal(@"C:\Projects\MySong.koktpj", result);
        }

        [Fact]
        public void Derives_From_ImportedFile_ForRzlrcImport()
        {
            var service = CreateService();
            var process = new KaraokeProcess
            {
                KaraokeSource = InitialKaraokeSource.RzlrcImport,
                ImportedKaraokeSourceFilePath = @"C:\Projects\MySong.rzlrc"
            };

            var result = service.GetAutoSaveFilePath(process);
            Assert.Equal(@"C:\Projects\MySong.koktpj", result);
        }

        [Fact]
        public void Derives_From_ImportedFile_ForKbpImport()
        {
            var service = CreateService();
            var process = new KaraokeProcess
            {
                KaraokeSource = InitialKaraokeSource.KbpImport,
                ImportedKaraokeSourceFilePath = @"C:\Projects\MySong.kbp"
            };

            var result = service.GetAutoSaveFilePath(process);
            Assert.Equal(@"C:\Projects\MySong.koktpj", result);
        }

        [Fact]
        public void Derives_From_UnseparatedAudio_ForAzure()
        {
            var service = CreateService();
            var process = new KaraokeProcess
            {
                KaraokeSource = InitialKaraokeSource.AzureSpeechService,
                UnseparatedAudioFilePath = @"C:\Music\MySong.flac",
                InstrumentalAudioFilePath = @"C:\Music\MySong (Instrumental).flac",
                VocalsAudioFilePath = @"C:\Music\MySong (Vocals).flac"
            };

            var result = service.GetAutoSaveFilePath(process);
            Assert.Equal(@"C:\Music\MySong.koktpj", result);
        }

        [Fact]
        public void Falls_Back_To_InstrumentalAudio()
        {
            var service = CreateService();
            var process = new KaraokeProcess
            {
                KaraokeSource = InitialKaraokeSource.ManualSync,
                InstrumentalAudioFilePath = @"C:\Music\MySong (Instrumental).flac",
                VocalsAudioFilePath = @"C:\Music\MySong (Vocals).flac"
            };

            var result = service.GetAutoSaveFilePath(process);
            Assert.Equal(@"C:\Music\MySong (Instrumental).koktpj", result);
        }

        [Fact]
        public void Falls_Back_To_VocalsAudio()
        {
            var service = CreateService();
            var process = new KaraokeProcess
            {
                KaraokeSource = InitialKaraokeSource.ManualSync,
                VocalsAudioFilePath = @"C:\Music\MySong (Vocals).flac"
            };

            var result = service.GetAutoSaveFilePath(process);
            Assert.Equal(@"C:\Music\MySong (Vocals).koktpj", result);
        }

        [Fact]
        public void Returns_Null_WhenNoPathsAvailable()
        {
            var service = CreateService();
            var process = new KaraokeProcess
            {
                KaraokeSource = InitialKaraokeSource.AzureSpeechService
            };

            var result = service.GetAutoSaveFilePath(process);
            Assert.Null(result);
        }
    }
}
