using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ImportReceiptsController : BaseController<PhieuNhapSach>
    {
        public ImportReceiptsController(IGenericRepository<PhieuNhapSach> repo) : base(repo)
        {
        }
    }
}
