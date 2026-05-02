using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ReceiptsController : BaseController<PhieuThuTien, int>
    {
        public ReceiptsController(IGenericRepository<PhieuThuTien, int> repo) : base(repo)
        {
        }
    }
}
