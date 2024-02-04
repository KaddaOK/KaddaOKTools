using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using KaddaOK.Library.Ytmm;

namespace KaddaOK.AvaloniaApp
{
    public class LyricItemsToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var lyricItems = value as IList<LyricItem>;

            if (lyricItems == null) return null;

            return string.Join(" / ", lyricItems.Select(s => s.text));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
