using GestionCanchas.Application.Common;
using GestionCanchas.Application.DTOs;

namespace GestionCanchas.Application.Interfaces
{
    public interface IReservaService
    {
        Task<List<ReservaDto>> ObtenerTodasAsync();
        Task<ReservaDto?> ObtenerPorIdAsync(int id);
        Task<ServiceResult> CrearAsync(ReservaDto dto);
        Task<ServiceResult> ActualizarAsync(ReservaDto dto);
        Task<ServiceResult> EliminarAsync(int id);
    }
}
