using System;
using System.Collections.Generic;

namespace Garagato.Data.EF;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Contrasena { get; set; } = null!;
}
