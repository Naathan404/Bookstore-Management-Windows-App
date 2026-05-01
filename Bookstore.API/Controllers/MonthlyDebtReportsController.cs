using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class MonthlyDebtReportsController : BaseController<BC_KhachHang>
    {
        public MonthlyDebtReportsController(IGenericRepository<BC_KhachHang> repo) : base(repo)
        {
        }
    }
}
