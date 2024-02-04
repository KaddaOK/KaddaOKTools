using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using KaddaOK.Library;
using NAudio.Wave;
using SkiaSharp;

namespace KaddaOK.AvaloniaApp
{
    public class WaveformDraw : ObservableBase
    {
        private readonly Debouncer RedrawWaveformDebouncer = new();

        private int _desiredPeakHeight;
        public int DesiredPeakHeight
        {
            get => _desiredPeakHeight;
            set => SetProperty(ref _desiredPeakHeight, value);
        }

        private int _desiredImageWidth;
        public int DesiredImageWidth
        {
            get => _desiredImageWidth;
            set => SetProperty(ref _desiredImageWidth, value);
        }


        private IImage? _currentImageSource;
        public IImage? CurrentImageSource
        {
            get => _currentImageSource;
            set => SetProperty(ref _currentImageSource, value);
        }


        private bool _verticalImage;
        public bool VerticalImage
        {
            get => _verticalImage;
            set => SetProperty(ref _verticalImage, value);
        }

        private bool _drawing;
        public bool Drawing
        {
            get => _drawing;
            set => SetProperty(ref _drawing, value);
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

        public WaveformDraw()
        {
            Drawing = false;
        }

        // TODO: the only thing this does now is set WaveformLengthSeconds and WaveformLengthText.  The whole class should be eliminateable
        public async Task RedrawWaveform(WaveStream? wavestreamToRender)
        {
            if (wavestreamToRender != null)
            {
                Drawing = true;
                
                await RedrawWaveformDebouncer.Debounce(async () =>
                {
                    // update the length seconds and text if needed
                    WaveformLengthSeconds = wavestreamToRender.TotalTime.TotalSeconds;
                    WaveformLengthText = wavestreamToRender.TotalTime.ToString("m\\:ss\\.ff");

                    /*
                     if (wavestreamToRender != null)
                    {
                        var renderer = new WaveformImageRenderer();
                        var peakHeight = Math.Max(DesiredPeakHeight, 100);
                        var waveWidth = Math.Max(DesiredImageWidth, 100);
                        Debug.WriteLine($"Redrawing {WaveformLengthText} for {peakHeight}x{waveWidth}...");
                        if (wavestreamToRender.CanSeek)
                            wavestreamToRender.Seek(0, SeekOrigin.Begin);
                        var audioInputWaveBitmap = await renderer.RenderWaveform(wavestreamToRender,
                            peakHeight,
                            waveWidth);
                        if (VerticalImage)
                        {
                            audioInputWaveBitmap = renderer.Rotate(audioInputWaveBitmap);
                        }

                        var skdata = SKImage.FromBitmap(audioInputWaveBitmap).Encode();

                        CurrentImageSource = new Bitmap(new MemoryStream(skdata.ToArray()));
                        Debug.WriteLine($"Actual width turned out to be {CurrentImageSource.Size.Width}");
                        Drawing = false;
                    }
                    */
                });
            }
        }
    }
}
