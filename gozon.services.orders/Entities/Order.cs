namespace gozon.services.orders.Entities;

public enum OrderStatus { New, Finished, Cancelled }

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}