using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class BookCountsController : BaseController<BookCount>
    {
        public BookCountsController(IGenericRepository<BookCount> repo) : base(repo)
        {
        }
    }
}
