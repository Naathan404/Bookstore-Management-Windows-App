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
        // TODO: Đổi chuỗi object thành DTO tương ứng của bạn khi có API (VD: InvoiceDetailResponse)
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

        public InvoiceDetailPopupViewModel()
        {
            CloseCommand = new RelayCommand<object>((p) => IsOpen = false);
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