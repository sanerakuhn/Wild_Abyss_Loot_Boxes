using System.Collections;
using System.Globalization;

namespace Wild_Abyss_Loot_Boxes
{
    public class RarityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string rarity)
            {
                // Handle "varies" as "mundane"
                rarity = rarity.ToLower() == "varies" ? "non-magical" : rarity.ToLower();

                return rarity switch
                {
                    "non-magical" => Colors.Gray,
                    "common" => Color.FromArgb("#D3D3D3"),
                    "uncommon" => Color.FromArgb("#7CFC00"),
                    "rare" => Color.FromArgb("#00BFFF"),
                    "very rare" => Color.FromArgb("#DA70D6"),
                    "legendary" => Color.FromArgb("#FFA500"),
                    "artifact" => Color.FromArgb("#FF6347"),
                    _ => Colors.Gray
                };
            }

            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullOrEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable enumerable)
            {
                return enumerable.GetEnumerator().MoveNext();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GreaterThanZeroToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
                return count > 0;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
