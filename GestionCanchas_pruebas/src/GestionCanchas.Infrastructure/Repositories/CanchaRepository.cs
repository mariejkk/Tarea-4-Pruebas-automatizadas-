using GestionCanchas.Application.Interfaces;
using GestionCanchas.Domain.Entities;
using GestionCanchas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionCanchas.Infrastructure.Repositories
{
    public class CanchaRepository : ICanchaRepository
    {
        private readonly AppDbContext _context;

        public CanchaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cancha>> ObtenerTodasAsync() =>
            await _context.Canchas.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync();

        public async Task<Cancha?> ObtenerPorIdAsync(int id) =>
            await _context.Canchas.Include(c => c.Reservas).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AgregarAsync(Cancha cancha)
        {
            _context.Canchas.Add(cancha);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Cancha cancha)
        {
            _context.Canchas.Update(cancha);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Cancha cancha)
        {
            _context.Canchas.Remove(cancha);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id) =>
            await _context.Canchas.AnyAsync(c => c.Id == id);
    }
}
