using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bookstore.WPF.Helpers
{
    public static class TextBoxFormatHelper
    {
        // Khai báo một Attached Property có tên là "IsCurrency"
        public static readonly DependencyProperty IsCurrencyProperty =
            DependencyProperty.RegisterAttached(
                "IsCurrency",
                typeof(bool),
                typeof(TextBoxFormatHelper),
                new UIPropertyMetadata(false, OnIsCurrencyChanged));

        public static bool GetIsCurrency(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCurrencyProperty);
        }

        public static void SetIsCurrency(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCurrencyProperty, value);
        }

        // Khi Property này được gán True/False trên XAML, hàm này sẽ chạy
        private static void OnIsCurrencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                // Hủy đăng ký sự kiện cũ (tránh lỗi rò rỉ bộ nhớ)
                textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                textBox.TextChanged -= TextBox_TextChanged;

                // Nếu gán bằng True thì đăng ký sự kiện mới
                if ((bool)e.NewValue)
                {
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    textBox.TextChanged += TextBox_TextChanged;
                }
            }
        }

        // ==============================================================
        // BÊ NGUYÊN SI 2 HÀM LOGIC CỦA BẠN VÀO ĐÂY
        // ==============================================================

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private static bool _isUpdating = false; // Biến cờ chặn đệ quy

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating || !(sender is TextBox textBox)) return;

            _isUpdating = true; // Bật cờ chặn

            try
            {
                int oldCursor = textBox.SelectionStart;
                string rawText = Regex.Replace(textBox.Text, "[^0-9]", "");

                if (string.IsNullOrEmpty(rawText))
                {
                    textBox.Text = "0";
                    textBox.SelectionStart = 1;
                }
                else if (decimal.TryParse(rawText, out decimal value))
                {
                    string formattedText = value.ToString("N0", new CultureInfo("en-US"));

                    if (textBox.Text != formattedText)
                    {
                        int oldLength = textBox.Text.Length;
                        textBox.Text = formattedText;

                        // TÍNH TOÁN VỊ TRÍ CON TRỎ MỚI ĐẢM BẢO KHÔNG ÂM
                        int newCursor = oldCursor + (formattedText.Length - oldLength);

                        // ÉP VỊ TRÍ CON TRỎ NẰM TRONG KHOẢNG [0, Độ dài chuỗi]
                        textBox.SelectionStart = Math.Max(0, Math.Min(newCursor, formattedText.Length));
                    }
                }
            }
            finally
            {
                _isUpdating = false; // Tắt cờ chặn
            }
        }
    }
}
