using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaNicDen.Data;

namespace PizzeriaNicDen.ViewComponents;

public class AnuntActivViewComponent : ViewComponent
{
    private readonly PizzeriaContext _context;

    public AnuntActivViewComponent(PizzeriaContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var acum = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Europe/Bucharest");

        var lista = await _context.Anunturi
            .Where(a => a.Activ && a.DataInceput <= acum && a.DataSfarsit >= acum)
            .OrderByDescending(a => a.DataCreare)
            .ToListAsync();

        return View(lista);
    }
}