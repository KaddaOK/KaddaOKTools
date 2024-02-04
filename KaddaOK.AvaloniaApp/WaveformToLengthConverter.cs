using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Avalonia.Data.Converters;

namespace KaddaOK.AvaloniaApp
{
    public class WaveformToLengthConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            /*
            var sb = new StringBuilder();
            sb.AppendLine(nameof(WaveformToLengthConverter));
            for (int i = 0; i < values.Count; i++)
            {
                sb.AppendLine($"[{i}]: {values[i]?.ToString() ?? "null"}");
            }
            Debug.WriteLine(sb.ToString());
            */

            // need to know the start second of the point
            // need to know the end second of the point
            // need to know the length of the waveform in seconds
            // need to know the width of the image
            if (values.Count < 4 || values.Take(4).Any(v => v == null || v.ToString() == "(unset)")) return null;

            if (double.TryParse(values[0]?.ToString(), out double startSeconds)
                && double.TryParse(values[1]?.ToString(), out double endSeconds)
                && double.TryParse(values[2]?.ToString(), out double waveLengthSeconds)
                && double.TryParse(values[3]?.ToString(), out double totalWidth))
            {
                // there may also be an offset, which affects the position
                if (values.Count == 5 && values[4]?.ToString() != "(unset)"
                                      && double.TryParse(values[4]?.ToString(), out double startOffsetSeconds))
                {
                    startSeconds -= startOffsetSeconds;
                    endSeconds -= startOffsetSeconds;
                }

                //Debug.WriteLine($"start: {startSeconds}, end: {endSeconds}");
                var duration = endSeconds - startSeconds;
                //Debug.WriteLine($"{duration} / {waveLengthSeconds} * {totalWidth}");
                return duration / waveLengthSeconds * totalWidth;
            }

            return null;
        }
    }
}
