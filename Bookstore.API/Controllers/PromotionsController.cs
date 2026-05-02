using Bookstore.API.Interfaces;
using Bookstore.API.Models;

namespace Bookstore.API.Controllers
{
    public class PromotionsController : BaseController<UuDai, int>
    {
        public PromotionsController(IGenericRepository<UuDai, int> repo) : base(repo)
        {
        }
    }
}
