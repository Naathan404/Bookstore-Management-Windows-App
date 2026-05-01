using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class CategoriesController : BaseController<TheLoai>
    {
        public CategoriesController(IGenericRepository<TheLoai> repo) : base(repo)
        {
        }
    }
}
