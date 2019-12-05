using AXVideo.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AXVideo.Converters
{
    public class DateTimeToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var datetime = (DateTime)value;
            if (datetime.Kind == DateTimeKind.Utc)
                datetime = datetime.ToLocalTime();

            //return $"{datetime.ToString("m", new CultureInfo("zh-CN"))}";
            return datetime.GetTimeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
