using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class InvoiceDetailsController : BaseController<InvoiceDetail>
    {
        public InvoiceDetailsController(IGenericRepository<InvoiceDetail> repo) : base(repo)
        {
        }
    }
}
