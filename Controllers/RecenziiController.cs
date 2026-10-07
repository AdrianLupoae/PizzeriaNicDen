using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaNicDen.Data;
using PizzeriaNicDen.Models;
using System.Security.Claims;

namespace PizzeriaNicDen.Controllers
{
    public class RecenziiController : Controller
    {
        private readonly PizzeriaContext _context;

        public RecenziiController(PizzeriaContext context)
        {
            _context = context;
        }

        // Afișează toate recenziile
        public async Task<IActionResult> Index()
        {
            var recenzii = await _context.Recenzii
                .Include(r => r.Utilizator)
                .OrderByDescending(r => r.DataAdaugare)
                .ToListAsync();
                
            return View(recenzii);
        }

        // Procesează adăugarea unei recenzii (doar pentru cei logați)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Adauga(int stele, string text)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int utilizatorId))
            {
                var recenzie = new RecenziePizzerie
                {
                    Stele = stele,
                    Text = text,
                    UtilizatorId = utilizatorId,
                    DataAdaugare = DateTime.Now
                };
                
                _context.Recenzii.Add(recenzie);
                await _context.SaveChangesAsync();
            }

            // Redirecționăm înapoi la pagina de recenzii, dar trimitem un mesaj temporar
            // pentru a declanșa pop-up-ul către Google dacă recenzia e de 4 sau 5 stele.
            if (stele >= 4)
            {
                TempData["RecenzieBuna"] = "true";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}