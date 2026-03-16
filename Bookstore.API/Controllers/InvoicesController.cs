using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class InvoicesController : BaseController<Invoice>
    {
        public InvoicesController(Interfaces.IGenericRepository<Invoice> repo) : base(repo)
        {
        }
    }
}
