using Microsoft.AspNetCore.Mvc;
using TableTabsApp.Data;
using TableTabsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace TableTabsApp.Controllers;

public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string tableNumber)
    {
        if (string.IsNullOrEmpty(tableNumber) || !tableNumber.StartsWith("A") || !int.TryParse(tableNumber.Substring(1), out int tableId) || tableId < 1 || tableId > 10)
        {
            ViewBag.Error = "Invalid table number.";
            return View();
        }

        HttpContext.Session.SetString("TableNumber", tableNumber);
        return RedirectToAction("Menu");
    }

    public IActionResult Menu()
    {
        var tableNumber = HttpContext.Session.GetString("TableNumber");
        if (string.IsNullOrEmpty(tableNumber))
        {
            return RedirectToAction("Login");
        }

        var menuItems = _context.MenuItems.ToList();
        return View(menuItems);
    }

    [HttpPost]
    public IActionResult PlaceOrder(int menuItemId, int quantity)
    {
        var tableNumber = HttpContext.Session.GetString("TableNumber");
        if (string.IsNullOrEmpty(tableNumber))
        {
            return RedirectToAction("Login");
        }

        var order = new Order
        {
            TableName = tableNumber,
            MenuItemId = menuItemId,
            Quantity = quantity
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        return RedirectToAction("Menu");
    }

    public IActionResult MyOrders()
    {
        var tableNumber = HttpContext.Session.GetString("TableNumber");
        if (string.IsNullOrEmpty(tableNumber))
        {
            return RedirectToAction("Login");
        }

        var orders = _context.Orders.Include(o => o.MenuItem).Where(o => o.TableName == tableNumber).ToList();
        return View(orders);
    }
}
