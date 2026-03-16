using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class StockTransferDetailsController : BaseController<StockTransferDetail>
    {
        public StockTransferDetailsController(IGenericRepository<StockTransferDetail> repo) : base(repo)
        {
        }
    }
}
