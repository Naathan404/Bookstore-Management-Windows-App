using Bookstore.API.Models;
using Bookstore.API.Interfaces;

namespace Bookstore.API.Controllers
{
    public class BooksController : BaseController<Book>
    {
        public BooksController(IGenericRepository<Book> repo) : base(repo)
        {
        }
    }
}
