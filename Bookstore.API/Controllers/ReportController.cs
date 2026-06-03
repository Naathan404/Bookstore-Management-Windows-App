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

                            decimal totalAmount = g.Sum(x => x.ct.SoLuong * x.ct.DonGia);
                            decimal discount = distinctOrders.Sum(x => x.GiamGia);
                            decimal netRevenue = totalAmount - discount;
                            decimal totalCost = g.Sum(x => x.ct.SoLuong * x.ct.GiaVon);
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

                // ============================================================
                // LOẠI 1 — BÁO CÁO TỒN KHO
                // Biểu đồ: Horizontal Bar — Top 10 giá trị tồn kho cao nhất
                // Bảng: từng phiên bản sách, tính đầu kỳ → cuối kỳ
                // ============================================================
                //else if (filter.ReportType == 1)
                //{
                //    var booksQuery = _context.PhienBanSach
                //        .Include(p => p.Sach)
                //            .ThenInclude(s => s.TheLoai)
                //        .AsQueryable();

                //    if (!string.IsNullOrEmpty(filter.CategoryName))
                //        booksQuery = booksQuery.Where(p =>
                //            p.Sach != null &&
                //            p.Sach.TheLoai != null &&
                //            p.Sach.TheLoai.TenTheLoai == filter.CategoryName); 

                //    var books = await booksQuery.ToListAsync();
                //    var allIsbns = books.Select(b => b.ISBN).ToList();

                //    // Tổng số lượng NHẬP trong kỳ theo ISBN
                //    var importsInPeriod = await _context.CT_PhieuNhapSach
                //        .Where(ct => allIsbns.Contains(ct.ISBN)
                //                  && ct.PhieuNhapSach.NgayTao >= fromDate
                //                  && ct.PhieuNhapSach.NgayTao <= toDate)
                //        .GroupBy(ct => ct.ISBN)
                //        .Select(g => new { ISBN = g.Key, Qty = g.Sum(x => x.SoLuong) })
                //        .ToDictionaryAsync(x => x.ISBN, x => x.Qty);

                //    // Tổng số lượng XUẤT (bán) trong kỳ theo ISBN
                //    var salesInPeriod = await _context.CT_HoaDon
                //        .Where(ct => allIsbns.Contains(ct.ISBN)
                //                  && ct.HoaDon.NgayTao >= fromDate
                //                  && ct.HoaDon.NgayTao <= toDate)
                //        .GroupBy(ct => ct.ISBN)
                //        .Select(g => new { ISBN = g.Key, Qty = g.Sum(x => x.SoLuong) })
                //        .ToDictionaryAsync(x => x.ISBN, x => x.Qty);

                //    // Lấy giá vốn trung bình mỗi ISBN từ lần nhập gần nhất
                //    var latestCostByIsbn = await _context.CT_PhieuNhapSach
                //        .Where(ct => allIsbns.Contains(ct.ISBN))
                //        .GroupBy(ct => ct.ISBN)
                //        .Select(g => new
                //        {
                //            ISBN = g.Key,
                //            LatestCost = g.OrderByDescending(x => x.PhieuNhapSach.NgayTao)
                //                          .Select(x => x.DonGiaNhap)
                //                          .FirstOrDefault()
                //        })
                //        .ToDictionaryAsync(x => x.ISBN, x => x.LatestCost);

                //    var inventoryRows = books.Select(b =>
                //    {
                //        int imported = importsInPeriod.GetValueOrDefault(b.ISBN, 0);
                //        int sold = salesInPeriod.GetValueOrDefault(b.ISBN, 0);
                //        int closing = b.TonKho;
                //        // Công thức ngược: Đầu kỳ = Cuối kỳ - Nhập + Bán
                //        int opening = closing - imported + sold;

                //        // Ưu tiên giá vốn nhập gần nhất; fallback về giá niêm yết
                //        decimal costPrice = latestCostByIsbn.GetValueOrDefault(b.ISBN, b.GiaNiemYet);
                //        decimal stockValue = closing * costPrice;

                //        return new InventoryReportRowDto
                //        {
                //            BookId = b.ISBN,
                //            BookName = b.Sach?.TenSach ?? "Không rõ",
                //            CategoryName = b.Sach?.TheLoai?.TenTheLoai ?? "Chưa phân loại",
                //            OpeningQty = Math.Max(opening, 0), // Không để âm gây hiểu lầm
                //            ImportedQty = imported,
                //            SoldQty = sold,
                //            ClosingQty = closing,
                //            StockValue = stockValue
                //        };
                //    })
                //    .OrderByDescending(x => x.StockValue)
                //    .ToList();

                //    result.InventoryRows = inventoryRows;

                //    // --- Dữ liệu biểu đồ Horizontal Bar (Top 10) ---
                //    // ViewModel expects:
                //    //   InventoryBarLabels = nhãn trục Y (tên sách)
                //    //   InventoryBarValues = giá trị tồn kho (double)
                //    var top10 = inventoryRows.Take(10).ToList();
                //    result.InventoryBarLabels = top10
                //        .Select(x => x.BookName.Length > 20
                //            ? x.BookName[..20] + "…"
                //            : x.BookName)
                //        .ToList();
                //    result.InventoryBarValues = top10
                //        .Select(x => (double)x.StockValue)
                //        .ToList();
                //}

                // ================================================================
                // LOẠI 1: BÁO CÁO TỒN KHO (TÍNH TOÁN ĐẦU KỲ - CUỐI KỲ VIA JOIN)
                // ================================================================
                else if (filter.ReportType == 1)
                {
                    var booksQuery = _context.PhienBanSach
                        .Include(x => x.Sach)
                            .ThenInclude(x => x.TheLoai)
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(filter.CategoryName))
                    {
                        booksQuery = booksQuery.Where(x => x.Sach.TheLoai.TenTheLoai == filter.CategoryName);
                    }

                    Console.WriteLine(await booksQuery.CountAsync());
                    var booksList = await booksQuery.ToListAsync();

                    // Tính toán số lượng nhập trong kỳ từ chi tiết phiếu nhập
                    var importsInPeriod = await _context.CT_PhieuNhapSach
                        .Join(_context.PhieuNhapSach, ct => ct.MaPhieuNhapSach, pn => pn.MaPhieuNhapSach, (ct, pn) => new { ct, pn })
                        .Where(x => x.pn.NgayTao >= fromDate && x.pn.NgayTao <= toDate)
                        .GroupBy(x => x.ct.ISBN)
                        .Select(g => new { ISBN = g.Key, Qty = g.Sum(x => x.ct.SoLuong) })
                        .ToDictionaryAsync(x => x.ISBN, x => x.Qty);

                    // Tính toán số lượng xuất trong kỳ từ chi tiết hóa đơn
                    var salesInPeriod = await _context.CT_HoaDon
                        .Join(_context.HoaDon, ct => ct.MaHoaDon, hd => hd.MaHoaDon, (ct, hd) => new { ct, hd })
                        .Where(x => x.hd.NgayTao >= fromDate && x.hd.NgayTao <= toDate)
                        .GroupBy(x => x.ct.ISBN)
                        .Select(g => new { ISBN = g.Key, Qty = g.Sum(x => x.ct.SoLuong) })
                        .ToDictionaryAsync(x => x.ISBN, x => x.Qty);

                    var inventoryRows = new List<InventoryReportRowDto>();

                    foreach (var b in booksList)
                    {
                        int imported = importsInPeriod.GetValueOrDefault(b.ISBN, 0);
                        int sold = salesInPeriod.GetValueOrDefault(b.ISBN, 0);
                        int closing = b.TonKho;
                        int opening = closing - imported + sold; // Thuật toán tính lùi tồn đầu kỳ

                        inventoryRows.Add(new InventoryReportRowDto
                        {
                            BookId = b.ISBN,         // FIX: Đổi từ BookCode sang BookId cho khớp 100% với XAML Binding
                            BookName = b.Sach.TenSach,   // FIX: Đổi từ Title sang BookName cho khớp 100% với XAML Binding
                            CategoryName = b.Sach.TheLoai.TenTheLoai,
                            OpeningQty = opening,
                            ImportedQty = imported,
                            SoldQty = sold,
                            ClosingQty = closing,
                            StockValue = closing * (b.GiaNiemYet * 0.6m) // Giả định giá vốn ước tính bằng 60% giá niêm yết
                        });
                    }

                    result.InventoryRows = inventoryRows.OrderByDescending(x => x.StockValue).ToList();

                    // Đóng gói dữ liệu biểu đồ thanh ngang (Lấy Top 10 đầu sách tồn lớn nhất)
                    var topStock = result.InventoryRows.Take(10).ToList();
                    result.InventoryBarValues = topStock.Select(x => (double)x.StockValue).ToList();
                    result.InventoryBarLabels = topStock.Select(x => x.BookName.Length > 12 ? x.BookName.Substring(0, 12) + "..." : x.BookName).ToList();
                }

                // ============================================================
                // LOẠI 2 — BÁO CÁO CÔNG NỢ KHÁCH HÀNG
                // Biểu đồ: Line Chart theo ngày — Nợ phát sinh vs Nợ thu hồi
                // Bảng: tổng hợp theo từng khách hàng
                // ============================================================
                //else if (filter.ReportType == 2)
                //{
                //    // ---- BẢNG: Tổng hợp theo khách hàng ----
                //    var customerQuery = _context.KhachHang.AsQueryable();

                //    if (!string.IsNullOrEmpty(filter.CustomerName))
                //        customerQuery = customerQuery.Where(x => x.TenKhachHang.Contains(filter.CustomerName));

                //    var customers = await customerQuery.ToListAsync();
                //    var customerIds = customers.Select(c => c.MaKhachHang).ToList();

                //    // Hóa đơn mua nợ trong kỳ (còn nợ = TongTien - SoTienTra > 0)
                //    // Nếu HoaDon không có SoTienTra thì xem toàn bộ TongTien là nợ phát sinh
                //    var invoicesInPeriod = await _context.HoaDon
                //        .Where(x => customerIds.Contains(x.MaKhachHang!)
                //                 && x.NgayTao >= fromDate
                //                 && x.NgayTao <= toDate)
                //        .ToListAsync();

                //    // Phiếu thu tiền trong kỳ
                //    var receiptsInPeriod = await _context.PhieuThuTien
                //        .Where(x => x.MaKhachHang != null
                //                 && x.MaKhachHang != null && customerIds.Contains(x.MaKhachHang)
                //                 && x.NgayTao >= fromDate
                //                 && x.NgayTao <= toDate)
                //        .ToListAsync();

                //    var debtRows = customers.Select(c =>
                //    {
                //        // Phát sinh nợ mới = phần chưa trả của các hóa đơn trong kỳ
                //        // SoTienTra: nếu bảng của bạn có cột này thì dùng; không thì dùng 0
                //        decimal newDebt = invoicesInPeriod
                //            .Where(x => x.MaKhachHang == c.MaKhachHang)
                //            .Sum(x => x.TongTien - (x.SoTienTra > 0 ? x.SoTienTra : 0));

                //        decimal paidDebt = receiptsInPeriod
                //            .Where(x => x.MaKhachHang == c.MaKhachHang)
                //            .Sum(x => x.SoTienThu);

                //        // Tính ngược nợ đầu kỳ từ số nợ hiện tại (c.TienNo = nợ cuối kỳ hiện tại)
                //        // Công thức: OpeningDebt = ClosingDebtNow - NewDebt + PaidDebt
                //        decimal openingDebt = c.TienNo - newDebt + paidDebt;
                //        decimal closingDebt = openingDebt + newDebt - paidDebt;

                //        return new DebtReportRowDto
                //        {
                //            CustomerName = c.TenKhachHang,
                //            OpeningDebt = Math.Max(openingDebt, 0),
                //            NewDebt = Math.Max(newDebt, 0),
                //            PaidDebt = Math.Max(paidDebt, 0),
                //            ClosingDebt = Math.Max(closingDebt, 0)
                //        };
                //    })
                //    .OrderByDescending(x => x.ClosingDebt)
                //    .ToList();

                //    result.DebtRows = debtRows;

                //    // ---- BIỂU ĐỒ: Theo ngày (line chart biến động) ----
                //    // Gom hóa đơn và phiếu thu theo ngày để vẽ đường biến động
                //    // ViewModel expects:
                //    //   DebtAxisLabels = nhãn trục X (ngày)
                //    //   DebtNewSeries  = nợ phát sinh mỗi ngày (decimal)
                //    //   DebtPaidSeries = nợ thu hồi mỗi ngày (decimal)
                //    var totalDays = (filter.ToDate.Date - filter.FromDate.Date).Days + 1;

                //    // Nhóm hóa đơn theo ngày
                //    var invoicesByDay = invoicesInPeriod
                //        .GroupBy(x => x.NgayTao.Date)
                //        .ToDictionary(
                //            g => g.Key,
                //            g => g.Sum(x => x.TongTien - (x.SoTienTra > 0 && x.SoTienTra != null ? x.SoTienTra : 0)));

                //    // Nhóm phiếu thu theo ngày
                //    var receiptsByDay = receiptsInPeriod
                //        .GroupBy(x => x.NgayTao.Date)
                //        .ToDictionary(
                //            g => g.Key,
                //            g => g.Sum(x => x.SoTienThu));

                //    var debtAxisLabels = new List<string>();
                //    var debtNewSeries = new List<decimal>();
                //    var debtPaidSeries = new List<decimal>();

                //    for (int i = 0; i < totalDays; i++)
                //    {
                //        var day = filter.FromDate.Date.AddDays(i);
                //        debtAxisLabels.Add(day.ToString("dd/MM"));
                //        debtNewSeries.Add(invoicesByDay.GetValueOrDefault(day, 0));
                //        debtPaidSeries.Add(receiptsByDay.GetValueOrDefault(day, 0));
                //    }

                //    result.DebtAxisLabels = debtAxisLabels;
                //    result.DebtNewSeries = debtNewSeries;
                //    result.DebtPaidSeries = debtPaidSeries;
                //}
                // ================================================================
                // LOẠI 2: BÁO CÁO CÔNG NỢ KHÁCH HÀNG (SỬ DỤNG COALESCE TRÁNH NULL)
                // ================================================================
                else if (filter.ReportType == 2)
                {
                    var customerQuery = _context.KhachHang.Include(x => x.LoaiKhachHang).AsQueryable();

                    if (!string.IsNullOrEmpty(filter.CustomerType))
                    {
                        customerQuery = customerQuery.Where(x => x.LoaiKhachHang.TenLoaiKhachHang == filter.CustomerType);
                    }

                    //var customers = await customerQuery.ToListAsync();

                    //// Quét hóa đơn và phiếu thu tiền phát sinh trong khoảng thời gian lọc
                    //var invoices = await _context.HoaDon
                    //    .Where(x => x.NgayTao >= fromDate && x.NgayTao <= toDate && x.MaKhachHang != 0)
                    //    .ToListAsync();

                    //var receipts = await _context.PhieuThuTien
                    //    .Where(x => x.NgayTao >= fromDate && x.NgayTao <= toDate && x.MaKhachHang != 0)
                    //    .ToListAsync();

                    //var debtRows = new List<DebtReportRowDto>();

                    //foreach (var c in customers)
                    //{
                    //    // Số nợ phát sinh mới do mua sách chưa trả hết tiền
                    //    decimal newDebt = invoices.Where(x => x.MaKhachHang == c.MaKhachHang).Sum(x => x.TongTien - x.SoTienTra);

                    //    // Số nợ giảm đi thu hồi được từ các phiếu thu tiền mặt
                    //    decimal paidDebt = receipts.Where(x => x.MaKhachHang == c.MaKhachHang).Sum(x => x.SoTienThu);

                    //    decimal closingDebt = c.TienNo; // Số nợ hiện tại cuối kỳ
                    //    decimal openingDebt = closingDebt - newDebt + paidDebt; // Tính ngược lại nợ đầu kỳ

                    //    debtRows.Add(new DebtReportRowDto
                    //    {
                    //        CustomerName = c.TenKhachHang,
                    //        CustomerType = c.LoaiKhachHang.TenLoaiKhachHang,
                    //        OpeningDebt = openingDebt,
                    //        NewDebt = newDebt,
                    //        PaidDebt = paidDebt,
                    //        ClosingDebt = closingDebt,
                    //    });
                    //}

                    //result.DebtRows = debtRows.OrderByDescending(x => x.ClosingDebt).ToList();

                    //// Gom dữ liệu biểu đồ đường biến động theo dòng thời gian ngày
                    //int totalDays = (filter.ToDate.Date - filter.FromDate.Date).Days + 1;
                    //var debtAxisLabels = new List<string>();
                    //var debtNewSeries = new List<decimal>();
                    //var debtPaidSeries = new List<decimal>();

                    //var invoicesByDay = invoices.GroupBy(x => x.NgayTao.Date).ToDictionary(g => g.Key, g => g.Sum(x => x.TongTien - x.SoTienTra));
                    //var receiptsByDay = receipts.GroupBy(x => x.NgayTao.Date).ToDictionary(g => g.Key, g => g.Sum(x => x.SoTienThu));

                    var customers = await customerQuery.ToListAsync();

                    var validCustomerIds = customers.Select(x => x.MaKhachHang).ToList();

                    var invoices = await _context.HoaDon
                        .Where(x => x.NgayTao >= fromDate
                                 && x.NgayTao <= toDate
                                 && x.MaKhachHang != 0
                                 && validCustomerIds.Contains(x.MaKhachHang)) 
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