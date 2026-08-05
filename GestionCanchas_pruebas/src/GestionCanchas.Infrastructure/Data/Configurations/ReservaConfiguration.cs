using GestionCanchas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionCanchas.Infrastructure.Data.Configurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.ToTable("Reservas");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.NombreCliente)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(r => r.Cancha)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.CanchaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
