using System;
using System.Collections.Generic;

namespace Garagato.Data.EF;

public partial class Sala
{
    public int SalaId { get; set; }

    public string NombreSala { get; set; } = null!;

    public string CreadorSala { get; set; } = null!;

    public virtual ICollection<Dibujo> Dibujos { get; set; } = new List<Dibujo>();

    public virtual ICollection<UsuarioSala> UsuarioSalas { get; set; } = new List<UsuarioSala>();
}
