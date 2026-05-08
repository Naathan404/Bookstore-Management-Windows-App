using Bookstore.Share.DTOResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class ImportRequest
    {
        public int MaNhaCungCap { get; set; } = 0;
        public string NguoiTao { get; set; } = string.Empty;
        public List<ImportDetailRequest> ChiTietSach { get; set; } = new();
    }

    public class ImportDetailRequest
    {
        public int MaPhieuNhap { get; set; } = 0;
        public string ISBN { get; set; } = string.Empty;
        public int SoLuong { get; set; } = 0;
        public decimal DonGiaNhap { get; set; } = 0;
    }

}
