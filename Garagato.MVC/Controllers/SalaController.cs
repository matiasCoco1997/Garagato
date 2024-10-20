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

    [HttpPost]
    public async Task<IActionResult> Crear(string nombreSalaInput, string contraseniaInput)
    {

        var token = Request.Cookies["AuthToken"];
        var usuarioCreador = _usuarioService.ObtenerUsuarioLogueado(token); 
        if (usuarioCreador == null)
        {
            return Unauthorized();
        }

        await _salaService.CrearSalaGaragatoAsync(nombreSalaInput, usuarioCreador);

        // Obtener la última sala creada
        var salaCreada = _salaService.ObtenerUltimaSalaCreada();

        // Redirigir al método Juego con el id de la sala
        return RedirectToAction("Juego", new { id = salaCreada.SalaId });
    }


    public async Task <IActionResult> Juego(string id)
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
                    Posicion = item.Item3,
                    idJugador = item.Item4
                };
                salaViewModel.InformacionSala.Add(jugador);
            }
            salaViewModel.nombreSala = salaEncontrada.NombreSala;
            salaViewModel.idSala = salaEncontrada.SalaId;

            var token = Request.Cookies["AuthToken"];

            if (token != null)
            {
               // Obtener el usuario logueado
              var usuarioLogueado = _usuarioService.ObtenerUsuarioLogueado(token);

                // Verificar si el usuario ya está en la sala
                bool usuarioEstaEnSala = await _salaService.UsuarioEstaEnSalaAsync(salaEncontrada.SalaId, usuarioLogueado.Id);

                if (!usuarioEstaEnSala)
                {
                    // Si no está en la sala, guardar en la tabla intermedia UsuarioSala
                    await _salaService.GuardarUsuarioSalaAsync(salaEncontrada.SalaId, usuarioLogueado.Id);
                }
            }
        }
        return View("Index", salaViewModel);
    }

} 
