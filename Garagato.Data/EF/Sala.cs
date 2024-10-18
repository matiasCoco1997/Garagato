using System;
using System.Collections.Generic;

namespace Garagato.Data.EF;

public partial class Sala
{
    public int SalaId { get; set; }

    public string NombreSala { get; set; } = null!;

    public string CreadorSala { get; set; } = null!;

    public virtual ICollection<Puntuacion> Puntuacions { get; set; } = new List<Puntuacion>();

    public virtual ICollection<UsuarioSala> UsuarioSalas { get; set; } = new List<UsuarioSala>();
}
