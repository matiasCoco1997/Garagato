using Garagato.Logica;
using Garagato.Data.EF;
using Microsoft.AspNetCore.SignalR;
using Garagato.MVC.Models;

namespace Garagato.MVC.Hubs;

public class signalR : Hub
{
    private readonly ISalaServicio _salaService;
    private readonly IUsuarioServicio _usuarioService;

    public signalR(ISalaServicio salaServicio, IUsuarioServicio usuarioServicio)
    {
        _salaService = salaServicio;
        _usuarioService = usuarioServicio;
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task CrearSalaGaragatoAsync(string nombreSala)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        if (token != null)
        {
            var creadorSala = _usuarioService.ObtenerUsuarioLogueado(token);
            await _salaService.CrearSalaGaragatoAsync(nombreSala, creadorSala);
            var salaCreada = _salaService.ObtenerUltimaSalaCreada();
            await Clients.All.SendAsync("MostrarSalaGaragato", nombreSala, creadorSala.Nombre, salaCreada.SalaId);
            await Clients.Caller.SendAsync("redirect", "/Sala/Juego/" + salaCreada.SalaId);
        }
    }

    public async Task salirDeSalaAsync(int idSala)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        if (token != null)
        {
            Usuario UsuarioSalirSala = _usuarioService.ObtenerUsuarioLogueado(token);
            Sala salaBuscada = _salaService.BuscarSalaPorId(idSala);

            if (salaBuscada != null)
            {
                bool resultadoOperacion = await _salaService.borrarUsuarioDeSala(UsuarioSalirSala, salaBuscada);
                if (resultadoOperacion)
                {
                    await Clients.All.SendAsync("borrarUsuarioDeSala", UsuarioSalirSala.Id);
                    await Clients.Caller.SendAsync("redirect", "/Home/Index");
                }
            }
        }
    }

    public async Task UnirseASalaAsync(int idSala)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        if (token != null)
        {
            Sala salaBuscada = _salaService.BuscarSalaPorId(idSala);

            if (salaBuscada != null)
            {
                Usuario UsuarioNuevoEnSala = _usuarioService.ObtenerUsuarioLogueado(token);

                if (!await _salaService.UsuarioEstaEnSalaAsync(salaBuscada.SalaId, UsuarioNuevoEnSala.Id))
                {
                    await _salaService.GuardarUsuarioSalaAsync(salaBuscada.SalaId, UsuarioNuevoEnSala.Id);
                }
                salaBuscada = _salaService.BuscarSalaPorId(idSala);

                var resultado = _salaService.SetInformacionNuevoJugador(salaBuscada, UsuarioNuevoEnSala);

                DataJugador nuevoJugador = new DataJugador()
                {
                    NombreJugador = resultado.Item1,
                    Puntos = resultado.Item2,
                    Posicion = resultado.Item3, //cambiar segun la posicion de la base de datos en el servicio que setea esto "SetInformacionNuevoJugador"
                    idJugador = resultado.Item4
                };
                await Clients.Others.SendAsync("agregarUsuarioASala", nuevoJugador);
                await Clients.Caller.SendAsync("redirect", "/Sala/Juego/" + idSala);
            }
        }
    }

    public async Task EnviarRespuestaAsync(string respuesta)
    {
        await Clients.All.SendAsync("MostrarRespuesta", respuesta);
    }

    public async Task DibujarAsync(string dibujo)
    {
        await Clients.All.SendAsync("CrearDibujo", dibujo);
    }

    public async Task BorrarDibujoAsync(string pizarra)
    {
        await Clients.All.SendAsync("BorrarDibujo", pizarra);
    }
}
