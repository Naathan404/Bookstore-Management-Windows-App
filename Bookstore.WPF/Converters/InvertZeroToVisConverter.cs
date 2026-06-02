using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Bookstore.WPF.Converters
{
    public class InvertZeroToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Nếu giá trị (tiền giảm) lớn hơn 0 thì hiện, ngược lại thì ẩn đi
            if (value is decimal decimalValue)
            {
                return decimalValue > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            if (value is int intValue)
            {
                return intValue > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            if (value is double doubleValue)
            {
                return doubleValue > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
