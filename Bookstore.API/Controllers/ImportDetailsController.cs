using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ImportDetailsController : BaseController<CT_PhieuNhapSach>
    {
        public ImportDetailsController(IGenericRepository<CT_PhieuNhapSach> repo) : base(repo)
        {
        }
    }
}
