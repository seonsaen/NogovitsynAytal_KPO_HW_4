using System.ComponentModel.DataAnnotations;

namespace gozon.services.payments.Entities;

public class Account
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    [ConcurrencyCheck]
    public decimal Balance { get; set; }
    
    [Timestamp] 
    public uint Version { get; set; }
}