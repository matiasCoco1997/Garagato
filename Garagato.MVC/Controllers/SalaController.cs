using Garagato.Logica;
using Garagato.Data.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers;

[Authorize]
public class SalaController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly ISalaServicio _salaService;
    private readonly IUsuarioServicio _usuarioService;

    public SalaController(ILogger<HomeController> logger, ISalaServicio salaServicio, IUsuarioServicio usuarioServicio)
    {
        _logger = logger;
        _salaService = salaServicio;
        _usuarioService = usuarioServicio;
    }


    public IActionResult Juego(string id)
    {
        var idSala = -1;

        if (int.TryParse(id, out idSala))
        {
            var sala = _salaService.BuscarSalaPorId(idSala);

            if (sala != null && sala.UsuarioSalas != null)
            {
                var usuarios = new List<Usuario>();

                foreach (var usuarioSala in sala.UsuarioSalas)
                {
                    if (usuarioSala.Usuario != null)
                    {
                        usuarios.Add(usuarioSala.Usuario);
                    }
                }
                ViewBag.usuarios = usuarios;
            }
        }
        return View("Index");
    }
}
