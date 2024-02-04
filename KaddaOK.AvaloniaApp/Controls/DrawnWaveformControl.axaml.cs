using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using KaddaOK.Library;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;

namespace KaddaOK.AvaloniaApp.Controls
{
    public partial class DrawnWaveformControl : UserControl
    {
        #region WaveStream property
        public static readonly DirectProperty<DrawnWaveformControl, WaveStream?> WaveStreamProperty =
            AvaloniaProperty.RegisterDirect<DrawnWaveformControl, WaveStream?>(nameof(WaveStream), o => o.WaveStream, (o, v) => o.WaveStream = v,
                defaultBindingMode: BindingMode.TwoWay);

        private WaveStream? _wavestream;
        public WaveStream? WaveStream
        {
            get => _wavestream;
            set => SetAndRaise(WaveStreamProperty, ref _wavestream, value);
        }
        #endregion

        #region WaveFloats property
        public static readonly DirectProperty<DrawnWaveformControl, (float min, float max)[]?> WaveFloatsProperty =
            AvaloniaProperty.RegisterDirect<DrawnWaveformControl, (float min, float max)[]?>(nameof(WaveFloats), o => o.WaveFloats, (o, v) => o.WaveFloats = v,
                defaultBindingMode: BindingMode.TwoWay);

        private (float min, float max)[]? _waveFloats;
        public (float min, float max)[]? WaveFloats
        {
            get => _waveFloats;
            set => SetAndRaise(WaveFloatsProperty, ref _waveFloats, value);
        }
        #endregion

        #region Loading property
        public static readonly DirectProperty<DrawnWaveformControl, bool> LoadingProperty =
            AvaloniaProperty.RegisterDirect<DrawnWaveformControl, bool>(nameof(Loading), o => o.Loading, (o, v) => o.Loading = v);

        private bool _loading;

        public bool Loading
        {
            get => _loading;
            set => SetAndRaise(LoadingProperty, ref _loading, value);
        }
        #endregion

        #region Waveform property
        public static readonly DirectProperty<DrawnWaveformControl, WaveformDraw?> WaveformProperty =
            AvaloniaProperty.RegisterDirect<DrawnWaveformControl, WaveformDraw?>(nameof(Waveform), o => o.Waveform, (o, v) => o.Waveform = v,
                defaultBindingMode: BindingMode.TwoWay);

        private WaveformDraw? _waveform;

        public WaveformDraw? Waveform
        {
            get => _waveform;
            set => SetAndRaise(WaveformProperty, ref _waveform, value);
        }
        #endregion

        #region WaveformFilePath property
        public static readonly DirectProperty<DrawnWaveformControl, string?> WaveformFilePathProperty =
            AvaloniaProperty.RegisterDirect<DrawnWaveformControl, string?>(nameof(WaveformFilePath), o => o.WaveformFilePath, (o, v) => o.WaveformFilePath = v,
                defaultBindingMode: BindingMode.TwoWay);

        private string? _waveformFilePath;

        public string? WaveformFilePath
        {
            get => _waveformFilePath;
            set => SetAndRaise(WaveformFilePathProperty, ref _waveformFilePath, value);
        }

        #endregion

        #region OpenTitle property
        public static readonly DirectProperty<DrawnWaveformControl, string?> OpenTitleProperty =
            AvaloniaProperty.RegisterDirect<DrawnWaveformControl, string?>(nameof(OpenTitle), o => o.OpenTitle, (o, v) => o.OpenTitle = v);

        private string? _openTitle;

        public string? OpenTitle
        {
            get => _openTitle;
            set => SetAndRaise(OpenTitleProperty, ref _openTitle, value);
        }

        #endregion

        #region CanSelectIfEmpty property
        public static readonly DirectProperty<DrawnWaveformControl, bool> CanSelectIfEmptyProperty =
            AvaloniaProperty.RegisterDirect<DrawnWaveformControl, bool>(nameof(CanSelectIfEmpty), o => o.CanSelectIfEmpty, (o, v) => o.CanSelectIfEmpty = v);

        private bool _canSelectIfEmpty;

        public bool CanSelectIfEmpty
        {
            get => _canSelectIfEmpty;
            set => SetAndRaise(CanSelectIfEmptyProperty, ref _canSelectIfEmpty, value);
        }
        #endregion

        private IAudioFromFile FileAudioReader { get; }
        private IMinMaxFloatWaveStreamSampler Sampler { get; }
        public DrawnWaveformControl()
        {
            InitializeComponent();
            FileAudioReader = App.ServiceProvider.GetRequiredService<IAudioFromFile>();
            Sampler = App.ServiceProvider.GetRequiredService<IMinMaxFloatWaveStreamSampler>();
        }

        private async void WaveformSelectButton_Clicked(object? sender, RoutedEventArgs args)
        {
            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = OpenTitle ?? "Select the wave file",
                FileTypeFilter = new FilePickerFileType[] { new ("Audio file")
                {
                    Patterns = new[] { "*.wav", "*.flac" }, MimeTypes = new[] { "audio/wav", "audio/flac" }
                } }
            };
            try
            {
                var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
                var result = results?.FirstOrDefault();
                if (result != null)
                {
                    Loading = true;
                    if (Waveform == null) Waveform = new WaveformDraw();
                    var previousFilePath = WaveformFilePath;
                    WaveformFilePath = result.TryGetLocalPath();
                    if (WaveformFilePath != previousFilePath)
                    {
                        if (!string.IsNullOrEmpty(WaveformFilePath))
                        {
                            var waveStream = FileAudioReader.GetAudioFromFile(WaveformFilePath);
                            var dataSamplingFactor = 20; // TODO: too low impacts performance, too high crashes the app; dependent on input audio length
                            var waveFloats = await Sampler.GetAllFloatsAsync(waveStream, dataSamplingFactor);
                            WaveStream = waveStream;
                            WaveFloats = waveFloats;

                            Waveform.WaveformLengthSeconds = WaveStream?.TotalTime.TotalSeconds;
                            Waveform.WaveformLengthText = WaveStream?.TotalTime.ToString("m\\:ss\\.ff");
                        }
                    }

                    Loading = false;
                }
            }
            catch (Exception)
            {
                // The user canceled or something went wrong
                // TODO: show error
            }
        }
    }
}
