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

    public virtual DbSet<Dibujo> Dibujos { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioSala> UsuarioSalas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-32VDJB4\\SQLEXPRESS;Database=GaragatoDatabase;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dibujo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dibujo__3214EC07271E7825");

            entity.ToTable("Dibujo");

            entity.Property(e => e.Dibujo1)
                .IsUnicode(false)
                .HasColumnName("Dibujo");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Dibujos)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dibujo__IdSala__02FC7413");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Dibujos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dibujo__IdUsuari__03F0984C");
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
