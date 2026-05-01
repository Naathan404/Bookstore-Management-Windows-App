using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class MonthlyStockReportsController : BaseController<MonthlyBookReport>
    {
        public MonthlyStockReportsController(IGenericRepository<MonthlyBookReport> repo) : base(repo)
        {
        }
    }
}
