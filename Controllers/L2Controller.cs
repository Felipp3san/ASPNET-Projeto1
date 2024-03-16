using Microsoft.AspNetCore.Mvc;
using ASPNETLogin.Data;
using Microsoft.EntityFrameworkCore;

namespace ASPNETLogin.Controllers;
public class L2Controller : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<L2Controller> _logger;
    
    public L2Controller(ApplicationDbContext context, ILogger<L2Controller> logger )
    {
        _context = context; 
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Equipas = await ListarNomesEquipas();
        ViewBag.QtEquipas = await ContarEquipas();
        ViewBag.EquipasOrdenadas = await ListarNomesEquipasOrdenadas();
        ViewBag.EquipasOrdenadasDesc = await ListarNomesEquipasOrdenadasDesc(); 
        ViewBag.NomeEquipaTres = await EncontrarNomeEquipa(3);
        ViewBag.NomeEquipaOitentaEOito = await EncontrarNomeEquipa(88);

        return View();
    }
    
    async Task<List<string>> ListarNomesEquipas()
    {
        return await _context.Tequipas.Select(e => e.NomeEquipa)
            .ToListAsync();  
    }

    async Task<int> ContarEquipas()
    {
        return await _context.Tequipas.CountAsync();
    }

    async Task<List<string>> ListarNomesEquipasOrdenadas()
    {
        return await _context.Tequipas.OrderBy(e => e.NomeEquipa)
            .Select(e => e.NomeEquipa)
            .ToListAsync();
    }

    async Task<List<string>> ListarNomesEquipasOrdenadasDesc()
    {
        return await _context.Tequipas.OrderByDescending(e => e.NomeEquipa)
            .Select(e => e.NomeEquipa)
            .ToListAsync();
    }

    async Task<string?> EncontrarNomeEquipa(int equipaId)
    {
        var equipa = await _context.Tequipas.FirstOrDefaultAsync(e => e.Id == equipaId);

        return equipa?.NomeEquipa; 
    }

    async Task<int?> EncontrarIdEquipa(string nomeEquipa)
    {
        var equipa = await _context.Tequipas.FirstOrDefaultAsync(e => e.NomeEquipa == nomeEquipa);

        return equipa?.Id; 
    }
}
