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

        List<Tuple<string, int, int>> SetInformacionSala(Sala salaEncontrada);

    }
    public class SalaServicio : ISalaServicio
    {
        private GaragatoDatabaseContext _context;

        public SalaServicio(GaragatoDatabaseContext context)
        {
            _context = context;
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

        public List<Tuple<string, int, int>> SetInformacionSala(Sala salaEncontrada)
        {
            var resultado = new List<Tuple<string, int, int>>();

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
                            resultado.Add(Tuple.Create(usuario.Nombre, puntuacion.Puntos, 1));
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

            var puntuacion = new Puntuacion
            {
                UsuarioId = creadorSala.Id,
                SalaId = SalaCreada.SalaId,
                Puntos = 0
            };

            _context.UsuarioSalas.Add(usuarioSala);
            _context.Puntuacions.Add(puntuacion);
            await _context.SaveChangesAsync();
        }

    }
}
