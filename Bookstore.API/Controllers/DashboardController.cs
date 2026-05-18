using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Security.Cryptography.Xml;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverviewData()
        {
            try
            {
                var today = DateTime.Today;
                var sevenDaysAgo = today.AddDays(-6);
                var startOfMonth = new DateTime(today.Year, today.Month, 1);

                /// tính doanh thu
                var sale = await _context.HoaDon.Where(x => x.NgayTao.Date == today).SumAsync(x => (decimal?)x.TongTien) ?? 0;
                ///tính lợi nhuận
                var expense = await _context.CT_HoaDon
                    .Where(x => x.HoaDon.NgayTao.Date == today)
                    .SumAsync(x => (decimal?)(x.SoLuong * x.GiaVon)) ?? 0;
                var profit = sale - expense;

                /// tính số lượt khách mới
                var newCustomerNumber = await _context.KhachHang.Where(x => x.NgayTao.Date == today).CountAsync();
                /// tính số hóa đơn
                var receiptNumber = await _context.HoaDon.Where(x => x.NgayTao.Date == today).CountAsync();

                // dữ liệu cho biểu đồ doanh thu 7 ngày trước
                var rawRevenue = await _context.HoaDon
                    .Where(x => x.NgayTao.Date >= sevenDaysAgo && x.NgayTao.Date <= today)
                    .GroupBy(x => x.NgayTao.Date)
                    .Select(g => new { Date = g.Key, Total = (double?)g.Sum(x => x.TongTien) ?? 0 })
                    .ToListAsync();
                var revenueSeries = new List<RevenueDataDto>();
                for(int i = 0; i < 7; i++)
                {
                    var dateIdx = sevenDaysAgo.AddDays(i);
                    var dateData = rawRevenue.FirstOrDefault(x => x.Date == dateIdx);
                    revenueSeries.Add(new RevenueDataDto
                    {
                        Date = dateIdx.ToString("dd/MM"),
                        Value = dateData != null ? dateData.Total : 0
                    });
                }

                /// dữ liệu cho biểu đồ thống kê doanh mục
                var rawCategoryShares = await _context.CT_HoaDon
                    .Where(x => x.HoaDon.NgayTao.Date >= startOfMonth)
                    .Join(_context.PhienBanSach, ct => ct.ISBN, pbs => pbs.ISBN, (ct, pbs) => new { ct, pbs })
                    .Join(_context.Sach, x => x.pbs.MaSach, s => s.MaSach, (x, s) => new { x.ct, s })
                    .Join(_context.TheLoai, x => x.s.MaTheLoai, tl => tl.MaTheLoai, (x, tl) => new { x.ct, tl })
                    .GroupBy(x => x.tl.TenTheLoai)
                    .Select(g => new {
                        CategoryName = g.Key,
                        Revenue = (double?)g.Sum(x => x.ct.SoLuong * x.ct.DonGia) ?? 0
                    }).ToListAsync();


                var totalMonthRevenue = rawCategoryShares.Sum(x => x.Revenue);
                var categoryShares = rawCategoryShares.Select(x => new CategoryShareDto
                {
                    CategoryName = x.CategoryName ?? "Khác",
                    Percentage = totalMonthRevenue > 0 ? Math.Round((x.Revenue / totalMonthRevenue) * 100, 2) : 0
                }).ToList();

                //// dữ liệu biểu đồ so sánh doanh thu, chi phí và suy ra lợi nhuận
                // Lấy Doanh thu theo ngày
                var dailyRevenue = await _context.HoaDon
                    .Where(x => x.NgayTao.Date >= sevenDaysAgo && x.NgayTao.Date <= today)
                    .GroupBy(x => x.NgayTao.Date)
                    .Select(g => new { Date = g.Key, Total = (double?)g.Sum(x => x.TongTien) ?? 0 })
                    .ToListAsync();

                // Lấy Chi phí nhập kho theo ngày
                var dailyImports = await _context.CT_PhieuNhapSach
                            .Where(x => x.PhieuNhapSach.NgayTao.Date >= sevenDaysAgo && x.PhieuNhapSach.NgayTao.Date <= today)
                            .GroupBy(x => x.PhieuNhapSach.NgayTao.Date)
                            .Select(g => new {
                                Date = g.Key,
                                Total = (double?)g.Sum(ct => ct.SoLuong * ct.DonGiaNhap) ?? 0
                            })
                            .ToListAsync();

                // Lấy Giá vốn hàng bán theo ngày để tính Lợi nhuận thực tế
                var dailyCogs = await _context.CT_HoaDon
                            .Where(x => x.HoaDon.NgayTao.Date >= sevenDaysAgo && x.HoaDon.NgayTao.Date <= today)
                            .GroupBy(x => x.HoaDon.NgayTao.Date)
                            .Select(g => new { Date = g.Key, Total = (double?)g.Sum(x => x.SoLuong * x.GiaVon) ?? 0 })
                            .ToListAsync();

                var comparisonSeries = new List<ComparisonDataDto>();

                for (int i = 0; i < 7; i++)
                {
                    var dateIdx = sevenDaysAgo.AddDays(i);
                    var rev = dailyRevenue.FirstOrDefault(x => x.Date == dateIdx)?.Total ?? 0;
                    var imp = dailyImports.FirstOrDefault(x => x.Date == dateIdx)?.Total ?? 0;
                    var cogs = dailyCogs.FirstOrDefault(x => x.Date == dateIdx)?.Total ?? 0;

                    comparisonSeries.Add(new ComparisonDataDto
                    {
                        Date = dateIdx.ToString("dd/MM"),
                        Revenue = rev,
                        ImportCost = imp,
                        Profit = rev - cogs // Lợi nhuận = Doanh thu - Giá vốn
                    });
                }


                /// top sách
                var topBooksQuery = await _context.CT_HoaDon
                    .Where(x => x.HoaDon.NgayTao.Date >= startOfMonth)
                    .GroupBy(x => x.ISBN)
                    .Select(g => new {
                        ISBN = g.Key,
                        TotalSold = g.Sum(x => x.SoLuong)
                    })
                    .OrderByDescending(x => x.TotalSold)
                    .Take(5).ToListAsync();

                // lấy Image riêng trong bộ nhớ
                var topBooksDto = topBooksQuery.Select((b, index) => new TopBookDto
                {
                    Rank = index + 1,
                    BookImage = _context.PhienBanSach
                        .Include(p => p.Sach)
                        .FirstOrDefault(p => p.ISBN == b.ISBN)?.Sach?.ImageUrl
                        ?? "/Resources/Images/Books/default_book_cover.jpg"
                }).ToList();


                /// top khách hàng
                var topCustomers = await _context.HoaDon
                    .Where(x => x.NgayTao.Date >= startOfMonth)
                    .GroupBy(x => x.KhachHang != null ? x.KhachHang.TenKhachHang : null)
                    .Select(g => new CustomerRankingDto
                    {
                        Name = g.Key ?? "Khách Lẻ",
                        TotalSpent = (decimal?)g.Sum(x => x.TongTien) ?? 0
                    })
                    .OrderByDescending(x => x.TotalSpent).Take(5).ToListAsync();

                // top nhân viên
                var topStaffs = await _context.HoaDon
                    .Where(x => x.NgayTao.Date >= startOfMonth)
                    .GroupBy(x => x.NguoiTao)
                    .Select(g => new StaffRankingDto
                    {
                        Name = g.Key ?? "Admin",
                        SalesAmount = (decimal?)g.Sum(x => x.TongTien) ?? 0
                    })
                    .OrderByDescending(x => x.SalesAmount).Take(5).ToListAsync();



                //// =========== Các trnagj thái vân hành ============ ///
                var recentOrders = await _context.HoaDon
                    //.Include(x => x.KhachHang)
                    .Where(x => x.NgayTao.Date == today)
                    .OrderByDescending(x => x.NgayTao)
                    .Take(5)
                    .Select(x => new OrderDto
                    {
                        ReceiptNum = x.MaHoaDon,
                        CustomerName = x.KhachHang != null ? x.KhachHang.TenKhachHang : "Khách Lẻ",
                        TotalCost = x.TongTien,
                        CreateAt = x.NgayTao
                    }).ToListAsync();


                var recentImports = await _context.PhieuNhapSach
                                    .Include(x => x.CT_PhieuNhapSach)
                                    .Include(x => x.NhaCungCap)
                                    //.Where(x => x.NgayTao.Date == today)
                                    .OrderByDescending(x => x.NgayTao)
                                    .Take(5)
                                    .Select(x => new ImportDto
                                    {
                                        ImportId = x.MaPhieuNhapSach,
                                        SupplierName = x.NhaCungCap!.TenNhaCungCap,
                                        TotalQuantity = x.CT_PhieuNhapSach.Sum(ct => ct.SoLuong),
                                        Total = x.CT_PhieuNhapSach.Sum(ct => ct.DonGiaNhap * ct.SoLuong),
                                        CreatedAt = x.NgayTao
                                    }).ToListAsync();

                var recentPayments = await _context.PhieuThuTien
                    //.Where(x => x.NgayTao.Date == today)
                    .OrderByDescending(x => x.NgayTao)
                    .Take(5)
                    .Select(x => new PaymentDto
                    {
                        PaymentId = x.MaPhieuThuTien,
                        Reason = x.LyDoThu,
                        Amount = x.SoTienThu,
                        CreatedAt = x.NgayTao,
                        CustomerName = x.KhachHang != null ? x.KhachHang.TenKhachHang : "Khách vãng lai"
                    }).ToListAsync();

                // Cảnh báo tồn kho dưới 10 cuốn
                var stockWarnings = await _context.PhienBanSach
                    .Include(x => x.Sach)
                    .Where(x => x.TonKho < 10)
                    .Select(x => new StockWarningDto
                    {
                        Name = x.Sach.TenSach,
                        RemainingQuantity = x.TonKho
                    })
                    .OrderBy(x => x.RemainingQuantity).Take(10).ToListAsync();

                // mock data
                DashboardOverviewDto result = new DashboardOverviewDto
                {
                    Sale = sale,
                    Profit = profit,
                    CustNum = newCustomerNumber,
                    ReceiptNum = receiptNumber,

                    RevenueSeries = revenueSeries,
                    CategoryShares = categoryShares,

                    TopBooks = topBooksDto,
                    TopCustomers = topCustomers,
                    TopStaffs = topStaffs,

                    RecentOrders = recentOrders,
                    RecentImports = recentImports,
                    RecentPayments = recentPayments,
                    StockWarnings = stockWarnings,
                    ComparisonSeries = comparisonSeries
                };

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