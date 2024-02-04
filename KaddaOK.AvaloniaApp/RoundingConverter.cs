using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace KaddaOK.AvaloniaApp
{
    public class RoundingConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double doubValue)
            {
                return Math.Round(doubValue, 2);
            }

            if (value is decimal decValue)
            {
                return Math.Round(decValue, 2);
            }


            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
