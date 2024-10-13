using Garagato.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Garagato.MVC.Models.DataBase
{
    public class DataBaseConfig : DbContext
    {
        public DataBaseConfig(DbContextOptions<DataBaseConfig> options) : base(options) { 
        }

        public DbSet<Usuario> Usuarios { get; set; }

    }
}
