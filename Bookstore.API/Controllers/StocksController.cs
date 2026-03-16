using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class StocksController : BaseController<Stock>
    {
        public StocksController(IGenericRepository<Stock> repo) : base(repo)
        {
        }
    }
}
