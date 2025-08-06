using Microsoft.AspNetCore.Mvc;
using TableTabsApp.Data;
using TableTabsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace TableTabsApp.Controllers;

public class KitchenController : Controller
{
    private readonly ApplicationDbContext _context;

    public KitchenController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _context.KitchenUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }
        HttpContext.Session.SetString("KitchenUser", username);
        return RedirectToAction("OrderQueue");
    }

    public IActionResult OrderQueue()
    {
        if (HttpContext.Session.GetString("KitchenUser") == null)
            return RedirectToAction("Login");
        var orders = _context.Orders.Include(o => o.MenuItem).Where(o => !o.IsReady).ToList();
        return View(orders);
    }

    public IActionResult MarkReady(int id)
    {
        if (HttpContext.Session.GetString("KitchenUser") == null)
            return RedirectToAction("Login");
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            order.IsReady = true;
            _context.SaveChanges();
        }
        return RedirectToAction("OrderQueue");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("KitchenUser");
        return RedirectToAction("Login");
    }
}
