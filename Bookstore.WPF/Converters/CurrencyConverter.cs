using System.Globalization;
using System.Windows.Data;

namespace Bookstore.WPF.Converters
{

    public class CurrencyConverter : IValueConverter
    {
        // View Model (số) -> Giao diện (chuỗi có dấu phẩy)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d) return d.ToString("N0", new CultureInfo("en-US"));
            return value;
        }

        // Giao diện (chuỗi) -> View Model (số thuần)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Lọc sạch dấu phẩy/chấm do UI sinh ra trước khi đẩy về ViewModel
            string strValue = value?.ToString().Replace(",", "").Replace(".", "");
            if (decimal.TryParse(strValue, out decimal result)) return result;
            return 0m;
        }
    }
}