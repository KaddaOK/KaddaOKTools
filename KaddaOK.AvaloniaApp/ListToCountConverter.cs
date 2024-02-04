using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace KaddaOK.AvaloniaApp
{
    public class ListToCountConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var list = value as IList;
            
            if (list == null) return null;

            return list.Count;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
