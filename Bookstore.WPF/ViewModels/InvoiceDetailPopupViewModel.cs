using Bookstore.Share.DTO;
using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class InvoiceDetailPopupViewModel : BaseViewModel
    {
        private bool _isOpen;
        public bool IsOpen { get => _isOpen; set { _isOpen = value; OnPropertyChanged(); } }

        // 1. Thông tin chung hóa đơn
        private InvoiceResponse _invoiceInfo;
        public InvoiceResponse InvoiceInfo { get => _invoiceInfo; set { _invoiceInfo = value; OnPropertyChanged(); } }

        // 2. Danh sách sản phẩm mua (Dùng dynamic hoặc DTO chi tiết từ API)
        private List<InvoiceDetailResponse> _danhSachSanPham;
        public List<InvoiceDetailResponse> DanhSachSanPham
        {
            get => _danhSachSanPham;
            set { _danhSachSanPham = value; OnPropertyChanged(); }
        }
        // 3. Danh sách ưu đãi áp dụng
        private List<InvoicePromoResponse> _danhSachUuDai;
        public List<InvoicePromoResponse> DanhSachUuDai
        {
            get => _danhSachUuDai;
            set { _danhSachUuDai = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; }
        public ICommand TaiHoaDonCommand { get; }

        public InvoiceDetailPopupViewModel()
        {
            CloseCommand = new RelayCommand<object>((p) => IsOpen = false);

            TaiHoaDonCommand = new RelayCommand<System.Windows.FrameworkElement>(printArea =>
            {
                if (printArea == null) return;

                // 1. LƯU LẠI MARGIN GỐC CỦA GIAO DIỆN TRÊN MÀN HÌNH
                var originalMargin = printArea.Margin;

                try
                {
                    System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();

                    var pdfPrinter = new System.Printing.LocalPrintServer()
                                        .GetPrintQueues()
                                        .FirstOrDefault(q => q.Name == "Microsoft Print to PDF");

                    if (pdfPrinter != null)
                    {
                        printDialog.PrintQueue = pdfPrinter;
                    }
                    else
                    {
                        if (printDialog.ShowDialog() != true) return;
                    }

                    // =======================================================
                    // 2. THÊM "PADDING" (LỀ GIẤY) TRƯỚC KHI IN
                    // 40 pixel tương đương khoảng 1cm lề giấy rất đẹp
                    // =======================================================
                    printArea.Margin = new System.Windows.Thickness(40);

                    double width = printDialog.PrintableAreaWidth > 0 ? printDialog.PrintableAreaWidth : 793.7;

                    printArea.Measure(new System.Windows.Size(width, double.PositiveInfinity));
                    printArea.Arrange(new System.Windows.Rect(new System.Windows.Point(0, 0), printArea.DesiredSize));
                    printArea.UpdateLayout();

                    string tenFile = $"Hoa_don_HD{InvoiceInfo.NgayTao:ddMMyy}{InvoiceInfo.MaHoaDon:D3}";

                    System.Windows.Clipboard.SetText(tenFile);

                    printDialog.PrintVisual(printArea, tenFile);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Lỗi xuất hóa đơn: {ex.Message}", "Lỗi hệ thống", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                finally
                {
                    // =======================================================
                    // 3. TRẢ LẠI MARGIN GỐC CHO GIAO DIỆN ĐỂ KHÔNG BỊ LỆCH UI
                    // =======================================================
                    printArea.Margin = originalMargin;

                    printArea.InvalidateMeasure();
                    printArea.InvalidateArrange();
                    printArea.UpdateLayout();
                }
            });
        }

        // HÀM ĐỂ BẬT POPUP LÊN (Được gọi từ InvoiceViewModel)
        public void ShowPopup(InvoiceResponse invoice, List<InvoiceDetailResponse> details, List<InvoicePromoResponse> promos)
        {
            InvoiceInfo = invoice;
            DanhSachSanPham = details;
            DanhSachUuDai = promos;
            IsOpen = true;
        }
    }
}