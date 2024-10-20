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
        Task GuardarUsuarioSalaAsync(int salaId, int usuarioId);
        Task<bool> UsuarioEstaEnSalaAsync(int salaId, int usuarioId);
        List<Tuple<string, int, int, int>> SetInformacionSala(Sala salaEncontrada);
        Task<bool> borrarUsuarioDeSala(Usuario usuarioABorrar, Sala sala);

    }
    public class SalaServicio : ISalaServicio
    {
        private GaragatoDatabaseContext _context;

        public SalaServicio(GaragatoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> UsuarioEstaEnSalaAsync(int salaId, int usuarioId)
        {
            return await _context.UsuarioSalas.AnyAsync(us => us.SalaId == salaId && us.UsuarioId == usuarioId);
        }


        public List<Sala> ObtenerSalas()
        {
            return  _context.Salas
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

        public List<Tuple<string, int, int, int>> SetInformacionSala(Sala salaEncontrada)
        {
            var resultado = new List<Tuple<string, int, int, int>>();

            if (salaEncontrada.UsuarioSalas != null)
            {
                foreach (var usuarioSala in salaEncontrada.UsuarioSalas)
                {
                    var usuario = usuarioSala.Usuario;

                    foreach (var puntuacion in salaEncontrada.Puntuacions)
                    {
                        if (usuario.Id == puntuacion.UsuarioId)
                        {
                            //falta la logica para agarrar la posicion del jugador segun los puntos
                            resultado.Add(Tuple.Create(usuario.Nombre, puntuacion.Puntos, 1, usuario.Id));
                        }
                    }
                }
            }
            return resultado;
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

            var SalaCreada = this.ObtenerUltimaSalaCreada();

            var usuarioSala = new UsuarioSala
            {
                UsuarioId = creadorSala.Id,
                SalaId = SalaCreada.SalaId
            };

            _context.UsuarioSalas.Add(usuarioSala);
            await _context.SaveChangesAsync();

            await AgregarPuntuacionAsync(SalaCreada.SalaId, creadorSala.Id);
        }

        public async Task GuardarUsuarioSalaAsync(int salaId, int usuarioId)
        {
            var usuarioSala = new UsuarioSala
            {
                SalaId = salaId,
                UsuarioId = usuarioId
            };

            _context.UsuarioSalas.Add(usuarioSala);
            await _context.SaveChangesAsync();

            await AgregarPuntuacionAsync(salaId, usuarioId);
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
                _context.SaveChanges();
                resultadoBorrado = true;
            }
            return resultadoBorrado;
        }

        private async Task AgregarPuntuacionAsync(int salaId, int usuarioId)
        {
            var puntuacion = new Puntuacion
            {
                UsuarioId = usuarioId,
                SalaId = salaId,
                Puntos = 0
            };

            _context.Puntuacions.Add(puntuacion);
            await _context.SaveChangesAsync();
        }

    }
}
