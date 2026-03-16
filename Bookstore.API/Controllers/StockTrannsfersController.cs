using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class StockTrannsfersController : BaseController<StockTransfer>
    {
        public StockTrannsfersController(IGenericRepository<StockTransfer> repo) : base(repo)
        {
        }
    }
}
