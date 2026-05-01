using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ImportReceiptsController : BaseController<PhieuNhapSach, int>
    {
        public ImportReceiptsController(IGenericRepository<PhieuNhapSach, int> repo) : base(repo)
        {
        }
    }
}
