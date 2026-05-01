using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class CategoriesController : BaseController<TheLoai, int>
    {
        public CategoriesController(IGenericRepository<TheLoai, int> repo) : base(repo)
        {
        }
    }
}
