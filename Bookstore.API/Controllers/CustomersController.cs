using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class CustomersController : BaseController<KhachHang>
    {
        public CustomersController(IGenericRepository<KhachHang> repo) : base(repo)
        {
        }
    }
}
