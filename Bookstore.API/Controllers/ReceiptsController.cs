using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ReceiptsController : BaseController<Receipt>
    {
        public ReceiptsController(IGenericRepository<Receipt> repo) : base(repo)
        {
        }
    }
}
