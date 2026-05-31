using Bookstore.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels.Base
{
    /// <summary>
    /// Lớp cơ sở có các thuộc tính binding cho component pagination ở View
    /// </summary>
    public abstract class BaseListViewModel : BaseViewModel
    {
        #region 1. CÁC THUỘC TÍNH CHUNG (Universal Properties)

        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set { _searchKeyword = value; OnPropertyChanged(); }
        }

        protected int _pageSize = 10;
        public int PageSize { get => _pageSize; set { _pageSize = value; OnPropertyChanged(); } }

        private int _trangHienTai = 1;
        public int TrangHienTai
        {
            get => _trangHienTai;
            set { _trangHienTai = value; OnPropertyChanged(); }
        }

        private int _tongSoTrang = 1;
        public int TongSoTrang
        {
            get => _tongSoTrang;
            set { _tongSoTrang = value; OnPropertyChanged(); }
        }

        private int _tongBanGhi = 0;
        public int TongBanGhi
        {
            get => _tongBanGhi;
            set { _tongBanGhi = value; OnPropertyChanged(); }
        }
        #endregion

        #region 2. LOGIC PHÂN TRANG CHUNG

        public ICommand PhanTrangCommand { get; }

        public BaseListViewModel()
        {
            PhanTrangCommand = new RelayCommand<string>(ExecutePhanTrang);
        }

        private void ExecutePhanTrang(string parameter)
        {
            int targetPage = TrangHienTai;

            switch (parameter)
            {
                case "First": targetPage = 1; break;
                case "Prev": if (TrangHienTai > 1) targetPage = TrangHienTai - 1; break;
                case "Next": if (TrangHienTai < TongSoTrang) targetPage = TrangHienTai + 1; break;
                case "Last": targetPage = TongSoTrang; break;
            }

            if (targetPage != TrangHienTai)
            {
                TrangHienTai = targetPage;
                // Gọi hàm lọc của class con
                ApplyFilterAndPagination();
            }
        }

        // HÀM ẢO (ABSTRACT): Ép buộc tất cả các class con kế thừa phải tự định nghĩa cách lọc dữ liệu của riêng nó!
        protected abstract void ApplyFilterAndPagination();

        #endregion
    }
}
