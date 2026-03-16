using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class MonthlyDebtReportsController : BaseController<MonthlyDebtReport>
    {
        public MonthlyDebtReportsController(IGenericRepository<MonthlyDebtReport> repo) : base(repo)
        {
        }
    }
}
