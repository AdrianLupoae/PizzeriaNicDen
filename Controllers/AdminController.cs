using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaNicDen.Data;
using PizzeriaNicDen.Models;

namespace PizzeriaNicDen.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly PizzeriaContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(PizzeriaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var pizze = await _context.Pizze.Include(p => p.VariantaFamily).ToListAsync();
            return View(pizze);
        }
        public async Task<IActionResult> Recenzii()
{
    var recenzii = await _context.Recenzii
        .Include(r => r.Utilizator)
        .OrderByDescending(r => r.DataAdaugare)
        .ToListAsync();
        
    return View(recenzii);
}

        [HttpGet]
        public IActionResult AdaugaPizza()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdaugaPizza(Pizza pizza, IFormFile? pozaFisier, decimal? pret50cm, string? valori50cm)
        {
            // ELIMINĂM eroarea de validare, pentru că aceste câmpuri le completăm noi mai jos
            ModelState.Remove("CaleImagine");
            ModelState.Remove("VariantaFamily");

            if (ModelState.IsValid)
            {
                if (pozaFisier != null && pozaFisier.Length > 0)
                {
                    string numeFisierNou = Guid.NewGuid().ToString() + Path.GetExtension(pozaFisier.FileName);
                    string caleCompleta = Path.Combine(_env.WebRootPath, "img", numeFisierNou);
                    using (var stream = new FileStream(caleCompleta, FileMode.Create))
                    {
                        await pozaFisier.CopyToAsync(stream);
                    }
                    pizza.CaleImagine = "/img/" + numeFisierNou;
                }
                else
                {
                    pizza.CaleImagine = "/img/default.jpg";
                }

                _context.Pizze.Add(pizza);
                await _context.SaveChangesAsync();

                if (pret50cm.HasValue && pret50cm.Value > 0)
                {
                    var pizzaFamily = new PizzaFamily
                    {
                        PizzaId = pizza.Id,
                        Pret50cm = pret50cm.Value,
                        ValoriNutritionale50cm = valori50cm ?? string.Empty
                    };
                    _context.PizzeFamily.Add(pizzaFamily);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(pizza);
        }

        // --- NOU: AFIȘARE FORMULAR EDITARE ---
        [HttpGet]
        public async Task<IActionResult> EditeazaPizza(int id)
        {
            var pizza = await _context.Pizze.Include(p => p.VariantaFamily).FirstOrDefaultAsync(p => p.Id == id);
            if (pizza == null) return NotFound();
            
            return View(pizza);
        }

        // --- NOU: SALVARE MODIFICĂRI EDITARE ---
        [HttpPost]
        public async Task<IActionResult> EditeazaPizza(int id, Pizza pizzaForm, IFormFile? pozaFisier, decimal? pret50cm, string? valori50cm)
        {
            ModelState.Remove("CaleImagine");
            ModelState.Remove("VariantaFamily");

            if (ModelState.IsValid)
            {
                var pizzaDb = await _context.Pizze.Include(p => p.VariantaFamily).FirstOrDefaultAsync(p => p.Id == id);
                if (pizzaDb == null) return NotFound();

                // Actualizăm datele de bază
                pizzaDb.Nume = pizzaForm.Nume;
                pizzaDb.Ingrediente = pizzaForm.Ingrediente;
                pizzaDb.Pret30cm = pizzaForm.Pret30cm;
                pizzaDb.ValoriNutritionale30cm = pizzaForm.ValoriNutritionale30cm;

                // Dacă s-a urcat o poză nouă, o înlocuim
                if (pozaFisier != null && pozaFisier.Length > 0)
                {
                    string numeFisierNou = Guid.NewGuid().ToString() + Path.GetExtension(pozaFisier.FileName);
                    string caleCompleta = Path.Combine(_env.WebRootPath, "img", numeFisierNou);
                    using (var stream = new FileStream(caleCompleta, FileMode.Create))
                    {
                        await pozaFisier.CopyToAsync(stream);
                    }
                    pizzaDb.CaleImagine = "/img/" + numeFisierNou;
                }

                // Actualizăm sau ștergem varianta Family
                if (pret50cm.HasValue && pret50cm.Value > 0)
                {
                    if (pizzaDb.VariantaFamily == null)
                    {
                        pizzaDb.VariantaFamily = new PizzaFamily { PizzaId = pizzaDb.Id };
                    }
                    pizzaDb.VariantaFamily.Pret50cm = pret50cm.Value;
                    pizzaDb.VariantaFamily.ValoriNutritionale50cm = valori50cm ?? string.Empty;
                }
                else if (pizzaDb.VariantaFamily != null)
                {
                    // Dacă s-a lăsat prețul gol, înseamnă că vrem să scoatem varianta de 50cm
                    _context.PizzeFamily.Remove(pizzaDb.VariantaFamily);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pizzaForm);
        }
    }
}