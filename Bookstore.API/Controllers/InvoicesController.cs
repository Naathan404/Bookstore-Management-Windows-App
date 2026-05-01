using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class InvoicesController : BaseController<HoaDon>
    {
        public InvoicesController(Interfaces.IGenericRepository<HoaDon> repo) : base(repo)
        {
        }
    }
}
