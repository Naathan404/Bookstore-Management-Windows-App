using Bookstore.API.Data;
using Bookstore.Share.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoaiUuDaiController : ControllerBase
    {
        private AppDbContext _context;
        public LoaiUuDaiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PromotionTypeResponse>>> GetAll()
        {
            var result = await _context.LoaiUuDai
                .OrderBy(l => l.MaLoaiUuDai)
                .Select(l => new PromotionTypeResponse
                {
                    MaLoaiUuDai = l.MaLoaiUuDai,
                    TenLoaiUuDai = l.TenLoaiUuDai,
                    ApDungToiDa = l.ApDungToiDa
                })
                .ToListAsync();
            return Ok(result);
        }
    }
}
