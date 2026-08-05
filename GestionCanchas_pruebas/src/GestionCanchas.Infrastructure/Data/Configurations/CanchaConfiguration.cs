using GestionCanchas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionCanchas.Infrastructure.Data.Configurations
{
    public class CanchaConfiguration : IEntityTypeConfiguration<Cancha>
    {
        public void Configure(EntityTypeBuilder<Cancha> builder)
        {
            builder.ToTable("Canchas");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.TipoDeporte)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Ubicacion)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.PrecioPorHora)
                .HasPrecision(10, 2);
        }
    }
}
