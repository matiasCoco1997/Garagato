using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Garagato.MVC.EF;

public partial class GaragatoDatabaseContext : DbContext
{
    public GaragatoDatabaseContext()
    {
    }

    public GaragatoDatabaseContext(DbContextOptions<GaragatoDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Palabra> Palabras { get; set; }

    public virtual DbSet<Puntuacion> Puntuacions { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioSala> UsuarioSalas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-78BPKE0\\SQLEXPRESS;Database=GaragatoDatabase;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Palabra>(entity =>
        {
            entity.HasKey(e => e.PalabraId).HasName("PK__Palabra__8B6EAEF1539C94D7");

            entity.ToTable("Palabra");

            entity.Property(e => e.PalabraId).HasColumnName("PalabraID");
            entity.Property(e => e.Palabra1)
                .HasMaxLength(100)
                .HasColumnName("Palabra");
        });

        modelBuilder.Entity<Puntuacion>(entity =>
        {
            entity.HasKey(e => e.PuntuacionId).HasName("PK__Puntuaci__4DEA4BE579269C10");

            entity.ToTable("Puntuacion");

            entity.Property(e => e.PuntuacionId).HasColumnName("PuntuacionID");
            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Sala).WithMany(p => p.Puntuacions)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puntuacio__SalaI__1A14E395");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Puntuacions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puntuacio__Usuar__1B0907CE");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.SalaId).HasName("PK__Sala__0428485AD5F30E45");

            entity.ToTable("Sala");

            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.CreadorSala).HasMaxLength(50);
            entity.Property(e => e.NombreSala).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Contrasena).HasMaxLength(50);
            entity.Property(e => e.Mail).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<UsuarioSala>(entity =>
        {
            entity.HasKey(e => e.UsuarioSalaId).HasName("PK__UsuarioS__9B45A0FDFB15AA26");

            entity.ToTable("UsuarioSala");

            entity.Property(e => e.UsuarioSalaId).HasColumnName("UsuarioSalaID");
            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Sala).WithMany(p => p.UsuarioSalas)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioSa__SalaI__1BFD2C07");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioSalas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioSa__Usuar__1CF15040");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
