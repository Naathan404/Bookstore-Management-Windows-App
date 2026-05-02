using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class SuppliersConntroller : BaseController<NhaCungCap, int>
    {
        public SuppliersConntroller(IGenericRepository<NhaCungCap, int> repo) : base(repo)
        {
        }
    }
}
