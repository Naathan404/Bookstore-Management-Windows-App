using Bookstore.API.Data;
using Bookstore.Share.DTOs;
using Microsoft.AspNetCore.Mvc;

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
                // var today = DateTime.Today;
                // var doanhThu = await _context.HoaDons.Where(x => x.NgayLap.Date == today).SumAsync(x => x.TongTien);
                // var khachMoi = await _context.KhachHangs.CountAsync(x => x.NgayTao.Date == today);
                // var soDonHang = await _context.HoaDons.CountAsync(x => x.NgayLap.Date == today);

                // mock data
                DashboardOverviewDto result = new DashboardOverviewDto
                {
                    Sale = 25450000,
                    Profit = 12500000,
                    CustNum = 45,
                    ReceiptNum = 128,

                    // Biểu đồ Đường (Doanh thu 7 ngày)
                    RevenueSeries = new List<RevenueDataDto>
                    {
                        new RevenueDataDto { Date = "T2", Value = 15 },
                        new RevenueDataDto { Date = "T3", Value = 20 },
                        new RevenueDataDto { Date = "T4", Value = 18 },
                        new RevenueDataDto { Date = "T5", Value = 25 },
                        new RevenueDataDto { Date = "T6", Value = 22 },
                        new RevenueDataDto { Date = "T7", Value = 30 },
                        new RevenueDataDto { Date = "CN", Value = 28 }
                    },

                    // Biểu đồ Tròn (Tỷ trọng thể loại)
                    CategoryShares = new List<CategoryShareDto>
                    {
                        new CategoryShareDto { CategoryName = "Công nghệ thông tin", Percentage = 45 },
                        new CategoryShareDto { CategoryName = "Kinh tế - Quản trị", Percentage = 25 },
                        new CategoryShareDto { CategoryName = "Văn học", Percentage = 20 },
                        new CategoryShareDto { CategoryName = "Tâm lý - Kỹ năng", Percentage = 10 }
                    },

                    TopBooks = new List<TopBookDto>
                    {
                        new TopBookDto { Rank = 1, BookImage = "/Resources/Images/Books/matbiec.jpg" },
                        new TopBookDto { Rank = 2, BookImage = "/Resources/Images/Books/default_book_cover.jpg" },
                        new TopBookDto { Rank = 3, BookImage = "/Resources/Images/Books/default_book_cover.jpg" },
                        new TopBookDto { Rank = 4, BookImage = "/Resources/Images/Books/default_book_cover.jpg" },
                        new TopBookDto { Rank = 5, BookImage = "/Resources/Images/Books/default_book_cover.jpg" }
                    },

                    TopCustomers = new List<CustomerRankingDto>
                    {
                        new CustomerRankingDto { Name = "Nguyễn Văn A", TotalSpent = 15500000 },
                        new CustomerRankingDto { Name = "Trần Thị B", TotalSpent = 12200000 }
                    },

                    TopStaffs = new List<StaffRankingDto>
                    {
                        new StaffRankingDto { Name = "Phạm Nhân Viên 1", SalesAmount = 45000000 },
                        new StaffRankingDto { Name = "Hoàng Nhân Viên 2", SalesAmount = 38500000 }
                    },

                    RecentOrders = new List<OrderDto>
                    {
                        new OrderDto { ReceiptNum = "HD001", CustomerName = "Khách Lẻ", TotalCost = 150000 },
                        new OrderDto { ReceiptNum = "HD002", CustomerName = "Nguyễn Văn A", TotalCost = 1250000 }
                    },

                    RecentImports = new List<ImportDto>
                    {
                        new ImportDto { ImportId = "NK001", SupplierName = "NXB Trẻ", TotalQuantity = 500 }
                    },

                    RecentPayments = new List<PaymentDto>
                    {
                        new PaymentDto { PaymentId = "PT001", Reason = "Thu tiền nợ KH", Amount = 5000000 }
                    },

                    StockWarnings = new List<StockWarningDto>
                    {
                        new StockWarningDto { Name = "C# căn bản tới nâng cao", RemainingQuantity = 5 },
                        new StockWarningDto { Name = "Đắc Nhân Tâm", RemainingQuantity = 2 }
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}