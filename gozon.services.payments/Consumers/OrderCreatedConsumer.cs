using gozon.services.payments.Data;
using gozon.shared;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace gozon.services.payments.Consumers;

public class OrderCreatedConsumer(PaymentDbContext dbContext) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var msg = context.Message;
        
        var account = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.UserId == msg.UserId);

        bool success = false;
        string? reason = null;

        if (account == null)
        {
            reason = "Account not found";
        }
        else if (account.Balance < msg.Amount)
        {
            reason = "Insufficient funds";
        }
        else
        {
            try 
            {
                account.Balance -= msg.Amount;
                success = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw; 
            }
        }
        await context.Publish(new PaymentProcessedEvent(msg.OrderId, success, reason));
        await dbContext.SaveChangesAsync();
    }
}