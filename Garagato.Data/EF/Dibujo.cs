using System;
using System.Collections.Generic;

namespace Garagato.Data.EF;

public partial class Dibujo
{
    public int Id { get; set; }

    public string? Dibujo1 { get; set; }

    public int IdSala { get; set; }

    public int IdUsuario { get; set; }

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
