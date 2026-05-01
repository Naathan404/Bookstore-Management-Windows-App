using Bookstore.API.Models;
using Bookstore.API.Interfaces;

namespace Bookstore.API.Controllers
{
    public class BooksController : BaseController<Sach>
    {
        public BooksController(IGenericRepository<Sach> repo) : base(repo)
        {
        }
    }
}
