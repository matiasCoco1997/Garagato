using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Garagato.Data.EF;

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
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2JV6N88\\SQLEXPRESS;Database=GaragatoDatabase;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Palabra>(entity =>
        {
            entity.HasKey(e => e.PalabraId).HasName("PK__Palabra__8B6EAEF159EF5D82");

            entity.ToTable("Palabra");

            entity.Property(e => e.PalabraId).HasColumnName("PalabraID");
            entity.Property(e => e.Palabra1)
                .HasMaxLength(100)
                .HasColumnName("Palabra");
        });

        modelBuilder.Entity<Puntuacion>(entity =>
        {
            entity.HasKey(e => e.PuntuacionId).HasName("PK__Puntuaci__4DEA4BE5E1DA8F6B");

            entity.ToTable("Puntuacion");

            entity.Property(e => e.PuntuacionId).HasColumnName("PuntuacionID");
            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Sala).WithMany(p => p.Puntuacions)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puntuacio__SalaI__4316F928");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Puntuacions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puntuacio__Usuar__4222D4EF");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.SalaId).HasName("PK__Sala__0428485A4F6ED19A");

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
            entity.HasKey(e => e.UsuarioSalaId).HasName("PK__UsuarioS__9B45A0FDDAA68317");

            entity.ToTable("UsuarioSala");

            entity.Property(e => e.UsuarioSalaId).HasColumnName("UsuarioSalaID");
            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Sala).WithMany(p => p.UsuarioSalas)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioSa__SalaI__3E52440B");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioSalas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioSa__Usuar__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
