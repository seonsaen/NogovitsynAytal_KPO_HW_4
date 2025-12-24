namespace gozon.shared;

public record OrderCreatedEvent(Guid OrderId, Guid UserId, decimal Amount);
