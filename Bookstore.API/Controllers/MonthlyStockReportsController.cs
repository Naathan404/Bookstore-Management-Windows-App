using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class MonthlyStockReportsController : BaseController<BC_Sach>
    {
        public MonthlyStockReportsController(IGenericRepository<BC_Sach> repo) : base(repo)
        {
        }
    }
}
