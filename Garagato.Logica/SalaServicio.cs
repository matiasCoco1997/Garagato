using Garagato.Data.EF;
using Microsoft.EntityFrameworkCore;


namespace Garagato.Logica
{

    public interface ISalaServicio
    {
        List<Sala> ObtenerSalas();
        Task CrearSalaGaragatoAsync(string nombreSala, Usuario creadorSala);
        Sala ObtenerUltimaSalaCreada();
        Sala BuscarSalaPorId(int salaId);
        Task GuardarUsuarioSalaAsync(int salaId, string usuarioId);
        Task<bool> UsuarioEstaEnSalaAsync(int salaId, string usuarioId);
        public List<Tuple<string, int, int, string>> SetInformacionSala(Sala salaEncontrada);
        Task<bool> borrarUsuarioDeSala(Usuario usuarioABorrar, Sala sala);
        Tuple<string, int, int, string> SetInformacionNuevoJugador(Sala salaBuscada, Usuario usuarioNuevoEnSala);

    }
    public class SalaServicio : ISalaServicio
    {
        private GaragatoDatabaseContext _context;

        public SalaServicio(GaragatoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> UsuarioEstaEnSalaAsync(int salaId, string usuarioId)
        {
            return await _context.UsuarioSalas.AnyAsync(us => us.SalaId == salaId && us.UsuarioId == usuarioId);
        }

        public List<Sala> ObtenerSalas()
        {
            return _context.Salas
                    .Include(s => s.UsuarioSalas)
                    .Include(s => s.Puntuacions)
                    .ToList();
        }

        public Sala ObtenerUltimaSalaCreada()
        {
            return _context.Salas.ToList().LastOrDefault();
        }

        public Sala BuscarSalaPorId(int salaId)
        {
            return _context.Salas
                    .Include(s => s.UsuarioSalas).ThenInclude(us => us.Usuario)
                    .Include(s => s.Puntuacions)
                    .FirstOrDefault(s => s.SalaId == salaId);
        }

        public List<Tuple<string, int, int, string>> SetInformacionSala(Sala salaEncontrada)
        {
            var resultado = new List<Tuple<string, int, int, string>>();

            if (salaEncontrada.UsuarioSalas != null)
            {
                foreach (var usuarioSala in salaEncontrada.UsuarioSalas)
                {
                    var usuario = usuarioSala.Usuario;

                    foreach (var puntuacion in salaEncontrada.Puntuacions)
                    {
                        if (usuario.Id == puntuacion.UsuarioId)
                        {
                            // lógica para determinar la posición del jugador según los puntos
                            resultado.Add(Tuple.Create(usuario.Nombre, puntuacion.Puntos, 1, usuario.Id));
                        }
                    }
                }
            }
            return resultado;
        }

        public Tuple<string, int, int, string> SetInformacionNuevoJugador(Sala salaBuscada, Usuario usuarioNuevoEnSala)
        {
            string nombreJugador = "";
            int puntosJugador = 0;
            int posicionJugador = 1; // CAMBIAR CON LO DE LA BASE DE DATOS DESPUES
            string idJugador = ""; // Cambiar a string

            if (salaBuscada.UsuarioSalas != null)
            {
                foreach (var usuarioSala in salaBuscada.UsuarioSalas)
                {
                    var usuario = usuarioSala.Usuario;

                    foreach (var puntuacion in salaBuscada.Puntuacions)
                    {
                        if (usuario.Id == puntuacion.UsuarioId)
                        {
                            nombreJugador = usuario.Nombre;
                            puntosJugador = puntuacion.Puntos;
                            posicionJugador = 1; // lógica para determinar la posición del jugador según los puntos
                            idJugador = usuario.Id; // Cambiar a string
                        }
                    }
                }
            }
            return new Tuple<string, int, int, string>(nombreJugador, puntosJugador, posicionJugador, idJugador);
        }

        public async Task CrearSalaGaragatoAsync(string nombreSala, Usuario creadorSala)
        {
            var nuevaSala = new Sala
            {
                NombreSala = nombreSala,
                CreadorSala = creadorSala.Nombre
            };

            _context.Salas.Add(nuevaSala);
            await _context.SaveChangesAsync();

            var salaCreada = this.ObtenerUltimaSalaCreada();

            var usuarioSala = new UsuarioSala
            {
                UsuarioId = creadorSala.Id, // usar Id de IdentityUser
                SalaId = salaCreada.SalaId
            };

            _context.UsuarioSalas.Add(usuarioSala);
            await _context.SaveChangesAsync();

            await AgregarPuntuacionAsync(salaCreada.SalaId, creadorSala.Id); // usar Id de IdentityUser
        }

        public async Task GuardarUsuarioSalaAsync(int salaId, string usuarioId)
        {
            var usuarioSala = new UsuarioSala
            {
                SalaId = salaId,
                UsuarioId = usuarioId // usar Id de IdentityUser
            };

            _context.UsuarioSalas.Add(usuarioSala);
            await _context.SaveChangesAsync();

            await AgregarPuntuacionAsync(salaId, usuarioId); // usar Id de IdentityUser
        }

        public async Task<bool> borrarUsuarioDeSala(Usuario usuarioABorrar, Sala sala)
        {
            bool resultadoBorrado = false;

            var usuarioSala = _context.UsuarioSalas.FirstOrDefault(us => us.UsuarioId == usuarioABorrar.Id && us.SalaId == sala.SalaId);
            var puntuacion = _context.Puntuacions.FirstOrDefault(p => p.UsuarioId == usuarioABorrar.Id && p.SalaId == sala.SalaId);

            if (usuarioSala != null && puntuacion != null)
            {
                _context.Puntuacions.Remove(puntuacion);
                _context.UsuarioSalas.Remove(usuarioSala);
                await _context.SaveChangesAsync(); // Cambiar a async
                resultadoBorrado = true;
            }
            return resultadoBorrado;
        }

        private async Task AgregarPuntuacionAsync(int salaId, string usuarioId)
        {
            var puntuacion = new Puntuacion
            {
                UsuarioId = usuarioId, // usar Id de IdentityUser
                SalaId = salaId,
                Puntos = 0
            };

            _context.Puntuacions.Add(puntuacion);
            await _context.SaveChangesAsync();
        }

       
    }
}