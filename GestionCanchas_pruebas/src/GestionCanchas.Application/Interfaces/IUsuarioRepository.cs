using GestionCanchas.Domain.Entities;

namespace GestionCanchas.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    }
}
