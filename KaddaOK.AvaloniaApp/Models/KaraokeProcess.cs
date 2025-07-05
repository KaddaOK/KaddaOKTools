using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using KaddaOK.Library;
using KaddaOK.Library.Kbs;
using KaddaOK.Library.Ytmm;
using NAudio.Wave;

namespace KaddaOK.AvaloniaApp.Models
{
    public enum InitialKaraokeSource
    {
        NotSelected,
        AzureSpeechService,
        RzlrcImport,
        KbpImport,
        CtmImport,
        ManualSync
    }

    public partial class KaraokeProcess : ObservableBase
    {
        private InitialKaraokeSource _karaokeSource;
        public InitialKaraokeSource KaraokeSource
        {
            get => _karaokeSource;
            set
            {
                SetProperty(ref _karaokeSource, value);

                RaisePropertyChanged(nameof(KaraokeSourceIsSet));

                RaisePropertyChanged(nameof(RecognizeTabVisible));
                RaisePropertyChanged(nameof(NarrowTabVisible));
                RaisePropertyChanged(nameof(ManualAlignTabVisible));
                RaisePropertyChanged(nameof(LyricsTabVisible));

                RaisePropertyChanged(nameof(ReasonManualAlignIsDisabled));
                RaisePropertyChanged(nameof(ManualAlignIsEnabled));

                RaisePropertyChanged(nameof(ReasonEditTabIsDisabled));
                RaisePropertyChanged(nameof(EditTabIsEnabled));

                RaisePropertyChanged(nameof(ReasonLyricsStepIsIncomplete));
                RaisePropertyChanged(nameof(LyricsStepIsComplete));
            }
        }

        private string? _importedKaraokeSourceFilePath;
        public string? ImportedKaraokeSourceFilePath
        {
            get => _importedKaraokeSourceFilePath;
            set
            {
                if (SetProperty(ref _importedKaraokeSourceFilePath, value))
                {
                    AudioStepCompletenessChanged();
                };
            }
        }

        private List<RzlrcLyric>? _originalImportedRzlrcFile;
        public List<RzlrcLyric>? OriginalImportedRzlrcFile
        {
            get => _originalImportedRzlrcFile;
            set => SetProperty(ref _originalImportedRzlrcFile, value);
        }

        private RzlrcLyric? _originalImportedRzlrcPage;
        public RzlrcLyric? OriginalImportedRzlrcPage
        {
            get => _originalImportedRzlrcPage;
            set => SetProperty(ref _originalImportedRzlrcPage, value);
        }

        private KbpFile? _originalImportedKbpFile;
        public KbpFile? OriginalImportedKbpFile
        {
            get => _originalImportedKbpFile;
            set => SetProperty(ref _originalImportedKbpFile, value);
        }

        private int? _selectedTabIndex;
        public int? SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        public bool KaraokeSourceIsSet => KaraokeSource != InitialKaraokeSource.NotSelected;

        #region Audio Step

        public void AudioStepCompletenessChanged()
        {
            RaisePropertyChanged(nameof(ReasonAudioStepIsIncomplete));
            RaisePropertyChanged(nameof(AudioStepIsComplete));
            RaisePropertyChanged(nameof(ReasonManualAlignIsDisabled));
            RaisePropertyChanged(nameof(ManualAlignIsEnabled));
        }

        public bool AudioStepIsComplete => string.IsNullOrWhiteSpace(ReasonAudioStepIsIncomplete);

        public string? ReasonAudioStepIsIncomplete
        {
            get
            {
                if (string.IsNullOrWhiteSpace(VocalsAudioFilePath))
                {
                    return "Vocal audio file is required.";
                }

                return null;
            }
        }

        private string? _vocalsAudioFilePath;
        public string? VocalsAudioFilePath
        {
            get => _vocalsAudioFilePath;
            set
            {
                if (SetProperty(ref _vocalsAudioFilePath, value))
                {
                    AudioStepCompletenessChanged();
                };
            }
        }

        private double? _waveformLengthSeconds;
        public double? WaveformLengthSeconds
        {
            get => _waveformLengthSeconds;
            set => SetProperty(ref _waveformLengthSeconds, value);
        }

        private string? _waveformLengthText;
        public string? WaveformLengthText
        {
            get => _waveformLengthText;
            set => SetProperty(ref _waveformLengthText, value);
        }

        private WaveStream? _vocalsAudioStream;
        public WaveStream? VocalsAudioStream
        {
            get => _vocalsAudioStream;
            set => SetProperty(ref _vocalsAudioStream, value);
        }

        private (float min, float max)[]? _vocalsAudioFloats;
        public (float min, float max)[]? VocalsAudioFloats
        {
            get => _vocalsAudioFloats;
            set => SetProperty(ref _vocalsAudioFloats, value);
        }

        private string? _unseparatedAudioFilePath;
        public string? UnseparatedAudioFilePath
        {
            get => _unseparatedAudioFilePath;
            set => SetProperty(ref _unseparatedAudioFilePath, value);
        }

        private WaveStream? _unseparatedAudioStream;
        public WaveStream? UnseparatedAudioStream
        {
            get => _unseparatedAudioStream;
            set => SetProperty(ref _unseparatedAudioStream, value);
        }

        private (float min, float max)[]? _unseparatedAudioFloats;
        public (float min, float max)[]? UnseparatedAudioFloats
        {
            get => _unseparatedAudioFloats;
            set => SetProperty(ref _unseparatedAudioFloats, value);
        }

        private string? _instrumentalAudioFilePath;
        public string? InstrumentalAudioFilePath
        {
            get => _instrumentalAudioFilePath;
            set => SetProperty(ref _instrumentalAudioFilePath, value);
        }

        private WaveStream? _instrumentalAudioStream;
        public WaveStream? InstrumentalAudioStream
        {
            get => _instrumentalAudioStream;
            set => SetProperty(ref _instrumentalAudioStream, value);
        }

        private (float min, float max)[]? _instrumentalAudioFloats;
        public (float min, float max)[]? InstrumentalAudioFloats
        {
            get => _instrumentalAudioFloats;
            set => SetProperty(ref _instrumentalAudioFloats, value);
        }

        public void ClearAudioAndDownstream()
        {
            VocalsAudioFilePath = null;
            WaveformLengthSeconds = null;
            WaveformLengthText = null;
            VocalsAudioStream = null;
            UnseparatedAudioFilePath = null;
            UnseparatedAudioStream = null;
            InstrumentalAudioFilePath = null;
            InstrumentalAudioStream = null;
            ImportedKaraokeSourceFilePath = null;
            OriginalImportedKbpFile = null;
            OriginalImportedRzlrcFile = null;
            OriginalImportedRzlrcPage = null;
            //KaraokeSource = InitialKaraokeSource.AzureSpeechService;
            ClearLyricsAndDownstream();
        }

        #endregion

        #region Lyrics Step

        public bool LyricsTabVisible => KaraokeSource != InitialKaraokeSource.KbpImport 
                                        && KaraokeSource != InitialKaraokeSource.RzlrcImport
                                        && KaraokeSource != InitialKaraokeSource.NotSelected;

        private string? _lyricsFilePath;
        public string? LyricsFilePath
        {
            get => _lyricsFilePath;
            set => SetProperty(ref _lyricsFilePath, value);
        }

        private KnownOriginalLyrics? _knownOriginalLyrics;
        public KnownOriginalLyrics? KnownOriginalLyrics
        {
            get => _knownOriginalLyrics;
            set
            {
                if (SetProperty(ref _knownOriginalLyrics, value))
                {
                    LyricsStepCompletenessChanged();
                };
            }
        }

        public void LyricsStepCompletenessChanged()
        {
            RaisePropertyChanged(nameof(ReasonLyricsStepIsIncomplete));
            RaisePropertyChanged(nameof(LyricsStepIsComplete));
            RaisePropertyChanged(nameof(ReasonManualAlignIsDisabled));
            RaisePropertyChanged(nameof(ManualAlignIsEnabled));
        }

        public bool LyricsStepIsComplete => string.IsNullOrWhiteSpace(ReasonLyricsStepIsIncomplete);

        public string? ReasonLyricsStepIsIncomplete
        {
            get
            {
                if (KaraokeSource != InitialKaraokeSource.CtmImport // Which has the lyrics anyway; inserting them just gives line breaks
                    && (!KnownOriginalLyrics?.SeparatorCleansedLines?.Any() ?? true))
                {
                    return "Lyrics are required for this project type.";
                }

                return null;
            }
        }

        public void ClearLyricsAndDownstream()
        {
            LyricsFilePath = null;
            KnownOriginalLyrics = null;
            ClearRecognizeAndDownstream();
        }

        #endregion

        #region Recognize Step

        public bool RecognizeTabVisible => KaraokeSource == InitialKaraokeSource.AzureSpeechService;

        private ObservableCollection<LinePossibilities>? detectedLinePossibilities;
        public ObservableCollection<LinePossibilities>? DetectedLinePossibilities
        {
            get => detectedLinePossibilities;
            set
            {
                if (SetProperty(ref detectedLinePossibilities, value))
                {
                    RecognizeStepCompletenessChanged();
                    NarrowingStepCompletenessChanged();
                };
            }
        }

        private bool recognitionIsRunning;
        public bool RecognitionIsRunning
        {
            get => recognitionIsRunning;
            set
            {
                if (SetProperty(ref recognitionIsRunning, value))
                {
                    RecognizeStepCompletenessChanged();
                };
            }
        }

        public void RecognizeStepCompletenessChanged()
        {
            RaisePropertyChanged(nameof(ReasonRecognizeStepIsIncomplete));
            RaisePropertyChanged(nameof(RecognizeStepIsComplete));
        }

        public bool RecognizeStepIsComplete => string.IsNullOrWhiteSpace(ReasonRecognizeStepIsIncomplete);

        public string? ReasonRecognizeStepIsIncomplete
        {
            get
            {
                if (RecognitionIsRunning)
                {
                    return "The recognition process is still running.";
                }

                if (!DetectedLinePossibilities?.Any() ?? true)
                {
                    return "No phrases have been recognized.";
                }

                return null;
            }
        }

        public void ClearRecognizeAndDownstream()
        {
            DetectedLinePossibilities = null;
            RecognitionIsRunning = false;
            ClearNarrowingAndDownstream();
        }

        #endregion

        #region Manual Alignment
        private ObservableCollection<ManualTimingLine>? manualTimingLines;
        public ObservableCollection<ManualTimingLine>? ManualTimingLines
        {
            get => manualTimingLines;
            set
            {
                if (SetProperty(ref manualTimingLines, value))
                {
                    ManualAlignmentCompletenessChanged();
                };
            }
        }

        public int TotalWordsInManualTiming
        {
            get
            {
                if (!(ManualTimingLines?.Any() ?? false)) return 0;

                return ManualTimingLines.SelectMany(m => m.Words).Count();
            }
        }

        public int TimedWordsInManualTiming
        {
            get
            {
                if (!(ManualTimingLines?.Any() ?? false)) return 0;

                return ManualTimingLines.SelectMany(m => m.Words).Count(m => m.StartHasBeenManuallySet && m.EndHasBeenManuallySet);
            }
        }

        private ObservableQueue<TimingWord>? manualTimingQueue;
        public ObservableQueue<TimingWord>? ManualTimingQueue
        {
            get => manualTimingQueue;
            set => SetProperty(ref manualTimingQueue, value);
        }

        public bool ManualAlignTabVisible => KaraokeSource == InitialKaraokeSource.ManualSync;

        public bool ManualAlignIsEnabled => string.IsNullOrWhiteSpace(ReasonManualAlignIsDisabled);

        public string? ReasonManualAlignIsDisabled
        {
            get
            {
                if (KaraokeSource != InitialKaraokeSource.ManualSync)
                {
                    return "This is not a manual sync project.";
                }

                return ReasonAudioStepIsIncomplete ?? ReasonLyricsStepIsIncomplete;
            }
        }

        public void ManualAlignmentCompletenessChanged()
        {
            RaisePropertyChanged(nameof(TimedWordsInManualTiming));
            RaisePropertyChanged(nameof(TotalWordsInManualTiming));
            RaisePropertyChanged(nameof(ReasonManualAlignmentIsIncomplete));
            RaisePropertyChanged(nameof(ManualAlignmentIsComplete));
            RaisePropertyChanged(nameof(ReasonEditTabIsDisabled));
            RaisePropertyChanged(nameof(EditTabIsEnabled));
        }

        public bool ManualAlignmentIsComplete => string.IsNullOrWhiteSpace(ReasonManualAlignmentIsIncomplete);

        public string? ReasonManualAlignmentIsIncomplete
        {
            get
            {
                if (ManualTimingLines == null || !ManualTimingLines.Any())
                {
                    return "There are no lines of lyrics.";
                }

                var unprocessedSyllables = TotalWordsInManualTiming - TimedWordsInManualTiming;
                if (unprocessedSyllables > 0)
                {
                    return $"{unprocessedSyllables} syllables have not yet been timed.";
                }

                return null;
            }
        }
        #endregion


        #region Narrow Step

        public bool NarrowTabVisible => KaraokeSource == InitialKaraokeSource.AzureSpeechService;

        private ObservableCollection<LyricLine>? chosenLines;
        public ObservableCollection<LyricLine>? ChosenLines
        {
            get => chosenLines;
            set
            {
                if (SetProperty(ref chosenLines, value))
                {
                    NarrowingStepCompletenessChanged();
                };
            }
        }

        public void NarrowingStepCompletenessChanged()
        {
            RaisePropertyChanged(nameof(ReasonNarrowingStepIsIncomplete));
            RaisePropertyChanged(nameof(NarrowingStepIsComplete));
            RaisePropertyChanged(nameof(ReasonEditTabIsDisabled));
            RaisePropertyChanged(nameof(EditTabIsEnabled));
        }

        public bool NarrowingStepIsComplete => string.IsNullOrWhiteSpace(ReasonNarrowingStepIsIncomplete);

        public string? ReasonNarrowingStepIsIncomplete
        {
            get
            {
                if (DetectedLinePossibilities == null)
                {
                    return "No lines have been recognized.";
                }

                var unprocessedCount = DetectedLinePossibilities.Count(lp => !lp.IsIgnored && !lp.HasSelected);

                if (unprocessedCount > 0)
                {
                    return $"{unprocessedCount} recognized lines still need attention.";
                }

                return null;
            }
        }

        public bool EditTabIsEnabled => string.IsNullOrWhiteSpace(ReasonEditTabIsDisabled);

        public string? ReasonEditTabIsDisabled
        {
            get
            {
                if (KaraokeSource == InitialKaraokeSource.RzlrcImport
                    || KaraokeSource == InitialKaraokeSource.KbpImport)
                {
                    return null;
                }
                if (KaraokeSource == InitialKaraokeSource.CtmImport)
                {
                    if (!ChosenLines?.Any() ?? true)
                    {
                        return "No lines to edit.";
                    }
                    return null;
                }
                if (KaraokeSource == InitialKaraokeSource.ManualSync)
                {
                    return ReasonManualAlignmentIsIncomplete;
                }

                return ReasonNarrowingStepIsIncomplete;
            }
        }

        public void ClearNarrowingAndDownstream()
        {
            // TODO: this
            ChosenLines = null;
        }

        public void SetChosenLinesToSelectedPossibilities()
        {
            if (DetectedLinePossibilities == null)
            {
                return;
            }
            ChosenLines =
                new ObservableCollection<LyricLine>(DetectedLinePossibilities
                    .Where(lp => lp.HasSelected && !lp.IsIgnored)
                    .Select(s => s.SelectedLyric)!);
            CanExportFactorsChanged();
        }

        public void RaiseChosenLinesChanged()
        {
            RaisePropertyChanged(nameof(ChosenLines));
        }

        #endregion

        public void CanExportFactorsChanged()
        {
            RaisePropertyChanged(nameof(ReasonCantExport));
            RaisePropertyChanged(nameof(CanExport));
        }

        public bool CanExport => string.IsNullOrWhiteSpace(ReasonCantExport);

        public string? ReasonCantExport
        {
            get
            {
                if (!ChosenLines?.Any() ?? true)
                {
                    return "No lines to export.";
                }

                return null;
            }
        }

        private string? _exportToFilePath;
        public string? ExportToFilePath
        {
            get => _exportToFilePath;
            set => SetProperty(ref _exportToFilePath, value);
        }

        private Color unsungTextColor = Color.FromRgb(155, 234, 244);
        public Color UnsungTextColor
        {
            get => unsungTextColor;
            set => SetProperty(ref unsungTextColor, value);
        }

        private Color unsungOutlineColor = Color.FromRgb(4, 28, 32);
        public Color UnsungOutlineColor
        {
            get => unsungOutlineColor;
            set => SetProperty(ref unsungOutlineColor, value);
        }

        private Color sungTextColor = Color.FromRgb(255, 110, 148);
        public Color SungTextColor
        {
            get => sungTextColor;
            set => SetProperty(ref sungTextColor, value);
        }

        private Color sungOutlineColor = Color.FromRgb(40, 0, 20);
        public Color SungOutlineColor
        {
            get => sungOutlineColor;
            set => SetProperty(ref sungOutlineColor, value);
        }

        private Color backgroundColor = Color.FromRgb(0, 10, 18);
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }

        private bool generateProject = true;
        public bool GenerateRzProject
        {
            get => generateProject;
            set => SetProperty(ref generateProject, value);
        }

        private bool insertProgressBars = true;
        public bool InsertProgressBars
        {
            get => insertProgressBars;
            set => SetProperty(ref insertProgressBars, value);
        }

        private decimal progressBarGapLength = 5;
        public decimal ProgressBarGapLength
        {
            get => progressBarGapLength;
            set => SetProperty(ref progressBarGapLength, value);
        }

        private int progressBarWidth = 920;
        public int ProgressBarWidth
        {
            get => progressBarWidth;
            set => SetProperty(ref progressBarWidth, value);
        }

        private int progressBarHeight = 24;
        public int ProgressBarHeight
        {
            get => progressBarHeight;
            set => SetProperty(ref progressBarHeight, value);
        }

        private int progressBarYPosition = 540;
        public int ProgressBarYPosition
        {
            get => progressBarYPosition;
            set => SetProperty(ref progressBarYPosition, value);
        }

        private Color progressBarFillColor = Color.FromRgb(155, 234, 244);
        public Color ProgressBarFillColor
        {
            get => progressBarFillColor;
            set => SetProperty(ref progressBarFillColor, value);
        }

        private Color progressBarOutlineColor = Color.FromRgb(155, 234, 244);
        public Color ProgressBarOutlineColor
        {
            get => progressBarOutlineColor;
            set => SetProperty(ref progressBarOutlineColor, value);
        }

        private bool useRzProjectTemplate;
        public bool UseRzProjectTemplate
        {
            get => useRzProjectTemplate;
            set => SetProperty(ref useRzProjectTemplate, value);
        }

        private string? rzProjectTemplatePath;
        public string? RzProjectTemplatePath
        {
            get => rzProjectTemplatePath;
            set => SetProperty(ref rzProjectTemplatePath, value);
        }

        private bool useRzVideo;
        public bool UseRzVideo
        {
            get => useRzVideo;
            set => SetProperty(ref useRzVideo, value);
        }

        private string? rzVideoPath;
        public string? RzVideoPath
        {
            get => rzVideoPath;
            set => SetProperty(ref rzVideoPath, value);
        }

        private double rzVideoLength;
        public double RzVideoLength
        {
            get => rzVideoLength;
            set => SetProperty(ref rzVideoLength, value);
        }

        private bool rzVideoAsOverlay;
        public bool RzVideoAsOverlay
        {
            get => rzVideoAsOverlay;
            set => SetProperty(ref rzVideoAsOverlay, value);
        }

        private bool launchResult = true;
        public bool LaunchResult
        {
            get => launchResult;
            set => SetProperty(ref launchResult, value);
        }
    }
}
