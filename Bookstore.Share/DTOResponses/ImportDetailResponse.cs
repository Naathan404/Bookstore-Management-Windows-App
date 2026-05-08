using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTOResponses
{
    public class ImportDetailResponse
    {
        public int MaPhieuNhap { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public int SoLuong { get; set; } = 0;
        public decimal DonGiaNhap { get; set; }
    }
}
