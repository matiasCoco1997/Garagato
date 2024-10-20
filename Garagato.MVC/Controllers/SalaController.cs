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
            Sala salaEncontrada = _salaService.BuscarSalaPorId(idSala);

            var informacionSalaObtenida = _salaService.SetInformacionSala(salaEncontrada);

            foreach (var item in informacionSalaObtenida)
            {
                DataJugador jugador = new DataJugador()
                {
                    NombreJugador = item.Item1,
                    Puntos = item.Item2,
                    Posicion = item.Item3
                };
                salaViewModel.InformacionSala.Add(jugador);
            }
            salaViewModel.nombreSala = salaEncontrada.NombreSala;
        }
        return View("Index", salaViewModel);
    }
}
