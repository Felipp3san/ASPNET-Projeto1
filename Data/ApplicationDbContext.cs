using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ASPNETLogin.Models;
namespace ASPNETLogin.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Membro> Tmembros { get; set; } = default!;
    public DbSet<Equipa> Tequipas { get; set; } = default!;

}
