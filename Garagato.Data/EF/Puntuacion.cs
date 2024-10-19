using System;
using System.Collections.Generic;

namespace Garagato.Data.EF;

public partial class Puntuacion
{
    public int PuntuacionId { get; set; }

    public int UsuarioId { get; set; }

    public int SalaId { get; set; }

    public int Puntos { get; set; }

    public virtual Sala Sala { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
