namespace Bookstore.Share.DTOs
{
    // ================================================================
    // BỘ LỌC — gửi từ WPF client lên API
    // ================================================================
    public class ReportFilterDto
    {
        /// <summary>0 = Doanh thu; Lợi nhuận | 1 = Tồn kho | 2 = Công nợ</summary>
        public int ReportType { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        /// <summary>Lọc theo nhân viên (NguoiTao). Null = tất cả.</summary>
        public string? StaffName { get; set; }

        /// <summary>Lọc theo thể loại sách. Null = tất cả.</summary>
        public string? CategoryName { get; set; }

        /// <summary>Lọc theo tên khách hàng (cho báo cáo Công nợ). Null = tất cả.</summary>
        public string? CustomerName { get; set; }
    }

    // ================================================================
    // KẾT QUẢ — trả về từ API về WPF client
    // ================================================================
    public class ReportResultDto
    {
        // --- Bảng chi tiết (chỉ 1 list được fill tùy ReportType) ---
        public List<RevenueReportRowDto>? RevenueRows { get; set; }
        public List<InventoryReportRowDto>? InventoryRows { get; set; }
        public List<DebtReportRowDto>? DebtRows { get; set; }

        // --- Dữ liệu biểu đồ: Doanh thu (Stacked Column) ---
        /// <summary>Nhãn trục X: ["01/04", "02/04", ...]</summary>
        public List<string>? RevenueDateLabels { get; set; }

        /// <summary>Giá vốn hàng bán theo ngày (phần đỏ)</summary>
        public List<decimal>? RevenueCostSeries { get; set; }

        /// <summary>Lợi nhuận gộp theo ngày (phần xanh)</summary>
        public List<decimal>? RevenueProfitSeries { get; set; }

        // --- Dữ liệu biểu đồ: Tồn kho (Horizontal Bar) ---
        /// <summary>Tên sách — nhãn trục Y</summary>
        public List<string>? InventoryBarLabels { get; set; }

        /// <summary>Giá trị tồn kho tương ứng — thanh ngang</summary>
        public List<double>? InventoryBarValues { get; set; }

        // --- Dữ liệu biểu đồ: Công nợ (Line) ---
        /// <summary>Nhãn trục X theo ngày</summary>
        public List<string>? DebtAxisLabels { get; set; }

        /// <summary>Nợ phát sinh mới theo ngày (đường đỏ)</summary>
        public List<decimal>? DebtNewSeries { get; set; }

        /// <summary>Nợ đã thu theo ngày (đường xanh)</summary>
        public List<decimal>? DebtPaidSeries { get; set; }
    }

    // ================================================================
    // ROW DTOs — từng dòng trong DataGrid
    // ================================================================

    /// <summary>Một dòng trong báo cáo Doanh thu & Lợi nhuận (nhóm theo ngày).</summary>
    public class RevenueReportRowDto
    {
        public DateTime Date { get; set; }

        /// <summary>Số hóa đơn phát sinh trong ngày</summary>
        public int InvoiceCount { get; set; }

        /// <summary>Tổng số lượng sách bán</summary>
        public int BooksSold { get; set; }

        /// <summary>Tổng tiền hàng (DonGia * SoLuong, chưa trừ giảm giá)</summary>
        public decimal TotalAmount { get; set; }

        /// <summary>Tiền giảm giá (HoaDon.GiamGia). TODO: bổ sung nếu bảng HoaDon có cột này.</summary>
        public decimal Discount { get; set; }

        /// <summary>Doanh thu thuần = TotalAmount - Discount</summary>
        public decimal NetRevenue { get; set; }

        /// <summary> Giá vốn </summary>
        public decimal TotalCost { get; set; }

        /// <summary>Lợi nhuận gộp = NetRevenue - GiaVon*SoLuong</summary>
        public decimal GrossProfit { get; set; }
    }

    /// <summary>Một dòng trong báo cáo Tồn kho (theo từng phiên bản sách).</summary>
    public class InventoryReportRowDto
    {
        public string BookId { get; set; } = string.Empty;
        public string BookName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>Tồn đầu kỳ = TonKho hiện tại - Nhập trong kỳ + Bán trong kỳ (back-calculate)</summary>
        public int OpeningQty { get; set; }

        /// <summary>Số lượng nhập kho trong kỳ</summary>
        public int ImportedQty { get; set; }

        /// <summary>Số lượng xuất (bán) trong kỳ</summary>
        public int SoldQty { get; set; }

        /// <summary>Tồn cuối kỳ = TonKho hiện tại của PhienBanSach</summary>
        public int ClosingQty { get; set; }

        /// <summary>Giá trị tồn kho = ClosingQty * Giá vốn gần nhất</summary>
        public decimal StockValue { get; set; }
    }

    /// <summary>Một dòng trong báo cáo Công nợ (theo từng khách hàng).</summary>
    public class DebtReportRowDto
    {
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>Nợ đầu kỳ (trước FromDate)</summary>
        public decimal OpeningDebt { get; set; }

        /// <summary>Phát sinh trong kỳ = tổng HoaDon chưa thanh toán đủ</summary>
        public decimal NewDebt { get; set; }

        /// <summary>Đã thu trong kỳ = tổng PhieuThuTien</summary>
        public decimal PaidDebt { get; set; }

        /// <summary>Nợ cuối kỳ = OpeningDebt + NewDebt - PaidDebt</summary>
        public decimal ClosingDebt { get; set; }

        /// <summary>Dùng cho DataTrigger tô màu trong WPF</summary>
        public bool HasDebt => ClosingDebt > 0;
    }
}