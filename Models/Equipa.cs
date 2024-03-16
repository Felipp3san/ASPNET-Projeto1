using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ASPNETLogin.Models;

public partial class Equipa 
{
    public int Id { get; set; }

    [DisplayName("Nome Da Equipa")]
    public string NomeEquipa { get; set; } = default!;

    public virtual ICollection<Membro> Membros { get; set; } = new List<Membro>();
}
