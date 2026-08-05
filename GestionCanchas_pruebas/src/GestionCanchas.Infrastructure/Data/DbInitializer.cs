using GestionCanchas.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GestionCanchas.Infrastructure.Data
{
  
    public static class DbInitializer
    {
        public static async Task InicializarAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.Usuarios.AnyAsync())
            {
                var hasher = new PasswordHasher<Usuario>();
                var admin = new Usuario
                {
                    NombreUsuario = "admin",
                    NombreCompleto = "Administrador",
                    Rol = "Admin",
                    Activo = true
                };
                admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

                context.Usuarios.Add(admin);
            }

            if (!await context.Canchas.AnyAsync())
            {
                context.Canchas.AddRange(
                    new Cancha
                    {
                        Nombre = "Estadio Quisqueya - Fútbol 11",
                        TipoDeporte = "Futbol",
                        Ubicacion = "Zona Norte, Módulo A",
                        PrecioPorHora = 2500.00m,
                        Disponible = true
                    },
                    new Cancha
                    {
                        Nombre = "Cancha Techada Los Prados",
                        TipoDeporte = "Baloncesto",
                        Ubicacion = "Sector Central, Techado Principal",
                        PrecioPorHora = 1200.00m,
                        Disponible = true
                    },
                    new Cancha
                    {
                        Nombre = "Club Pádel Elite - Pista 1",
                        TipoDeporte = "Pádel",
                        Ubicacion = "Terraza Exterior, Zona Este",
                        PrecioPorHora = 1800.00m,
                        Disponible = true
                    },
                    new Cancha
                    {
                        Nombre = "Complex Tenis Pro - Cancha Central",
                        TipoDeporte = "Tenis",
                        Ubicacion = "Área de Raquetas, Bloque C",
                        PrecioPorHora = 1500.00m,
                        Disponible = false
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}