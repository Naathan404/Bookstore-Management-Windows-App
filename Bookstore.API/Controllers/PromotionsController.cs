using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class PromotionsController : BaseController<UuDai>
    {
        public PromotionsController(IGenericRepository<UuDai> repo) : base(repo)
        {
        }
    }
}
