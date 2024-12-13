using System.Globalization;

namespace CardGameCorner.Resources.Converters
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imageUrl && !string.IsNullOrWhiteSpace(imageUrl))
            {
                try
                {
                    // Use HttpClient for more reliable image loading
                    return ImageSource.FromUri(new Uri(imageUrl));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Image conversion error: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}