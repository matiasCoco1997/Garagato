using Garagato.Logica;
using Garagato.Data.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Garagato.MVC.Models;
using Microsoft.AspNetCore.Identity;

namespace Garagato.MVC.Controllers;

[Authorize]
public class SalaController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISalaServicio _salaService;
    private readonly IUsuarioServicio _usuarioService;
    private readonly UserManager<IdentityUser> _userManager;
    public SalaController(ILogger<HomeController> logger, ISalaServicio salaServicio, IUsuarioServicio usuarioServicio, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _salaService = salaServicio;
        _usuarioService = usuarioServicio;
        _userManager = userManager;
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

    public async Task<IActionResult> Juego(int id)
    {
        SalaViewModel salaViewModel = new SalaViewModel
        {
            InformacionSala = new List<DataJugador>()
        };

        // Obtener el ID del usuario autenticado
        var usuarioId = _userManager.GetUserId(User); // Obtener el ID del usuario actual

        // Buscar sala por ID
        Sala salaEncontrada = _salaService.BuscarSalaPorId(id);

        if (salaEncontrada != null)
        {
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
            salaViewModel.idSala = salaEncontrada.SalaId; // Si SalaId sigue siendo int
        }

        return View("Index", salaViewModel);
    }
}