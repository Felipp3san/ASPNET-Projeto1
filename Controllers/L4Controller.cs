using Microsoft.AspNetCore.Mvc;
using ASPNETLogin.Data;
using Microsoft.EntityFrameworkCore;

namespace ASPNETLogin.Controllers;
public class L4Controller : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<L4Controller> _logger;
    
    public L4Controller(ApplicationDbContext context, ILogger<L4Controller> logger )
    {
        _context = context; 
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.MaiorIDMembro = await EncontrarMaiorIDMembro();
        ViewBag.MenorIDEquipa = await EncontrarMenorIDEquipa();
        ViewBag.SomaIDEquipas = await SomarIDEquipas();
        ViewBag.SomaIDFalcoes = await SomarIDEquipa("Falcões do Picoto");
        ViewBag.SomaIDMembrosFalcoes = await SomarIDMembrosEquipa("Falcões do Picoto");
        ViewBag.ContarEquipasComMembroAbel = await ContarEquipasMembro("Abel");
        ViewBag.ContarMembrosMaximinenseFalcoes = await ContarMembrosEquipas("Maximinense", "Falcões do Picoto");
        ViewBag.ListaOrdenadaMembrosMaximinenseFalcoes = await ListarMembrosDeEquipasOrdenados("Maximinense", "Falcões do Picoto");
        ViewBag.ListaMembrosIdsMaximinense = await ListarNomesIDsMembrosOrdenadosEquipas("Maximinense");
        ViewBag.ListaEquipaComMembros = await ListarEquipasComMembros();
        return View();
    }

    async Task<int> EncontrarMaiorIDMembro()
    {
        return await _context.Tmembros.MaxAsync(e => e.Id);
    }

    async Task<int> EncontrarMenorIDEquipa()
    {
        return await _context.Tmembros.MinAsync(e => e.Id);
    }

    async Task<int> SomarIDEquipas()
    {
        return await _context.Tequipas.SumAsync(e => e.Id);
    }

    async Task<int> SomarIDEquipa(string nomeEquipa)
    {
        return await _context.Tequipas.Where(e => e.NomeEquipa == nomeEquipa)
            .SumAsync(e => e.Id);
    }
    
    async Task<int> SomarIDMembrosEquipa(string nomeEquipa)
    {
        int equipaId = await BuscarIdEquipa(nomeEquipa);

        if (equipaId != 0)
        {
            return await _context.Tmembros.Where(e => e.EquipaId == equipaId)
                .SumAsync(e => e.Id);
        }

        return 0;
    }

    async Task<int> BuscarIdEquipa(string nomeEquipa)
    {
        var equipa = await _context.Tequipas.FirstOrDefaultAsync(e => e.NomeEquipa == nomeEquipa); 

        return equipa?.Id ?? 0;
    }

    async Task<int> ContarEquipasMembro(string nomeMembro)
    {
        return await _context.Tmembros.Where(e => e.NomeMembro == nomeMembro)
            .Select(e => e.EquipaId)
            .Distinct()
            .CountAsync();
    }

    async Task<int> ContarMembrosEquipas(params string[] equipas)
    {  
        int total = 0;

        foreach (var equipa in equipas)
        {
            int equipaId = await BuscarIdEquipa(equipa); 

            if (equipaId != 0)
            {
                total += await _context.Tmembros.Where(e => e.EquipaId == equipaId)
                    .CountAsync();
            }           
        }
        
        return total;
    }

    async Task<List<string>> ListarNomesIDsMembrosOrdenadosEquipas(params string[] equipas)
    {
        var membrosIds = new List<string>();
            
        foreach (var equipa in equipas)
        {
            int equipaId = await BuscarIdEquipa(equipa);

            if(equipaId != 0)
            {
                var listaTemporaria = await _context.Tmembros.Where(e => e.EquipaId == equipaId)
                    .Select(e => $"{e.NomeMembro} - {e.Id}")
                    .ToListAsync();

                membrosIds.AddRange(listaTemporaria);
            }
        }

        return membrosIds;
    }

    async Task<List<string>> ListarMembrosDeEquipasOrdenados(params string[] equipas)
    {
        var membros = new List<string>();
            
        foreach (var equipa in equipas)
        {
            int equipaId = await BuscarIdEquipa(equipa);

            if(equipaId != 0) 
            {
                var listaTemporaria = await _context.Tmembros.Where(e => e.EquipaId == equipaId)
                    .Select(e => e.NomeMembro)
                    .ToListAsync();

                membros.AddRange(listaTemporaria);
            }
        }

        return membros.Order().ToList();
    }

    async Task<List<string>> ListarEquipasComMembros()
    {
        var equipasMembros = new List<string>();
        var equipas = await _context.Tequipas.ToListAsync();

        foreach (var equipa in equipas)
        {
            List<string> membros = await _context.Tmembros.Where(e => e.EquipaId == equipa.Id)
                .Select(e => e.NomeMembro)
                .ToListAsync();
            
            string membrosString = string.Join(", ", membros);

            equipasMembros.Add($"{equipa.NomeEquipa} <-> {membrosString}");
        }        
        
        return equipasMembros;
    }
}    
