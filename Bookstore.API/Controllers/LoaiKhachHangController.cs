using Bookstore.API.Data;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Bookstore.Share.DTOResponses;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;
using Bookstore.Share.DTO;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiKhachHangController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LoaiKhachHangController(AppDbContext context)
        {
            _context = context;
        }
        private static Expression<Func<LoaiKhachHang, CustomerTierResponse>> MapToDTOResponse()
        {
            return l => new CustomerTierResponse
            {
                MaLoaiKhachHang = l.MaLoaiKhachHang,
                TenLoaiKhachHang = l.TenLoaiKhachHang,
                NoToiDa = l.NoToiDa,
                TiLeTraToiThieu = l.TiLeTraToiThieu
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerTierResponse>>> GetAll()
        {
            var customerTiers = await _context.LoaiKhachHang.Select(MapToDTOResponse()).ToListAsync();
            return Ok(customerTiers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerTierResponse>> GetDetailById(int id)
        {
            var customerTier = await _context.LoaiKhachHang
                .Where(l => l.MaLoaiKhachHang == id)
                .Select(MapToDTOResponse())
                .FirstOrDefaultAsync();
            return Ok(customerTier);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoaiKhachHang([FromBody] CustomerTierRequest request)
        {
            var newLoaiKhachHang = new LoaiKhachHang
            {
                TenLoaiKhachHang = request.TenLoaiKhachHang,
                NoToiDa = request.NoToiDa,
                TiLeTraToiThieu = request.TiLeTraToiThieu
            };
            _context.LoaiKhachHang.Add(newLoaiKhachHang);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDetailById), new { id = newLoaiKhachHang.MaLoaiKhachHang }, newLoaiKhachHang);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoaiKhachHang(int id, CustomerTierRequest request)
        {
            var existingLoaiKhachHang = await _context.LoaiKhachHang.FindAsync(id);
            if (existingLoaiKhachHang == null)
            {
                return NotFound();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //TODO: Kiểm tra ràng buộc các phiếu thu
                existingLoaiKhachHang.TenLoaiKhachHang = request.TenLoaiKhachHang;
                existingLoaiKhachHang.NoToiDa = request.NoToiDa;
                existingLoaiKhachHang.TiLeTraToiThieu = request.TiLeTraToiThieu;
                _context.LoaiKhachHang.Update(existingLoaiKhachHang);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiKhachHang(int id)
        {
            var existingLoaiKhachHang = await _context.LoaiKhachHang.FindAsync(id);
            if (existingLoaiKhachHang == null)
            {
                return NotFound();
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var relatedKhachHang = await _context.KhachHang.Where(k => k.MaLoaiKhachHang == id).ToListAsync();
                if (relatedKhachHang.Any())
                {
                    return BadRequest("Không thể xóa loại khách hàng này vì có khách hàng liên quan.");
                }
                _context.LoaiKhachHang.Remove(existingLoaiKhachHang);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
    }
}
