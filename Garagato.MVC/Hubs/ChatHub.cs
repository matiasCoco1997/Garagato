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
}
