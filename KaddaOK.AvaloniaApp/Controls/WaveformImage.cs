using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using NAudio.Wave;
using ReactiveUI;

namespace KaddaOK.AvaloniaApp.Controls
{
    public class WaveformRefreshEvent { }

    class WaveformImage : Control
    {
        private readonly Debouncer RedrawWaveformDebouncer = new();

        #region IsVertical property
        public static readonly DirectProperty<WaveformImage, bool> IsVerticalProperty =
            AvaloniaProperty.RegisterDirect<WaveformImage, bool>(
                nameof(IsVertical),
                o => o.IsVertical,
                (o, v) => o.IsVertical = v);

        private bool _isVertical;
        public bool IsVertical
        {
            get => _isVertical;
            set => SetAndRaise(IsVerticalProperty, ref _isVertical, value);
        }
        #endregion

        #region StartSeconds property
        public static readonly DirectProperty<WaveformImage, double?> StartSecondsProperty =
            AvaloniaProperty.RegisterDirect<WaveformImage, double?>(
                nameof(StartSeconds),
                o => o.StartSeconds,
                (o, v) => o.StartSeconds = v);

        private double? _startSeconds;
        public double? StartSeconds
        {
            get => _startSeconds;
            set => SetAndRaise(StartSecondsProperty, ref _startSeconds, value);
        }
        #endregion

        #region EndSeconds property
        public static readonly DirectProperty<WaveformImage, double?> EndSecondsProperty =
            AvaloniaProperty.RegisterDirect<WaveformImage, double?>(
                nameof(EndSeconds),
                o => o.EndSeconds,
                (o, v) => o.EndSeconds = v);

        private double? _endSeconds;
        public double? EndSeconds
        {
            get => _endSeconds;
            set => SetAndRaise(EndSecondsProperty, ref _endSeconds, value);
        }
        #endregion

        #region WaveStream property
        public static readonly DirectProperty<WaveformImage, WaveStream?> WaveStreamProperty =
            AvaloniaProperty.RegisterDirect<WaveformImage, WaveStream?>(
                nameof(WaveStream), 
                o => o.WaveStream, 
                (o, v) => o.WaveStream = v);

        private WaveStream? _wavestream;
        public WaveStream? WaveStream
        {
            get => _wavestream;
            set => SetAndRaise(WaveStreamProperty, ref _wavestream, value);
        }
        #endregion

        #region WaveformData property
        public static readonly DirectProperty<WaveformImage, (float min, float max)[]?> WaveformDataProperty =
            AvaloniaProperty.RegisterDirect<WaveformImage, (float min, float max)[]?>(
                nameof(WaveformData),
                o => o.WaveformData,
                (o, v) => o.WaveformData = v);

        private (float min, float max)[]? _waveformData;
        public (float min, float max)[]? WaveformData
        {
            get => _waveformData;
            set => SetAndRaise(WaveformDataProperty, ref _waveformData, value);
        }
        #endregion

        private WriteableBitmap? bitmap;
        private uint[] bitmapData = Array.Empty<uint>();

        public WaveformImage()
        {
            MessageBus.Current.Listen<WaveformRefreshEvent>()
                .Subscribe(e => {
                    InvalidateVisual();
                });
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == StartSecondsProperty ||
                change.Property == EndSecondsProperty ||
                change.Property == WaveformDataProperty || 
                change.Property == WaveStreamProperty)
            {
                InvalidateVisual();
            }
        }

        public override void Render(DrawingContext context)
        {
            if (WaveStream == null || WaveformData == null || Bounds.Width == 0 || Bounds.Height == 0)
            {
                return;
            }

            base.Render(context);

            var bitmap = Draw((int)Bounds.Width,
                (int)Bounds.Height,
                IsVertical,
                StartSeconds, EndSeconds);

            if (bitmap != null)
            {
                var rect = Bounds.WithX(0).WithY(0);
                context.DrawImage(bitmap, rect, rect);
            }
        }

        private WriteableBitmap? Draw(int width, int height, bool isVertical, double? startSeconds = null, double? endSeconds = null)
        {
            if (width == 0 || height == 0)
            {
                return null;
            }

            var bitmap = GetBitmap(width, height);
            Array.Clear(bitmapData, 0, bitmapData.Length);
            int pixelIndex = 0;
            int valueRange = isVertical ? width : height;
            var actualSamplesPerSecond = WaveformData!.Length / WaveStream!.TotalTime.TotalSeconds;
            var startSampleIndex = startSeconds == null ? 0 : (int)(actualSamplesPerSecond * startSeconds.Value);
            var endSampleIndex = endSeconds == null ? WaveformData.Length - 1 : (int)(actualSamplesPerSecond * endSeconds.Value);
            var totalSamples = endSampleIndex - startSampleIndex;

            var samplesPerPixelDecimal = (decimal)totalSamples / (isVertical ? height : width);
            var samplesPerPixel = (int)samplesPerPixelDecimal;

            while ((pixelIndex < width && !isVertical) || (isVertical && pixelIndex < height))
            {
                var segmentOffset = startSampleIndex + (pixelIndex * samplesPerPixel);
                var samplesToUse = (segmentOffset + samplesPerPixel) < endSampleIndex ? samplesPerPixel : endSampleIndex - segmentOffset;
                var currentSegment = new ArraySegment<(float min, float max)>(WaveformData, segmentOffset, samplesToUse);
                var currentPeakMin = currentSegment.Min(m => m.min);
                var currentPeakMax = currentSegment.Max(m => m.max);
                float min = 0.5f + currentPeakMin * 0.5f;
                float max = 0.5f + currentPeakMax * 0.5f;
                double yMax = Math.Clamp(max * valueRange, 0, valueRange - 1);
                double yMin = Math.Clamp(min * valueRange, 0, valueRange - 1);
                DrawPeak(bitmapData, (int)width, pixelIndex, (int)Math.Round(yMin), (int)Math.Round(yMax), isVertical);
                pixelIndex++;
            }

            using (var frameBuffer = bitmap.Lock())
            {
                CopyToNative(bitmapData, 0, frameBuffer.Address, bitmapData.Length);
            }
            return bitmap;
        }

        private static unsafe void CopyToNative<T>(T[] source, int startIndex, IntPtr destination, int length)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (destination == IntPtr.Zero)
                throw new ArgumentNullException(nameof(destination));

            new Span<T>(source, startIndex, length).CopyTo(new Span<T>((void*)destination, length));
        }

        private WriteableBitmap GetBitmap(int desiredWidth, int desiredHeight)
        {
            if (desiredWidth == 0 || desiredHeight == 0)
            {
                throw new InvalidOperationException("Can't create a bitmap with 0 height or width");
            }
            if (bitmap == null || (int)bitmap.Size.Width != (int)desiredWidth || (int)bitmap.Size.Height != (int)desiredHeight)
            {
                bitmap?.Dispose();
                var size = new PixelSize(desiredWidth, desiredHeight);
                bitmap = new WriteableBitmap(
                    size, new Vector(96, 96),
                    Avalonia.Platform.PixelFormat.Rgba8888,
                    Avalonia.Platform.AlphaFormat.Unpremul);
                bitmapData = new uint[size.Width * size.Height];
            }
            return bitmap;
        }

        private void DrawPeak(uint[] data, int width, int x, int y1, int y2, bool isVertical)
        {
            const uint color = 0xff946EFF;
            if (y1 > y2)
            {
                (y2, y1) = (y1, y2);
            }
            for (var y = y1; y <= y2; ++y)
            {
                if (isVertical)
                {
                    data[x * width + y] = color;
                }
                else
                {
                    data[x + width * y] = color;
                }
            }
        }
    }
}
