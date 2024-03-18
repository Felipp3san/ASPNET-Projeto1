using Microsoft.AspNetCore.Mvc;
using ASPNETLogin.Data;
using Microsoft.EntityFrameworkCore;
using ASPNETLogin.Models;

namespace ASPNETLogin.Controllers;
public class O1Controller : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<O1Controller> _logger;
    
    public O1Controller(ApplicationDbContext context, ILogger<O1Controller> logger )
    {
        _context = context; 
        _logger = logger;
    }

    public async Task<IActionResult> Index(int id)
    {
        ViewBag.ListaEquipas = await ListarEquipas();
        ViewBag.ListaEquipas = await ListarEquipas();
        ViewBag.ListaMembros = await ListarMembros(id);
        ViewBag.EquipaSelecionada = await BuscarEquipa(id);
        ViewBag.ContagemMembros = await ContarMembros();
        ViewBag.ContagemMembrosEquipa = await ContarMembros(id);

        return View();
    }

    async Task<List<Equipa>> ListarEquipas()
    {
        return await _context.Tequipas.ToListAsync();
    }

    async Task<List<Membro>> ListarMembros(int id)
    {
        return await _context.Tmembros.Where(e => e.EquipaId == id).ToListAsync();
    }

    async Task<int> ContarMembros()
    {
        return await _context.Tmembros.CountAsync();
    }

    async Task<int> ContarMembros(int id)
    {
        return await _context.Tmembros.Where(e => e.EquipaId == id).CountAsync();
    }

    async Task<Equipa> BuscarEquipa(int id)
    {
        return await _context.Tequipas.FirstOrDefaultAsync(e => e.Id == id);
    }
}