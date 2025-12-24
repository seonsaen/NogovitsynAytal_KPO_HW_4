namespace gozon.frontend.Models;

public record OrderViewModel(Guid Id, decimal Amount, string Description, int Status, DateTime CreatedAt);

public class DashboardViewModel
{
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public List<OrderViewModel> Orders { get; set; } = new();
    
    public decimal TopUpAmount { get; set; }
    public decimal OrderAmount { get; set; }
    public string OrderDescription { get; set; } = string.Empty;
}