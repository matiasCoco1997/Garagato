using Garagato.Data.EF;
using Garagato.Logica;
using Garagato.MVC.Models;
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

    public async Task EnviarRespuestaAsync(string respuesta)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];
        var Usuario = await _usuarioService.ObtenerUsuarioLogueado(token);
        await Clients.Caller.SendAsync("MostrarRespuesta", "Tú", respuesta);
        await Clients.Others.SendAsync("MostrarRespuesta", Usuario.Nombre, respuesta);
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
                    await Clients.Others.SendAsync("MostrarSalaGaragato", nombreSala, creadorSala.Nombre, salaCreada.SalaId);
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

                var cantidadDeJugadoresEnSala = salaBuscada.UsuarioSalas.Count;

                if (cantidadDeJugadoresEnSala < 5)
                {
                    Usuario UsuarioNuevoEnSala = await _usuarioService.ObtenerUsuarioLogueado(token);

                    var jugadorYaExisteEnLaSala = await _salaService.UsuarioEstaEnSalaAsync(salaBuscada.SalaId, UsuarioNuevoEnSala.Id);

                    if (!jugadorYaExisteEnLaSala)
                    {
                        await _salaService.GuardarUsuarioSalaAsync(salaBuscada.SalaId, UsuarioNuevoEnSala.Id);
                    }

                    salaBuscada = await _salaService.BuscarSalaPorId(idSala);

                    cantidadDeJugadoresEnSala = salaBuscada.UsuarioSalas.Count;

                    if (salaBuscada.UsuarioSalas.Count == 5)
                    {
                        await Clients.All.SendAsync("deshabilitarBotonUnirseASala", salaBuscada.SalaId);
                    }

                    var resultado = _salaService.SetInformacionNuevoJugador(salaBuscada, UsuarioNuevoEnSala);

                    DataJugador nuevoJugador = new DataJugador()
                    {
                        NombreJugador = resultado.Item1,
                        idJugador = resultado.Item2
                    };

                    await Clients.All.SendAsync("cambiarContadorDeJugadores", salaBuscada.SalaId, cantidadDeJugadoresEnSala);
                    await Clients.Others.SendAsync("agregarUsuarioASala", nuevoJugador, salaBuscada.SalaId, jugadorYaExisteEnLaSala);
                    await Clients.Caller.SendAsync("redirect", "/Sala/Juego/" + idSala);
                }
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
                var cantidadDeJugadoresEnSala = salaBuscada.UsuarioSalas.Count;

                if (cantidadDeJugadoresEnSala == 5)
                {
                    await Clients.All.SendAsync("habilitarBotonUnirseSala", salaBuscada.SalaId);
                }

                bool resultadoOperacion = await _salaService.borrarUsuarioDeSala(UsuarioSalirSala, salaBuscada);

                if (resultadoOperacion)
                {
                    await Clients.All.SendAsync("cambiarContadorDeJugadores", salaBuscada.SalaId, (cantidadDeJugadoresEnSala-1) );
                    await Clients.All.SendAsync("borrarUsuarioDeSala", UsuarioSalirSala.Id);
                    await Clients.Caller.SendAsync("redirect", "/Home/Index");
                }
            }
        }
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

    public async Task BorrarDibujoAsync(string pizarra, int idSala)
    {
        var token = Context.GetHttpContext().Request.Cookies["AuthToken"];

        if (token != null)
        {
            Usuario UsuarioLogueado = await _usuarioService.ObtenerUsuarioLogueado(token);
            List<string> dibujoABorrar = await _salaService.ObtenerDibujosABorrarAsync(idSala, UsuarioLogueado.Id);
            await _salaService.BorrarDibujosAsync(idSala, UsuarioLogueado.Id);
            await Clients.All.SendAsync("BorrarDibujo", pizarra, dibujoABorrar, UsuarioLogueado.Id);
        } 
    }
}
