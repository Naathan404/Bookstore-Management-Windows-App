using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ImportDetailsController : BaseController<ImportDetail>
    {
        public ImportDetailsController(IGenericRepository<ImportDetail> repo) : base(repo)
        {
        }
    }
}
