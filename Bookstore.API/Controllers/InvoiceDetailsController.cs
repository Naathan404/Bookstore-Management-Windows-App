using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class InvoiceDetailsController : BaseController<CT_HoaDon>
    {
        public InvoiceDetailsController(IGenericRepository<CT_HoaDon> repo) : base(repo)
        {
        }
    }
}
