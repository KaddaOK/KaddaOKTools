using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Avalonia.Data.Converters;

namespace KaddaOK.AvaloniaApp
{
    public class WaveformToPointConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            
            var sb = new StringBuilder();
            sb.Append(nameof(WaveformToPointConverter));
            for (int i = 0; i < values.Count; i++)
            {
                sb.Append($"[{i}]: {values[i]?.ToString() ?? "null"}");
            }
            Debug.WriteLine(sb.ToString());

            double? returnValue = null;

            // need to know the second of the point
            // need to know the length of the waveform in seconds
            // need to know the width of the image
            if (values.Count < 3 || values.Take(3).Any(v => v == null || v.ToString() == "(unset)")) return null;

            if (double.TryParse(values[0]?.ToString(), out double pointSeconds)
                && double.TryParse(values[1]?.ToString(), out double waveLengthSeconds)
                && double.TryParse(values[2]?.ToString(), out double totalWidth))
            {
                // there may also be an offset, which affects the position
                if (values.Count == 4 && values[3]?.ToString() != "(unset)"
                    && double.TryParse(values[3]?.ToString(), out double startOffsetSeconds))
                {
                    pointSeconds -= startOffsetSeconds;
                }

                //Debug.WriteLine($"{pointSeconds} / {waveLengthSeconds} * {totalWidth}");
                returnValue = pointSeconds / waveLengthSeconds * totalWidth;
            }

            sb.Append($" = {returnValue}");
            Debug.WriteLine(sb.ToString());

            return returnValue;
        }
    }
}
