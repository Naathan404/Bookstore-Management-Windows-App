using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookstore.WPF.Views.Popup
{
    /// <summary>
    /// Interaction logic for ReceiptPopup.xaml
    /// </summary>
    public partial class ReceiptPopup : UserControl
    {
        public ReceiptPopup()
        {
            InitializeComponent();
        }

        // 1. CHẶN PHÍM BẤM
        private void TxtSoTienThu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Regex "[^0-9]+" có nghĩa là: Bất cứ thứ gì KHÔNG PHẢI là số từ 0-9 sẽ bị chặn lại
            // Điều này tự động khóa luôn cả dấu chấm (.) và dấu phẩy (,)
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        // 2. FORMAT KHI GÕ & QUẢN LÝ CON TRỎ CHUỘT
        private void TxtSoTienThu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            int cursorPosition = textBox.SelectionStart;

            // Lọc sạch mọi thứ (đề phòng người dùng click chuột phải chọn Paste)
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

            // Dùng decimal thay vì long để khớp hoàn toàn với Backend
            if (decimal.TryParse(rawText, out decimal value))
            {
                // Ép định dạng 100,000 (dấu phẩy)
                string formattedText = value.ToString("N0", new CultureInfo("en-US"));

                // Chỉ cập nhật nếu chuỗi có sự thay đổi (chống lỗi vòng lặp)
                if (textBox.Text != formattedText)
                {
                    int oldLength = textBox.Text.Length;
                    textBox.Text = formattedText;

                    // Giữ cho con trỏ chuột không bị nhảy lung tung khi dấu phẩy được thêm vào
                    textBox.SelectionStart = cursorPosition + (formattedText.Length - oldLength);
                }
            }
        }
    }
}
