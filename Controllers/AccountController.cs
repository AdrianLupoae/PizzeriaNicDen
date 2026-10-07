using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PizzeriaNicDen.Data;
using PizzeriaNicDen.Models;

namespace PizzeriaNicDen.Controllers
{
    public class AccountController : Controller
    {
        private readonly PizzeriaContext _context;
        private readonly HashSet<string> _adminEmails;

        public AccountController(PizzeriaContext context, IConfiguration config)
        {
            _context = context;

            // Emailurile de Owner se citesc din appsettings.json -> "Owners:Emails"
            var owners = config.GetSection("Owners:Emails").Get<string[]>() ?? Array.Empty<string>();
            _adminEmails = new HashSet<string>(owners, StringComparer.OrdinalIgnoreCase);
        }

        // Această acțiune declanșează fereastra de logare Google
        public IActionResult Login()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Aici ne întoarce Google după ce utilizatorul se autentifică
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return RedirectToAction("Index", "Home");

            // Extragem datele trimise de Google
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null)
                return RedirectToAction("Index", "Home");

            // Căutăm utilizatorul în baza noastră de date SQLite
            var utilizator = await _context.Utilizatori.FirstOrDefaultAsync(u => u.Email == email);

            if (utilizator == null)
            {
                // Dacă nu există, îl creăm (prima logare)
                utilizator = new Utilizator
                {
                    Email = email,
                    Nume = name ?? "Utilizator",
                    NumarLogari = 1,
                    Rol = _adminEmails.Contains(email) ? "Admin" : "Client"
                };
                _context.Utilizatori.Add(utilizator);
            }
            else
            {
                // Dacă există, incrementăm numărul de logări
                utilizator.NumarLogari += 1;

                // Actualizăm rolul dacă între timp i-ai adăugat adresa în lista de Owners
                if (_adminEmails.Contains(email) && utilizator.Rol != "Admin")
                {
                    utilizator.Rol = "Admin";
                }
            }

            await _context.SaveChangesAsync();

            // Recreăm Cookie-ul aplicației pentru a include rolul și ID-ul din baza noastră de date
            var localClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilizator.Id.ToString()),
                new Claim(ClaimTypes.Email, utilizator.Email),
                new Claim(ClaimTypes.Name, utilizator.Nume),
                new Claim(ClaimTypes.Role, utilizator.Rol)
            };

            var localIdentity = new ClaimsIdentity(localClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(localIdentity));

            return RedirectToAction("Index", "Home");
        }

        // Acțiunea pentru deconectare
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}