using gozon.services.payments.Data;
using gozon.services.payments.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gozon.services.payments.Controllers;

public record CreateAccountDto(Guid UserId);
public record TopUpDto(Guid UserId, decimal Amount);

[ApiController]
[Route("api/payments")]
public class PaymentsController(PaymentDbContext dbContext) : ControllerBase
{
    [HttpPost("account")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        if (await dbContext.Accounts.AnyAsync(a => a.UserId == dto.UserId))
            return BadRequest("Account already exists");

        var account = new Account { Id = Guid.NewGuid(), UserId = dto.UserId, Balance = 0 };
        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();
        return Ok(account.Id);
    }

    [HttpPost("topup")]
    public async Task<IActionResult> TopUp([FromBody] TopUpDto dto)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.UserId == dto.UserId);
        if (account == null) return NotFound();

        account.Balance += dto.Amount;
        await dbContext.SaveChangesAsync();
        return Ok(new { account.Balance });
    }

    [HttpGet("balance/{userId}")]
    public async Task<IActionResult> GetBalance(Guid userId)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
        return account == null ? NotFound() : Ok(new { account.Balance });
    }
}