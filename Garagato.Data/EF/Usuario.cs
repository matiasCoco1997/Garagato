using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Garagato.Data.EF;

public partial class Usuario : IdentityUser
{

    public string Nombre { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Puntuacion> Puntuacions { get; set; } = new List<Puntuacion>();

    public virtual ICollection<UsuarioSala> UsuarioSalas { get; set; } = new List<UsuarioSala>();
}
