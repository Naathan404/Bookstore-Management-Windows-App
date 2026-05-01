using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class MonthlyStockReportsController : BaseController<BC_Sach, int>
    {
        public MonthlyStockReportsController(IGenericRepository<BC_Sach, int> repo) : base(repo)
        {
        }
    }
}
