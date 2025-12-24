namespace gozon.shared;

public record PaymentProcessedEvent(Guid OrderId, bool IsSuccess, string? FailReason);