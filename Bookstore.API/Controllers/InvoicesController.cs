using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class InvoicesController : BaseController<HoaDon, int>
    {
        public InvoicesController(Interfaces.IGenericRepository<HoaDon, int> repo) : base(repo)
        {
        }
    }
}
