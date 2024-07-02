using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.AvaloniaApp
{
    internal class TimingWordIsPlayingConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isRunning = values[0] as bool? ?? false;
            bool isRecording = values[1] as bool? ?? false;
            bool isPlaying = values[2] as bool? ?? false;
            bool startHasBeenManuallySet = values[3] as bool? ?? false;
            double currentPlaybackPositionSeconds = values[4] as double? ?? 0;
            double startSecond = values[5] as double? ?? 0;
            bool endHasBeenManuallySet = values[6] as bool? ?? false;
            double endSecond = values[7] as double? ?? 0;

            return (isRunning && isRecording) 
                   || 
                   (isPlaying && !isRecording 
                              && startHasBeenManuallySet && currentPlaybackPositionSeconds > startSecond 
                              && endHasBeenManuallySet && currentPlaybackPositionSeconds < endSecond);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
