using GestionCanchas.Domain.Entities;

namespace GestionCanchas.Application.Interfaces
{
    public interface IReservaRepository
    {
        Task<List<Reserva>> ObtenerTodasAsync();
        Task<Reserva?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Reserva reserva);
        Task ActualizarAsync(Reserva reserva);
        Task EliminarAsync(Reserva reserva);
        Task<bool> ExisteAsync(int id);
    }
}
