using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace KaddaOK.AvaloniaApp.Controls
{
    public partial class OverlayRectangle : UserControl
    {
        public static readonly StyledProperty<IBrush?> StrokeProperty = Shape.StrokeProperty.AddOwner<OverlayRectangle>();
        public IBrush? Stroke
        {
            get => GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }
        public static readonly StyledProperty<double> StrokeThicknessProperty = Shape.StrokeThicknessProperty.AddOwner<OverlayRectangle>();
        public double StrokeThickness
        {
            get => GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        public static readonly StyledProperty<IBrush?> FillProperty = Shape.FillProperty.AddOwner<OverlayRectangle>();
        public IBrush? Fill
        {
            get => GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly StyledProperty<double?> StartSecondProperty = AvaloniaProperty.Register<OverlayRectangle, double?>(nameof(StartSecond));
        public double? StartSecond
        {
            get => GetValue(StartSecondProperty);
            set => SetValue(StartSecondProperty, value);
        }

        public static readonly StyledProperty<double?> EndSecondProperty = AvaloniaProperty.Register<OverlayRectangle, double?>(nameof(EndSecond));
        public double? EndSecond
        {
            get => GetValue(EndSecondProperty);
            set => SetValue(EndSecondProperty, value);
        }

        public static readonly StyledProperty<double?> StartOffsetSecondProperty = AvaloniaProperty.Register<OverlayRectangle, double?>(nameof(StartOffsetSecond));
        public double? StartOffsetSecond
        {
            get => GetValue(StartOffsetSecondProperty);
            set => SetValue(StartOffsetSecondProperty, value);
        }

        public static readonly StyledProperty<bool> AllowDragProperty = AvaloniaProperty.Register<OverlayRectangle, bool>(nameof(AllowDrag));
        public bool AllowDrag
        {
            get => GetValue(AllowDragProperty);
            set => SetValue(AllowDragProperty, value);
        }

        public static readonly StyledProperty<bool> IsVerticalProperty = AvaloniaProperty.Register<OverlayRectangle, bool>(nameof(IsVertical));
        public bool IsVertical
        {
            get => GetValue(IsVerticalProperty);
            set => SetValue(IsVerticalProperty, value);
        }

        public static readonly DirectProperty<OverlayRectangle, WaveformDraw?> WaveformDrawProperty =
            AvaloniaProperty.RegisterDirect<OverlayRectangle, WaveformDraw?>(nameof(WaveformDraw), o => o.WaveformDraw, (o, v) => o.WaveformDraw = v);

        private WaveformDraw? _waveformDraw;

        public WaveformDraw? WaveformDraw
        {
            get => _waveformDraw;
            set => SetAndRaise(WaveformDrawProperty, ref _waveformDraw, value);
        }

        public OverlayRectangle()
        {
            InitializeComponent();
        }
    }
}
