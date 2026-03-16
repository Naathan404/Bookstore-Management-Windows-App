using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class SuppliersConntroller : BaseController<Supplier>
    {
        public SuppliersConntroller(IGenericRepository<Supplier> repo) : base(repo)
        {
        }
    }
}
