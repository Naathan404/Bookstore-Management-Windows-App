using Bookstore.API.Interfaces;
using Bookstore.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TKey> : ControllerBase where TEntity : class, IEntity<TKey>
    {
        // generic
        protected readonly IGenericRepository<TEntity, TKey> _repository;
        public BaseController(IGenericRepository<TEntity, TKey> repo)
        {
            _repository = repo;
        }

        [HttpGet] // get all
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")] // get by id
        public async Task<ActionResult<TEntity>> GetByID(TKey id)
        {
            var item = await _repository.GetByIDAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost] // add new item
        public async Task<ActionResult<TEntity>> Add(TEntity item)
        {
            await _repository.AddAsync(item);
            await _repository.SaveChangesAsync();
            var newID = item.GetID();
            return CreatedAtAction("GetByID", new { id = newID }, item);
        }

        [HttpPut("{id}")] // update by id
        public async Task<ActionResult<TEntity>> Update(TKey id, TEntity item)
        {
            if (!EqualityComparer<TKey>.Default.Equals(id, item.GetID()))
                return BadRequest();
            _repository.Update(item);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                var existing = await _repository.GetByIDAsync(id);
                if(existing == null) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")] // remove by id
        public async Task<ActionResult> Delete(TKey id)
        {
            var item = await _repository.GetByIDAsync(id);
            if(item == null) return NotFound();
            _repository.Delete(item);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}
