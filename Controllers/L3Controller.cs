using Microsoft.AspNetCore.Mvc;
using ASPNETLogin.Data;
using Microsoft.EntityFrameworkCore;

namespace ASPNETLogin.Controllers;
public class L3Controller : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<L3Controller> _logger;
    
    public L3Controller(ApplicationDbContext context, ILogger<L3Controller> logger )
    {
        _context = context; 
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.NomePrimeiraEquipa = await NomePrimeiraEquipa();
        ViewBag.EquipaArsenalExiste = await VerificarEquipaExiste("Arsenal da Devesa");
        ViewBag.QtMembrosEquipaQuatro = await ContarMembrosEquipa(4);
        ViewBag.QtMembrosLeoes = await ContarMembrosEquipa("Leões da Tecla");
        ViewBag.QtMembrosEquipaArcenal = await ContarMembrosEquipa("Arcenal da Debeza");

        return View();
    }
    
    public async Task<int?> EncontrarIdEquipa(string nomeEquipa)
    {
        var equipa = await _context.Tequipas.FirstOrDefaultAsync(e => e.NomeEquipa == nomeEquipa);

        return equipa?.Id; 
    }

    public async Task<string?> NomePrimeiraEquipa()
    {
        var equipa = await _context.Tequipas.FirstOrDefaultAsync();
            
        return equipa?.NomeEquipa; 
    }

    public async Task<bool> VerificarEquipaExiste(string nomeEquipa)
    {
        return await _context.Tequipas.AnyAsync(e => e.NomeEquipa == nomeEquipa);
    }
    
    public async Task<bool> VerificarEquipaExiste(int equipaId)
    {
        return await _context.Tequipas.AnyAsync(e => e.Id == equipaId);
    }

    public async Task<int?> ContarMembrosEquipa(string nomeEquipa)
    {
        var equipaId = await EncontrarIdEquipa(nomeEquipa);

        if (equipaId != null)
        {
            return await _context.Tmembros.Where(e => e.EquipaId == (int)equipaId)
                .CountAsync();
        }
        else {
            return null;
        }
    }

    public async Task<int?> ContarMembrosEquipa(int equipaId)
    {
        if (await VerificarEquipaExiste(equipaId))
        {
            return await _context.Tmembros.Where(e => e.EquipaId == equipaId)
                .CountAsync();
        }
        else {
            return null;
        }
    }
}
