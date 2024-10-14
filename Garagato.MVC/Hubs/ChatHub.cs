using Microsoft.AspNetCore.SignalR;

namespace Garagato.MVC.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task CrearSalaGaragatoAsync(string nombreSala, string contrasenia)
    {
        await Clients.All.SendAsync("MostrarSalaGaragato", nombreSala, contrasenia);
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
