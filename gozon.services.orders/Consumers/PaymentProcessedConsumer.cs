using gozon.services.orders.Data;
using gozon.services.orders.Entities;
using gozon.shared;
using MassTransit;

namespace gozon.services.orders.Consumers;

public class PaymentProcessedConsumer(OrderDbContext dbContext) : IConsumer<PaymentProcessedEvent>
{
    public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
    {
        var orderId = context.Message.OrderId;
        var order = await dbContext.Orders.FindAsync(orderId);
        if (order == null) 
            return;
        if (context.Message.IsSuccess)
        {
            order.Status = OrderStatus.Finished;
        }
        else
        {
            order.Status = OrderStatus.Cancelled;
        }

        await dbContext.SaveChangesAsync();
    }
}