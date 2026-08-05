using GestionCanchas.Domain.Entities;

namespace GestionCanchas.Application.Interfaces
{
    public interface ICanchaRepository
    {
        Task<List<Cancha>> ObtenerTodasAsync();
        Task<Cancha?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Cancha cancha);
        Task ActualizarAsync(Cancha cancha);
        Task EliminarAsync(Cancha cancha);
        Task<bool> ExisteAsync(int id);
    }
}
