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
        => optionsBuilder.UseSqlServer("Server=DESKTOP-32VDJB4\\SQLEXPRESS;Database=GaragatoDatabase;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Palabra>(entity =>
        {
            entity.HasKey(e => e.PalabraId).HasName("PK__Palabra__8B6EAEF122D07D5D");

            entity.ToTable("Palabra");

            entity.Property(e => e.PalabraId).HasColumnName("PalabraID");
            entity.Property(e => e.Palabra1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Palabra");
        });

        modelBuilder.Entity<Puntuacion>(entity =>
        {
            entity.HasKey(e => e.PuntuacionId).HasName("PK__Puntuaci__4DEA4BE5CDF68766");

            entity.ToTable("Puntuacion");

            entity.Property(e => e.PuntuacionId).HasColumnName("PuntuacionID");
            entity.Property(e => e.Puntuacion1).HasColumnName("Puntuacion");
            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Sala).WithMany(p => p.Puntuacions)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puntuacio__SalaI__656C112C");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Puntuacions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puntuacio__Usuar__6477ECF3");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.SalaId).HasName("PK__Sala__0428485A03B7D14B");

            entity.ToTable("Sala");

            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.NombreSala)
                .HasMaxLength(50)
                .IsUnicode(false);
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
            entity.HasKey(e => e.UsuarioSalaId).HasName("PK__UsuarioS__9B45A0FD990082A3");

            entity.ToTable("UsuarioSala");

            entity.Property(e => e.UsuarioSalaId).HasColumnName("UsuarioSalaID");
            entity.Property(e => e.SalaId).HasColumnName("SalaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Sala).WithMany(p => p.UsuarioSalas)
                .HasForeignKey(d => d.SalaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioSa__SalaI__619B8048");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioSalas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsuarioSa__Usuar__60A75C0F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
