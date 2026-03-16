using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class MonthlyStockReportsController : BaseController<MonthlyStockReport>
    {
        public MonthlyStockReportsController(IGenericRepository<MonthlyStockReport> repo) : base(repo)
        {
        }
    }
}
