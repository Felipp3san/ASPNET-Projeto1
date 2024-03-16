using Microsoft.AspNetCore.Mvc;
using ASPNETLogin.Data;
using Microsoft.EntityFrameworkCore;
using ASPNETLogin.Models;

namespace ASPNETLogin.Controllers;
public class L1Controller : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<L1Controller> _logger;
    
    public L1Controller(ApplicationDbContext context, ILogger<L1Controller> logger )
    {
        _context = context; 
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ContagemEquipas = await ContarEquipas();
        ViewBag.ContagemMembros = await ContarMembros();
        ViewBag.ListaMembrosOrdenada = await ListarMembrosOrdenados();
        ViewBag.ContagemMembrosEquipa1 = await ContarMembros(1);

        return View();
    }

    async Task<int> ContarEquipas()
    {
        return await _context.Tequipas.CountAsync();
    }

    async Task<int> ContarMembros()
    {
        return await _context.Tmembros.CountAsync();
    }
    async Task<int> ContarMembros(int equipaId)
    {
        return await _context.Tmembros.Where(e => e.EquipaId == equipaId)
            .CountAsync();
    }

    async Task<List<string>> ListarMembrosOrdenados()
    {
        return await _context.Tmembros.OrderBy(e => e.NomeMembro)
            .Select(e => e.NomeMembro)
            .ToListAsync();
    }
}    
