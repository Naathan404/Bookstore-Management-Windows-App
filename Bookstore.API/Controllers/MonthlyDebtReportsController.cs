using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class MonthlyDebtReportsController : BaseController<MonthlyCustomerReport>
    {
        public MonthlyDebtReportsController(IGenericRepository<MonthlyCustomerReport> repo) : base(repo)
        {
        }
    }
}
