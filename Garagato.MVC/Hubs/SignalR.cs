using Garagato.Logica;
using Garagato.Data.EF;
using Microsoft.AspNetCore.SignalR;

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

    public async Task CrearSalaGaragatoAsync(string nombreSala, string contrasenia)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        if (token != null)
        {
            var creadorSala = _usuarioService.ObtenerUsuarioLogueado(token);
            await _salaService.CrearSalaGaragatoAsync(nombreSala, creadorSala);
            await Clients.All.SendAsync("MostrarSalaGaragato", nombreSala, contrasenia, creadorSala.Nombre);
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
