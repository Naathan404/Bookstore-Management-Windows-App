using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ReceiptsController : BaseController<PhieuThuTien>
    {
        public ReceiptsController(IGenericRepository<PhieuThuTien> repo) : base(repo)
        {
        }
    }
}
