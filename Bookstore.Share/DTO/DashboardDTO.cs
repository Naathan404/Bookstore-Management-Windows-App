public class DashboardDTO
{
    // Các con số thống kê tổng
    public decimal DoanhThu { get; set; }
    public decimal LoiNhuan { get; set; }
    public double PhanTramLoiNhuan { get; set; }
    public decimal ChiPhi { get; set; }
    public int KhachHangMoi { get; set; }
    public int SoHoaDon { get; set; }

    // Các danh sách (Dùng chung model hoặc tạo DTO riêng tương tự model ở WPF)
    //public List<TopBookDTO> TopBooks { get; set; }
    //public List<InventoryItemDTO> InventoryItems { get; set; }
    //public List<StockWarningDTO> StockWarnings { get; set; }
    //public List<ReceiptDTO> HoaDonHomNay { get; set; }
    //public List<ReceiptDTO> PhieuThuHomNay { get; set; }

    // Dữ liệu biểu đồ (Tạm trả về Dictionary để WPF tự parse)
    public Dictionary<string, double> ChartData { get; set; }
}