using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class CategoriesController : BaseController<Category>
    {
        public CategoriesController(IGenericRepository<Category> repo) : base(repo)
        {
        }
    }
}
