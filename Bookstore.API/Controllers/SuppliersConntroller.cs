using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class SuppliersConntroller : BaseController<NhaCungCap>
    {
        public SuppliersConntroller(IGenericRepository<NhaCungCap> repo) : base(repo)
        {
        }
    }
}
