using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaNicDen.Data;
using PizzeriaNicDen.Models;

namespace PizzeriaNicDen.Controllers;

public class HomeController : Controller
{
    private readonly PizzeriaContext _context;

    // Injectăm baza de date în controller
    public HomeController(PizzeriaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Extragem toate pizzele. 
        // Folosim .Include() pentru a aduce și datele din tabelul legat (PizzaFamily)
        var pizze = await _context.Pizze
                                  .Include(p => p.VariantaFamily)
                                  .Include(p => p.Aprecieri)
                                  .ToListAsync();
                                  
        // Trimitem lista către fișierul HTML (View)
        return View(pizze);
    }
    public IActionResult Termeni() => View();
    public IActionResult Confidentialitate() => View();
    public IActionResult Cookies() => View();
    public IActionResult Alergeni() => View();

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}