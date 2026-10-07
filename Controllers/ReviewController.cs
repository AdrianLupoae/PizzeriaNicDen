using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaNicDen.Data;
using PizzeriaNicDen.Models;
using System.Security.Claims;

namespace PizzeriaNicDen.Controllers
{
    // Doar utilizatorii conectați cu Google pot da Like
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly PizzeriaContext _context;

        public ReviewController(PizzeriaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike([FromBody] int pizzaId)
        {
            // Extragem ID-ul utilizatorului conectat din Cookie
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int utilizatorId))
            {
                return Unauthorized(); // Returnează eroare dacă nu e logat
            }

            // Verificăm dacă a dat deja like acestei pizza
            var existingLike = await _context.AprecieriPizze
                .FirstOrDefaultAsync(a => a.PizzaId == pizzaId && a.UtilizatorId == utilizatorId);

            bool isLiked;

            if (existingLike != null)
            {
                // Dacă există deja, îl ștergem (Toggle Off)
                _context.AprecieriPizze.Remove(existingLike);
                isLiked = false;
            }
            else
            {
                // Dacă nu există, adăugăm like-ul (Toggle On)
                _context.AprecieriPizze.Add(new AprecierePizza { PizzaId = pizzaId, UtilizatorId = utilizatorId });
                isLiked = true;
            }

            await _context.SaveChangesAsync();

            // Numărăm totalul actualizat de like-uri pentru acea pizza
            var newCount = await _context.AprecieriPizze.CountAsync(a => a.PizzaId == pizzaId);

            // Returnăm datele către JavaScript
            return Json(new { success = true, count = newCount, liked = isLiked });
        }
    }
}