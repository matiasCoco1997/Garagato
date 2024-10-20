using Garagato.Logica;
using Garagato.Data.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Garagato.MVC.Models;

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

        SalaViewModel salaViewModel = new SalaViewModel();

        salaViewModel.InformacionSala = new List<DataJugador>();

        if (int.TryParse(id, out idSala))
        {
            var sala = _salaService.BuscarSalaPorId(idSala);

            if (sala.UsuarioSalas != null)
            {
                foreach (var usuarioSala in sala.UsuarioSalas)
                {
                    var usuario = usuarioSala.Usuario;

                    if (usuario != null)
                    {
                        foreach (var puntuacion in sala.Puntuacions)
                        {
                            if (usuario.Id == puntuacion.UsuarioId)
                            {

                                DataJugador jugador = new DataJugador() 
                                {
                                    NombreSala = sala.NombreSala,
                                    NombreJugador = usuario.Nombre,
                                    Puntos = puntuacion.Puntos,
                                    Posicion = 1
                                };
                                salaViewModel.InformacionSala.Add(jugador);
                            }
                        }
                    }
                }
                
            }
        }
        return View("Index", salaViewModel);
    }
}
