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

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            int cursorPosition = textBox.SelectionStart;
            string rawText = Regex.Replace(textBox.Text, "[^0-9]", "");

            if (string.IsNullOrEmpty(rawText))
            {
                if (textBox.Text != "0")
                {
                    textBox.Text = "0";
                    textBox.SelectionStart = 1;
                }
                return;
            }

            if (decimal.TryParse(rawText, out decimal value))
            {
                string formattedText = value.ToString("N0", new CultureInfo("en-US"));

                if (textBox.Text != formattedText)
                {
                    int oldLength = textBox.Text.Length;
                    textBox.Text = formattedText;
                    textBox.SelectionStart = cursorPosition + (formattedText.Length - oldLength);
                }
            }
        }
    }
}
