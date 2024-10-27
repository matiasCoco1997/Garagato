using System;
using System.Collections.Generic;

namespace Garagato.Data.EF;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Dibujo> Dibujos { get; set; } = new List<Dibujo>();

    public virtual ICollection<UsuarioSala> UsuarioSalas { get; set; } = new List<UsuarioSala>();
}
