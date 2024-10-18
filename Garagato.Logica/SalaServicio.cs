using Garagato.Data.EF;


namespace Garagato.Logica
{

    public interface ISalaServicio
    {
        List<Sala> ObtenerSalas();
        void CrearSala(int idUsuarioLogueado);

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
            return _context.Salas.ToList();
        }

        public void CrearSala(int idUsuarioLogueado)
        {
            // 1- Obtener el usuario logueado

            // 2- crear una sala

            // 3- insertar el usuario adentro de la tabla


            //_context.Usuarios.Add(usuario);
            //_context.SaveChanges();
        }

    }
}
