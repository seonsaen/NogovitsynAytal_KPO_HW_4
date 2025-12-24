using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gozon.frontend.Models;
using gozon.frontend.Services;

namespace gozon.frontend.Controllers;

public class HomeController(GozonApiService apiService) : Controller
{
    private static Guid CurrentUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public async Task<IActionResult> Index(Guid? userId)
    {
        if (userId.HasValue) CurrentUserId = userId.Value;

        var model = new DashboardViewModel
        {
            UserId = CurrentUserId,
            Balance = await apiService.GetBalanceAsync(CurrentUserId),
            Orders = await apiService.GetOrdersAsync(CurrentUserId)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount()
    {
        await apiService.CreateAccountAsync(CurrentUserId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> TopUp(decimal topUpAmount)
    {
        await apiService.TopUpAsync(CurrentUserId, topUpAmount);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(decimal orderAmount, string orderDescription)
    {
        await apiService.CreateOrderAsync(CurrentUserId, orderAmount, orderDescription);
        await Task.Delay(500); 
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult ChangeUser(Guid userId)
    {
        return RedirectToAction("Index", new { userId });
    }
}
