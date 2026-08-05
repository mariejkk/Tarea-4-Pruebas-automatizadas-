using GestionCanchas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionCanchas.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.NombreUsuario)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.NombreUsuario)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.NombreCompleto)
                .HasMaxLength(100);

            builder.Property(u => u.Rol)
                .HasMaxLength(30);
        }
    }
}
