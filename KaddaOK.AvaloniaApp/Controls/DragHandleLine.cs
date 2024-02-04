using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.Shapes;

namespace KaddaOK.AvaloniaApp.Controls
{
    public class DragHandleLine : Line
    {
        public static readonly StyledProperty<double?> ContainerTotalSecondsProperty = AvaloniaProperty.Register<DragHandleLine, double?>(nameof(ContainerTotalSeconds));
        public double? ContainerTotalSeconds
        {
            get => GetValue(ContainerTotalSecondsProperty);
            set => SetValue(ContainerTotalSecondsProperty, value);
        }

        public static readonly StyledProperty<double?> ContainerOffsetSecondsProperty = AvaloniaProperty.Register<DragHandleLine, double?>(nameof(ContainerOffsetSeconds));
        public double? ContainerOffsetSeconds
        {
            get => GetValue(ContainerOffsetSecondsProperty);
            set => SetValue(ContainerOffsetSecondsProperty, value);
        }

        public static readonly StyledProperty<double?> ContainerWidthProperty = AvaloniaProperty.Register<DragHandleLine, double?>(nameof(ContainerWidth));
        public double? ContainerWidth
        {
            get => GetValue(ContainerWidthProperty);
            set => SetValue(ContainerWidthProperty, value);
        }
    }
}
