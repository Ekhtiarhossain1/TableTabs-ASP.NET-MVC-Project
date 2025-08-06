using Microsoft.AspNetCore.Mvc;
using TableTabsApp.Data;
using TableTabsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace TableTabsApp.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
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
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        HttpContext.Session.SetString("Admin", username);
        return RedirectToAction("Dashboard");
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        var menuItems = _context.MenuItems.ToList();
        return View(menuItems);
    }

    public IActionResult AddMenuItem()
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        return View();
    }

    [HttpPost]
    public IActionResult AddMenuItem(MenuItem menuItem)
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        _context.MenuItems.Add(menuItem);
        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }

    public IActionResult EditMenuItem(int id)
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        var menuItem = _context.MenuItems.Find(id);
        if (menuItem == null)
        {
            return NotFound();
        }

        return View(menuItem);
    }

    [HttpPost]
    public IActionResult EditMenuItem(MenuItem menuItem)
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        _context.MenuItems.Update(menuItem);
        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }

    public IActionResult DeleteMenuItem(int id)
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        var menuItem = _context.MenuItems.Find(id);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
            _context.SaveChanges();
        }

        return RedirectToAction("Dashboard");
    }

    public IActionResult OrderQueue()
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        var orders = _context.Orders.Include(o => o.MenuItem).ToList();
        return View(orders);
    }

    public IActionResult ServeOrder(int id)
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        var order = _context.Orders.Find(id);
        if (order != null && order.IsReady)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        return RedirectToAction("OrderQueue");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("Admin");
        return RedirectToAction("Login");
    }

    public IActionResult CreateUser()
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        return View();
    }

    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        if (HttpContext.Session.GetString("Admin") == null)
        {
            return RedirectToAction("Login");
        }

        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        return View(user);
    }
}
