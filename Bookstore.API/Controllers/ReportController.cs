using Bookstore.API.Data;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // ================================================================
        // FILTER LIST ENDPOINTS
        // ViewModel gọi 3 endpoint này khi khởi động để đổ dữ liệu vào
        // các ComboBox bộ lọc phụ.
        // ================================================================

        /// <summary>Danh sách tên nhân viên (NguoiTao) dùng cho bộ lọc Doanh thu.</summary>
        [HttpGet("filter/staffs")]
        public async Task<IActionResult> GetStaffList()
        {
            var staffs = await _context.HoaDon
                .Where(x => x.NguoiTao != null)
                .Select(x => x.NguoiTao!)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            return Ok(staffs);
        }

        /// <summary>Danh sách tên thể loại sách dùng cho bộ lọc Doanh thu / Tồn kho.</summary>
        [HttpGet("filter/categories")]
        public async Task<IActionResult> GetCategoryList()
        {
            var categories = await _context.TheLoai
                .Where(x => x.TenTheLoai != null)
                .Select(x => x.TenTheLoai!)
                .OrderBy(x => x)
                .ToListAsync();

            return Ok(categories);
        }

        /// <summary>Danh sách tên khách hàng dùng cho bộ lọc Công nợ.</summary>
        [HttpGet("filter/customers")]
        public async Task<IActionResult> GetCustomerList()
        {
            var customers = await _context.LoaiKhachHang
                .Select(x => x.TenLoaiKhachHang)
                .ToListAsync();

            return Ok(customers);
        }

        // ================================================================
        // MAIN REPORT ENDPOINT
        // ViewModel POST body: ReportFilterDto
        // Response: ReportResultDto
        // ================================================================

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateReport([FromBody] ReportFilterDto filter)
        {
            try
            {
                var fromDate = filter.FromDate.Date;
                // Lấy đến hết ngày cuối — không bỏ sót hóa đơn cuối ngày
                var toDate = filter.ToDate.Date.AddDays(1).AddTicks(-1);

                var result = new ReportResultDto();

                // ============================================================
                // LOẠI 0 — BÁO CÁO DOANH THU & LỢI NHUẬN
                // Biểu đồ: StackedColumn — Giá vốn (đỏ) + Lợi nhuận (xanh)
                // Bảng: nhóm theo ngày
                // ============================================================
                if (filter.ReportType == 0)
                {
                    var query = _context.CT_HoaDon
                        .Include(x => x.HoaDon)
                        .Where(x => x.HoaDon.NgayTao >= fromDate && x.HoaDon.NgayTao <= toDate)
                        .Join(_context.PhienBanSach, ct => ct.ISBN, pbs => pbs.ISBN, (ct, pbs) => new { ct, pbs })
                        .Join(_context.Sach, x => x.pbs.MaSach, s => s.MaSach, (x, s) => new { x.ct, x.pbs, s })
                        .Join(_context.TheLoai, x => x.s.MaTheLoai, tl => tl.MaTheLoai, (x, tl) => new { x.ct, x.pbs, x.s, tl })
                        .AsQueryable();

                    // Áp dụng bộ lọc phụ tùy chọn
                    if (!string.IsNullOrEmpty(filter.StaffName))
                        query = query.Where(x => x.ct.HoaDon.NguoiTao == filter.StaffName);

                    if (!string.IsNullOrEmpty(filter.CategoryName))
                        query = query.Where(x => x.tl.TenTheLoai == filter.CategoryName);

                    var rawData = await query.ToListAsync();

                    // Gom nhóm theo ngày để ra từng dòng bảng
                    var groupedByDay = rawData
                        .GroupBy(x => x.ct.HoaDon.NgayTao.Date)
                        .OrderBy(g => g.Key)
                        .Select(g =>
                        {
                            var distinctOrders = g.Select(x => x.ct.HoaDon).DistinctBy(x => x.MaHoaDon).ToList();

                            decimal totalAmount = g.Sum(x => x.ct.SoLuong * x.ct.GiaBan);
                            decimal discount = distinctOrders.Sum(x => x.GiamGia);
                            decimal netRevenue = totalAmount - discount;
                            decimal totalCost = g.Sum(x => x.ct.SoLuong * x.ct.GiaNiemYet);
                            decimal grossProfit = netRevenue - totalCost;

                            return new RevenueReportRowDto
                            {
                                Date = g.Key,
                                InvoiceCount = distinctOrders.Count,
                                BooksSold = g.Sum(x => x.ct.SoLuong),
                                TotalAmount = totalAmount,
                                Discount = discount,
                                NetRevenue = netRevenue,
                                TotalCost = totalCost,
                                GrossProfit = grossProfit
                            };
                        })
                        .ToList();

                    result.RevenueRows = groupedByDay
                                        .OrderByDescending(x => x.Date)
                                        .ToList();

                    // --- Dữ liệu biểu đồ Stacked Column ---
                    result.RevenueDateLabels = groupedByDay.Select(x => x.Date.ToString("dd/MM")).ToList();
                    result.RevenueCostSeries = groupedByDay.Select(x => x.NetRevenue - x.GrossProfit).ToList();
                    result.RevenueProfitSeries = groupedByDay.Select(x => x.GrossProfit).ToList();
                }
                //else if (filter.ReportType == 1)
                //{
                //    var booksQuery = _context.PhienBanSach
                //        .Include(x => x.Sach)
                //            .ThenInclude(x => x.TheLoai)
                //        .AsQueryable();

                //    if (!string.IsNullOrEmpty(filter.CategoryName))
                //    {
                //        booksQuery = booksQuery.Where(x => x.Sach.TheLoai.TenTheLoai == filter.CategoryName);
                //    }

                //    Console.WriteLine(await booksQuery.CountAsync());
                //    var booksList = await booksQuery.ToListAsync();

                //    // Tính toán số lượng nhập trong kỳ từ chi tiết phiếu nhập
                //    var importsInPeriod = await _context.CT_PhieuNhapSach
                //        .Join(_context.PhieuNhapSach, ct => ct.MaPhieuNhapSach, pn => pn.MaPhieuNhapSach, (ct, pn) => new { ct, pn })
                //        .Where(x => x.pn.NgayTao >= fromDate && x.pn.NgayTao <= toDate)
                //        .GroupBy(x => x.ct.ISBN)
                //        .Select(g => new { ISBN = g.Key, Qty = g.Sum(x => x.ct.SoLuong) })
                //        .ToDictionaryAsync(x => x.ISBN, x => x.Qty);

                //    // Tính toán số lượng xuất trong kỳ từ chi tiết hóa đơn
                //    var salesInPeriod = await _context.CT_HoaDon
                //        .Join(_context.HoaDon, ct => ct.MaHoaDon, hd => hd.MaHoaDon, (ct, hd) => new { ct, hd })
                //        .Where(x => x.hd.NgayTao >= fromDate && x.hd.NgayTao <= toDate)
                //        .GroupBy(x => x.ct.ISBN)
                //        .Select(g => new { ISBN = g.Key, Qty = g.Sum(x => x.ct.SoLuong) })
                //        .ToDictionaryAsync(x => x.ISBN, x => x.Qty);

                //    var inventoryRows = new List<InventoryReportRowDto>();

                //    foreach (var b in booksList)
                //    {
                        
                //        int imported = importsInPeriod.GetValueOrDefault(b.ISBN, 0);
                //        int sold = salesInPeriod.GetValueOrDefault(b.ISBN, 0);
                //        int closing = b.TonKho;
                //        int opening = closing - imported + sold; // Thuật toán tính lùi tồn đầu kỳ

                //        inventoryRows.Add(new InventoryReportRowDto
                //        {
                //            BookId = b.ISBN,         
                //            BookName = b.Sach.TenSach,   
                //            CategoryName = b.Sach.TheLoai.TenTheLoai,
                //            OpeningQty = opening,
                //            ImportedQty = imported,
                //            SoldQty = sold,
                //            ClosingQty = closing,
                //            StockValue = closing * (b.GiaNiemYet * 0.6m) // Giả định giá vốn ước tính bằng 60% giá niêm yết
                //        });
                //    }

                //    result.InventoryRows = inventoryRows.OrderByDescending(x => x.StockValue).ToList();

                //    var topStock = result.InventoryRows.Take(10).ToList();
                //    result.InventoryBarValues = topStock.Select(x => (double)x.StockValue).ToList();
                //    result.InventoryBarLabels = topStock.Select(x => x.BookName.Length > 12 ? x.BookName.Substring(0, 12) + "..." : x.BookName).ToList();
                //}

                else if (filter.ReportType == 1)
                {
                    int fromMonth = fromDate.Month;
                    int fromYear = fromDate.Year;
                    int toMonth = toDate.Month;
                    int toYear = toDate.Year;
                    var reportQuery = _context.CT_BC_Sach
                        .Join(_context.BC_Sach, 
                            ct => ct.MaBaoCaoSach, 
                            bc => bc.MaBaoCaoSach, 
                            (ct, bc) => new { ct, bc })
                        .Join(_context.PhienBanSach.Include(p => p.Sach).ThenInclude(s => s.TheLoai),
                            combined => combined.ct.ISBN,
                            p => p.ISBN,
                            (combined, p) => new { combined.ct, combined.bc, p })
                        // Bộ lọc điều kiện thời gian dựa trên tháng/năm của báo cáo
                        .Where(x => (x.bc.Nam > fromYear || (x.bc.Nam == fromYear && x.bc.Thang >= fromMonth)) &&
                                    (x.bc.Nam < toYear || (x.bc.Nam == toYear && x.bc.Thang <= toMonth)))
                        .AsQueryable();

                    // Áp dụng bộ lọc Tên thể loại nếu có chọn
                    if (!string.IsNullOrEmpty(filter.CategoryName))
                    {
                        reportQuery = reportQuery.Where(x => x.p.Sach.TheLoai.TenTheLoai == filter.CategoryName);
                    }

                    var rawReportList = await reportQuery.ToListAsync();

                    // 3. GROUP BY theo từng cuốn sách (ISBN) để cộng dồn nếu khoảng thời gian xem gồm nhiều tháng
                    var inventoryRows = rawReportList
                        .GroupBy(x => new { x.ct.ISBN, x.p.Sach.TenSach, CategoryName = x.p.Sach.TheLoai.TenTheLoai })
                        .Select(g => {
                            // Sắp xếp các tháng tăng dần để lấy Tồn đầu của tháng nhỏ nhất và Tồn cuối của tháng lớn nhất
                            var sortedGroup = g.OrderBy(x => x.bc.Nam).ThenBy(x => x.bc.Thang).ToList();
                            
                            int opening = sortedGroup.First().ct.TonDau;   // Tồn đầu kỳ = Tồn đầu của tháng đầu tiên chọn
                            int closing = sortedGroup.Last().ct.TonCuoi;   // Tồn cuối kỳ = Tồn cuối của tháng cuối cùng chọn
                            int imported = g.Sum(x => x.ct.TongNhap);      // Tổng nhập = Cộng dồn tổng nhập các tháng
                            int sold = g.Sum(x => x.ct.TongXuat);          // Tổng xuất = Cộng dồn tổng xuất các tháng

                            // Lấy thông tin sách hiện tại phục vụ tính giá trị tồn kho
                            var currentBook = g.First().p;

                            return new InventoryReportRowDto
                            {
                                BookId = g.Key.ISBN,
                                BookName = g.Key.TenSach,
                                CategoryName = g.Key.CategoryName,
                                OpeningQty = opening,
                                ImportedQty = imported,
                                SoldQty = sold,
                                ClosingQty = closing,
                                // Tính giá trị tồn kho chuẩn xác theo giá vốn ước tính
                                StockValue = closing * (currentBook.GiaNiemYet * 0.6m) 
                            };
                        })
                        .OrderByDescending(x => x.StockValue)
                        .ToList();

                    result.InventoryRows = inventoryRows;

                    // 4. Đổ dữ liệu top 10 dòng ra biểu đồ thanh nằm ngang (RowSeries) của WPF
                    var topStock = result.InventoryRows.Take(10).ToList();
                    result.InventoryBarValues = topStock.Select(x => (double)x.StockValue).ToList();
                    result.InventoryBarLabels = topStock.Select(x => x.BookName.Length > 12 ? x.BookName.Substring(0, 12) + "..." : x.BookName).ToList();
                }
                else if (filter.ReportType == 2)
                {
                    var customerQuery = _context.KhachHang.Include(x => x.LoaiKhachHang).AsQueryable();

                    if (!string.IsNullOrEmpty(filter.CustomerType))
                    {
                        customerQuery = customerQuery.Where(x => x.LoaiKhachHang!.TenLoaiKhachHang == filter.CustomerType);
                    }

                    var customers = await customerQuery.ToListAsync();

                    var validCustomerIds = customers.Select(x => x.MaKhachHang).ToList();

                    var invoices = await _context.HoaDon
                        .Where(x => x.NgayTao >= fromDate
                                 && x.NgayTao <= toDate
                                 && x.MaKhachHang != 0
                                 && validCustomerIds.Contains(x.MaKhachHang ?? 0)) 
                        .ToListAsync();

                    var receipts = await _context.PhieuThuTien
                        .Where(x => x.NgayTao >= fromDate
                                 && x.NgayTao <= toDate
                                 && x.MaKhachHang != 0
                                 && validCustomerIds.Contains(x.MaKhachHang)) 
                        .ToListAsync();

                    var debtRows = new List<DebtReportRowDto>();

                    foreach (var c in customers)
                    {
                        if (c.MaKhachHang == 1) continue;
                        decimal newDebt = invoices.Where(x => x.MaKhachHang == c.MaKhachHang).Sum(x => x.TongTien - x.SoTienTra);

                        decimal paidDebt = receipts.Where(x => x.MaKhachHang == c.MaKhachHang).Sum(x => x.SoTienThu);

                        decimal closingDebt = c.TienNo; 
                        decimal openingDebt = closingDebt - newDebt + paidDebt; 

                        debtRows.Add(new DebtReportRowDto
                        {
                            CustomerName = c.TenKhachHang,
                            CustomerType = c.LoaiKhachHang.TenLoaiKhachHang,
                            OpeningDebt = openingDebt,
                            NewDebt = newDebt,
                            PaidDebt = paidDebt,
                            ClosingDebt = closingDebt,
                        });
                    }

                    result.DebtRows = debtRows.OrderByDescending(x => x.ClosingDebt).ToList();

                    int totalDays = (filter.ToDate.Date - filter.FromDate.Date).Days + 1;
                    var debtAxisLabels = new List<string>();
                    var debtNewSeries = new List<decimal>();
                    var debtPaidSeries = new List<decimal>();

                    var invoicesByDay = invoices.GroupBy(x => x.NgayTao.Date).ToDictionary(g => g.Key, g => g.Sum(x => x.TongTien - x.SoTienTra));
                    var receiptsByDay = receipts.GroupBy(x => x.NgayTao.Date).ToDictionary(g => g.Key, g => g.Sum(x => x.SoTienThu));

                    for (int i = 0; i < Math.Min(totalDays, 30); i++) 
                    {
                        var day = filter.FromDate.Date.AddDays(i);
                        debtAxisLabels.Add(day.ToString("dd/MM"));
                        debtNewSeries.Add(invoicesByDay.GetValueOrDefault(day, 0));
                        debtPaidSeries.Add(receiptsByDay.GetValueOrDefault(day, 0));
                    }

                    result.DebtAxisLabels = debtAxisLabels;
                    result.DebtNewSeries = debtNewSeries;
                    result.DebtPaidSeries = debtPaidSeries;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    type = ex.GetType().Name
                });
            }
        }
    }
}