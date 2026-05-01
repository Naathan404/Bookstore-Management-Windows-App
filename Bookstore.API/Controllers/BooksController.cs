using Bookstore.API.Models;
using Bookstore.API.Interfaces;

namespace Bookstore.API.Controllers
{
    public class BooksController : BaseController<Sach, int>
    {
        public BooksController(IGenericRepository<Sach, int> repo) : base(repo)
        {
        }
    }
}
