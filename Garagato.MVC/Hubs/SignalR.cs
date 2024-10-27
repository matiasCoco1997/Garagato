using Garagato.Logica;
using Garagato.Data.EF;
using Microsoft.AspNetCore.SignalR;
using Garagato.MVC.Models;
using System.Text.Json;

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
            var creadorSala = await _usuarioService.ObtenerUsuarioLogueado(token);
            if (creadorSala != null)
            {
                var salaCreada = await _salaService.CrearSalaGaragatoAsync(nombreSala, creadorSala);
                if (salaCreada != null)
                {
                    await Clients.All.SendAsync("MostrarSalaGaragato", nombreSala, creadorSala.Nombre, salaCreada.SalaId);
                    await Clients.Caller.SendAsync("redirect", "/Sala/Juego/" + salaCreada.SalaId);
                }
            }
        }
    }

    public async Task UnirseASalaAsync(int idSala)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        if (token != null)
        {
            Sala salaBuscada = await _salaService.BuscarSalaPorId(idSala);

            if (salaBuscada != null)
            {
                Usuario UsuarioNuevoEnSala = await _usuarioService.ObtenerUsuarioLogueado(token);

                if (!await _salaService.UsuarioEstaEnSalaAsync(salaBuscada.SalaId, UsuarioNuevoEnSala.Id))
                {
                    await _salaService.GuardarUsuarioSalaAsync(salaBuscada.SalaId, UsuarioNuevoEnSala.Id);
                }
                salaBuscada = await _salaService.BuscarSalaPorId(idSala);

                var resultado = _salaService.SetInformacionNuevoJugador(salaBuscada, UsuarioNuevoEnSala);

                DataJugador nuevoJugador = new DataJugador()
                {
                    NombreJugador = resultado.Item1,
                    idJugador = resultado.Item2
                };

                List<Dibujo> dibujos = await _salaService.TraerDibujosDeUnaSala(salaBuscada.SalaId);
                
                if (dibujos.Count != 0)
                {
                    List<string> dibujosPrevios = await _salaService.SetearDibujos(dibujos);
                    await Clients.Caller.SendAsync("cargarDibujosPrevios", dibujosPrevios, salaBuscada.SalaId);
                }
                
                await Clients.Others.SendAsync("agregarUsuarioASala", nuevoJugador);
                await Clients.Caller.SendAsync("redirect", "/Sala/Juego/" + idSala);
            }
        }
    }

    public async Task salirDeSalaAsync(int idSala)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        if (token != null)
        {
            Usuario UsuarioSalirSala = await _usuarioService.ObtenerUsuarioLogueado(token);
            Sala salaBuscada = await _salaService.BuscarSalaPorId(idSala);

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

    public async Task EnviarRespuestaAsync(string respuesta)
    {
        await Clients.All.SendAsync("MostrarRespuesta", respuesta);
    }

    public async Task DibujarAsync(string dibujo, int idSala)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        
        if (token != null)
        {
            Usuario UsuarioDibujante = await _usuarioService.ObtenerUsuarioLogueado(token);
            await _salaService.GuardarDibujoAsync(idSala, UsuarioDibujante.Id, dibujo);
            await Clients.All.SendAsync("CrearDibujo", dibujo, idSala);
        }
    }

    public async Task BorrarDibujoAsync(string pizarra)
    {
        await Clients.All.SendAsync("BorrarDibujo", pizarra);
    }
}
