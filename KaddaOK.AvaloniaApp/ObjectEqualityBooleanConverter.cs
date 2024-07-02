using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace KaddaOK.AvaloniaApp
{
    public class ObjectEqualityBooleanConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count != 2 || values.Any(v => v == null || v.ToString() == "(unset)")) return null;

            return values[0] == values[1];
        }
    }
}
