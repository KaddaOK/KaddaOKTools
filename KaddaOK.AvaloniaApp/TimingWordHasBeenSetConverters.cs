using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library.Ytmm;

namespace KaddaOK.AvaloniaApp
{
    public class TimingWordStartHasBeenSetConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var timingWord = value as TimingWord;

            if (timingWord == null) return true;

            return timingWord.StartHasBeenManuallySet;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimingWordEndHasBeenSetConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var timingWord = value as TimingWord;

            if (timingWord == null) return true;

            return timingWord.EndHasBeenManuallySet;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
