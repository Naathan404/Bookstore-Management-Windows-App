using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class PromotionsController : BaseController<Promotion>
    {
        public PromotionsController(IGenericRepository<Promotion> repo) : base(repo)
        {
        }
    }
}
