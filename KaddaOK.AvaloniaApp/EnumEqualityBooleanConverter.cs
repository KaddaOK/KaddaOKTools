using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Newtonsoft.Json.Linq;

namespace KaddaOK.AvaloniaApp
{
    public class EnumEqualityBooleanConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count != 2 || values.Any(v => v == null || v.ToString() == "(unset)")) return null;

            if (values[0] != values[1] && values[0]?.ToString() == values[1]?.ToString())
            {
                if (values[0] is Enum enum1)
                {
                    if (values[1] is Enum enum2)
                    {
                        return enum1.Equals(enum2);
                    }
                }
            }

            return values[0] == values[1];
        }
    }
}
