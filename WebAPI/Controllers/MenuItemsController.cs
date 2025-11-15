using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MenuItemsController(AppDbContext context) => _context = context;

        // GET: api/menuitems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.MenuItems.ToListAsync();
            return Ok(items);
        }

        // GET: api/menuitems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/menuitems
        [HttpPost]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return Ok(menuItem);
        }

        // PUT: api/menuitems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, MenuItem menuItem)
        {
            var existing = await _context.MenuItems.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = menuItem.Name;
            existing.Price = menuItem.Price;
            existing.Category = menuItem.Category;
            existing.IsAvailable = menuItem.IsAvailable;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE: api/menuitems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
