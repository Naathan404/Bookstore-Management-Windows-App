using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class ImportReceiptsController : BaseController<ImportReceipt>
    {
        public ImportReceiptsController(IGenericRepository<ImportReceipt> repo) : base(repo)
        {
        }
    }
}
