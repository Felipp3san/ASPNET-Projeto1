using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ASPNETLogin.Models;

public partial class Membro
{
    public int Id { get; set; }

    [DisplayName("Nome Do Membro")]
    public string NomeMembro { get; set; } = default!;

    [DisplayName("ID da Equipa")]
    public int EquipaId { get; set; }

    public virtual Equipa Equipa { get; set; } = null!;
}
