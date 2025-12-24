using gozon.services.orders.Data;
using gozon.services.orders.Entities;
using gozon.shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gozon.services.orders.Controllers;

public record CreateOrderDto(Guid UserId, decimal Amount, string Description);

[ApiController]
[Route("api/orders")]
public class OrdersController(OrderDbContext dbContext, IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Amount = dto.Amount,
            Description = dto.Description,
            Status = OrderStatus.New
        };
        
        dbContext.Orders.Add(order);

        await publishEndpoint.Publish(new OrderCreatedEvent(order.Id, order.UserId, order.Amount));
        
        await dbContext.SaveChangesAsync();

        return Ok(new { order.Id, Status = "Created" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStatus(Guid id) 
    {
        var order = await dbContext.Orders.FindAsync(id);
        return order is null ? NotFound() : Ok(order);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        var orders = await dbContext.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        
        return Ok(orders);
    }
}