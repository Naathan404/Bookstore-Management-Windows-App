using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class CustomersController : BaseController<Customer>
    {
        public CustomersController(IGenericRepository<Customer> repo) : base(repo)
        {
        }
    }
}
