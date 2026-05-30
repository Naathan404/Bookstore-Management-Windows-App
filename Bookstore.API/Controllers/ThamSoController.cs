using Bookstore.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThamSoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThamSoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("ti-le-gia-ban")]
        public async Task<IActionResult> GetTiLeGiaBan()
        {
            var thamSo = await _context.ThamSo
                .FirstOrDefaultAsync(t => t.TenThamSo == "TiLeDonGiaBan");

            if (thamSo == null)
                return NotFound(new { message = "Không tìm thấy tham số TiLeDonGiaBan trong DB." });

            return Ok(new ThamSoDTO
            {
                TenThamSo = thamSo.TenThamSo,
                GiaTri = thamSo.GiaTri / 100m
            });
        }

        [HttpGet("tien-thu-lon-hon-no")]
        public async Task<ActionResult<ThamSoDTO>> GetTienThuLonHonNo()
        {
            var tsTienThuLonHonNo = await _context.ThamSo.FindAsync("TienThuLonHonNo");
            string TenThamSo = (tsTienThuLonHonNo == null) ? "TienThuLonHonNo" : tsTienThuLonHonNo.TenThamSo;
            int GiaTri = (tsTienThuLonHonNo == null || tsTienThuLonHonNo.GiaTri == 0) ? 0 : 1;

            return Ok(new ThamSoDTO
            {
                TenThamSo = TenThamSo,
                GiaTri = GiaTri
            });
        }
    }
}
