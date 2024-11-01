using Garagato.Data.EF;
using Microsoft.EntityFrameworkCore;


namespace Garagato.Logica
{

    public interface ISalaServicio
    {
        List<Sala> ObtenerSalas();
        Task<Sala> CrearSalaGaragatoAsync(string nombreSala, Usuario creadorSala);
        Task<Sala> BuscarSalaPorId(int salaId);
        Task GuardarUsuarioSalaAsync(int salaId, int usuarioId);
        Task<bool> UsuarioEstaEnSalaAsync(int salaId, int usuarioId);
        Task<List<Tuple<string, int>>> SetInformacionSala(Sala salaEncontrada);
        Task<bool> borrarUsuarioDeSala(Usuario usuarioABorrar, Sala sala);
        Tuple<string, int> SetInformacionNuevoJugador(Sala salaBuscada, Usuario UsuarioNuevoEnSala);
        Task GuardarDibujoAsync(int idSala, int idUsuarioDibujante, string dibujo);
        Task<List<string>> TraerDibujosDeUnaSala(int salaId);
        Task<List<string>> ObtenerDibujosABorrarAsync(int idSala , int idUsuario);
        Task BorrarDibujosAsync(int idSala, int idUsuario);

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
                    .ToList();
        }

        public async Task<Sala> BuscarSalaPorId(int salaId)
        {
            return _context.Salas
                    .Include(s => s.UsuarioSalas).ThenInclude(us => us.Usuario)
                    .FirstOrDefault(s => s.SalaId == salaId);
        }

        public async Task<List<Tuple<string, int>>> SetInformacionSala(Sala salaEncontrada)
        {
            var resultado = new List<Tuple<string, int>>();

            if (salaEncontrada.UsuarioSalas != null)
            {
                foreach (var usuarioSala in salaEncontrada.UsuarioSalas)
                {
                    var usuario = usuarioSala.Usuario;
                    resultado.Add(Tuple.Create(usuario.Nombre, usuario.Id));
                }
            }
            return resultado;
        }


        public Tuple<string, int> SetInformacionNuevoJugador(Sala salaBuscada, Usuario UsuarioNuevoEnSala)
        {
            string nombreJugador = "";
            int idJugador = 0;

            if (salaBuscada.UsuarioSalas != null)
            {
                foreach (var usuarioSala in salaBuscada.UsuarioSalas)
                {
                    var usuario = usuarioSala.Usuario;

                    nombreJugador = usuario.Nombre;
                    idJugador = usuario.Id;

                }
            }
            return new Tuple<string, int>(nombreJugador, idJugador);
        }

        public async Task<Sala> CrearSalaGaragatoAsync(string nombreSala, Usuario creadorSala)
        {
            var nuevaSala = new Sala
            {
                NombreSala = nombreSala,
                CreadorSala = creadorSala.Nombre
            };

            _context.Salas.Add(nuevaSala);
            await _context.SaveChangesAsync();

            var SalaCreada = await ObtenerUltimaSalaCreada();

            var usuarioSala = new UsuarioSala
            {
                UsuarioId = creadorSala.Id,
                SalaId = SalaCreada.SalaId
            };

            _context.UsuarioSalas.Add(usuarioSala);
            await _context.SaveChangesAsync();

            return nuevaSala;
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
        }

        public async Task GuardarDibujoAsync(int idSala, int idUsuarioDibujante, string dibujo)
        {

            var nuevoDibujo = new Dibujo()
            {
                IdSala = idSala,
                Dibujo1 = dibujo,
                IdUsuario = idUsuarioDibujante
            };

            _context.Dibujos.Add(nuevoDibujo);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> borrarUsuarioDeSala(Usuario usuarioABorrar, Sala sala)
        {
            bool resultadoBorrado = false;

            var usuarioSala = _context.UsuarioSalas.FirstOrDefault(us => us.UsuarioId == usuarioABorrar.Id && us.SalaId == sala.SalaId);

            if (usuarioSala != null)
            {
                _context.UsuarioSalas.Remove(usuarioSala);
                _context.SaveChanges();
                resultadoBorrado = true;
            }
            return resultadoBorrado;
        }

        public async Task<List<string>> TraerDibujosDeUnaSala(int idSala)
        {
            return await _context.Dibujos
                         .Where(d => d.IdSala == idSala)
                         .Select( d => d.Dibujo1!)
                         .ToListAsync();
        }

        public async Task<List<string>> ObtenerDibujosABorrarAsync(int idSala, int idUsuario)
        {
             var dibujosABorrar = await _context.Dibujos
                                     .Where(d => d.IdSala == idSala && d.IdUsuario == idUsuario)
                                     .Select(d => d.Dibujo1!)
                                     .ToListAsync();

            return dibujosABorrar!;
        }

        public async Task BorrarDibujosAsync(int idSala, int idUsuario)
        {
            var dibujosABorrar = await _context.Dibujos
                                     .Where(d => d.IdSala == idSala && d.IdUsuario == idUsuario)
                                     .ToListAsync();
            if (dibujosABorrar != null)
            {
                _context.Dibujos.RemoveRange(dibujosABorrar);
                await _context.SaveChangesAsync();
            }
        }
        //-------------------------------------------------------- Private Functions -------------------------------------------------------------------------------------

        private async Task<Sala> ObtenerUltimaSalaCreada()
        {
            return _context.Salas.ToList().LastOrDefault();
        }
    }
}
