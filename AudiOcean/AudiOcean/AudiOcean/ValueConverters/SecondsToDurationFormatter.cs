using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AudiOcean.ValueConverters
{
    public class SecondsToDurationFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is double)
            {

                Int32 secondValue = (int)(double)value;
                Int32 minutes = secondValue / 60;
                Int32 newSecondValue = secondValue % 60;
                return $"{minutes}:{newSecondValue.ToString("00")}";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string)
            {
                try
                {
                    Tuple<string, string> minuteSecond = new Tuple<string, string>((value as string).Split(':')[0], (value as string).Split(':')[1]);
                    int minutes = int.Parse(minuteSecond.Item1);
                    int seconds = int.Parse(minuteSecond.Item2);
                    seconds += seconds + (minutes * 60);

                    return seconds;
                }
                catch (FormatException) { return null; }
                catch (IndexOutOfRangeException) { return null; }
                catch (InvalidCastException) { return null; }
            }
            return null;
        }
    }
}
