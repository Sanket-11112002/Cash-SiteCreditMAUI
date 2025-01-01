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
                    "rejected" => Color.FromArgb("#FF4444"),  // Bright red
                    "canceled" => Color.FromArgb("#FF9933"),  // Bright orange
                    "completed" => Color.FromArgb("#44BB44"), // Bright green
                    "pending" => Color.FromArgb("#3399FF"),   // Bright blue
                    _ => Color.FromArgb("#888888")           // Medium gray
                };
            }
            return Color.FromArgb("#888888");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}