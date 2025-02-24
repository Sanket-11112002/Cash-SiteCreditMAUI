using System.Globalization;

namespace CardGameCorner.Resources.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLower() switch
                {
                    "rejected" => Color.FromArgb("#FF0000"),  // Pure red
                    "canceled" => Color.FromArgb("#FF9933"),  // Bright orange
                    "completed" => Color.FromArgb("#008000"),  // Pure green
                    "pending" => Color.FromArgb("#3399FF"),   // Bright blue
                    _ => Color.FromArgb("#008000")           // Medium gray
                };
            }
            return Color.FromArgb("#008000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}