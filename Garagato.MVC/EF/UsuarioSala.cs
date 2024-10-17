using System;
using System.Collections.Generic;

namespace Garagato.MVC.EF;

public partial class UsuarioSala
{
    public int UsuarioSalaId { get; set; }

    public int UsuarioId { get; set; }

    public int SalaId { get; set; }

    public virtual Sala Sala { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
