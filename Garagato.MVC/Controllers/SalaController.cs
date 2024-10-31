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

    public async Task<IActionResult> Juego(string id)
    {
        var idSala = -1;

        SalaViewModel salaViewModel = new SalaViewModel();

        salaViewModel.InformacionSala = new List<DataJugador>();

        if (int.TryParse(id, out idSala))
        {
            Sala salaEncontrada = await _salaService.BuscarSalaPorId(idSala);

            var informacionSalaObtenida = await _salaService.SetInformacionSala(salaEncontrada);

            foreach (var item in informacionSalaObtenida)
            {
                DataJugador jugador = new DataJugador()
                {
                    NombreJugador = item.Item1,
                    idJugador = item.Item2
                };
                salaViewModel.InformacionSala.Add(jugador);
            }
            salaViewModel.nombreSala = salaEncontrada.NombreSala;
            salaViewModel.idSala = salaEncontrada.SalaId;
            var dibujosPrevios = await _salaService.TraerDibujosDeUnaSala(salaEncontrada.SalaId);
            if (dibujosPrevios.Count > 0)
            {
                salaViewModel.dibujosPrevios = dibujosPrevios;
            }
            
        }
        return View("Index", salaViewModel);
    }

}
