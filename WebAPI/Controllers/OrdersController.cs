using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrdersController(AppDbContext context) => _context = context;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders.ToListAsync();

            var result = new List<object>();

            foreach (var order in orders)
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == order.Id)
                    .ToListAsync();

                var itemsWithMenu = new List<object>();
                foreach (var oi in orderItems)
                {
                    var menuItem = await _context.MenuItems.FindAsync(oi.MenuItemId);
                    itemsWithMenu.Add(new
                    {
                        oi.Id,
                        MenuItem = menuItem,
                        oi.Quantity,
                        oi.Price
                    });
                }

                result.Add(new
                {
                    order.Id,
                    Date = order.CreatedAt,
                    order.TotalAmount,
                    OrderItems = itemsWithMenu
                });
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            order.OrderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == order.Id)
                .ToListAsync();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (order.OrderItems == null || !order.OrderItems.Any())
                return BadRequest("Order must have at least one item.");

            decimal totalAmount = 0;
            foreach (var item in order.OrderItems)
            {
                var menu = await _context.MenuItems.FindAsync(item.MenuItemId);
                if (menu == null)
                    return BadRequest($"Menu item with ID {item.MenuItemId} not found.");

                item.Price = menu.Price;
                totalAmount += item.Subtotal;
            }

            order.TotalAmount = totalAmount;
            order.CreatedAt = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Reload with items (no navigation properties needed)
            var createdOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            createdOrder.OrderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == createdOrder.Id)
                .ToListAsync();

            return Ok(createdOrder);
        }
    }
}
