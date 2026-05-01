using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class CustomersController : BaseController<KhachHang, int>
    {
        public CustomersController(IGenericRepository<KhachHang, int> repo) : base(repo)
        {
        }
    }
}
