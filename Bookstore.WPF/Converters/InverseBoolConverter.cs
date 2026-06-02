using System;
using System.Globalization;
using System.Windows.Data;

namespace Bookstore.WPF.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        // Hàm lật ngược dữ liệu từ ViewModel (hoặc DependencyProperty) đẩy lên UI
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false; // Mặc định trả về false nếu dữ liệu bị lỗi
        }

        // Hàm lật ngược dữ liệu từ UI đẩy ngược về ViewModel (thường ít dùng với trường hợp này)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}