using GestionCanchas.Application.Interfaces;
using GestionCanchas.Domain.Entities;
using GestionCanchas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionCanchas.Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reserva>> ObtenerTodasAsync() =>
            await _context.Reservas
                .Include(r => r.Cancha)
                .AsNoTracking()
                .OrderByDescending(r => r.FechaReserva)
                .ToListAsync();

        public async Task<Reserva?> ObtenerPorIdAsync(int id) =>
            await _context.Reservas.Include(r => r.Cancha).FirstOrDefaultAsync(r => r.Id == id);

        public async Task AgregarAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Reserva reserva)
        {
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Reserva reserva)
        {
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id) =>
            await _context.Reservas.AnyAsync(r => r.Id == id);
    }
}
