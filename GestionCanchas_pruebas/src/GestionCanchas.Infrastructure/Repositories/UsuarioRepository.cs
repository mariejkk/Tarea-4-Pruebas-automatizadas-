using GestionCanchas.Application.Interfaces;
using GestionCanchas.Domain.Entities;
using GestionCanchas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionCanchas.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
    }
}
