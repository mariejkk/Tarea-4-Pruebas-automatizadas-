using GestionCanchas.Application.Common;
using GestionCanchas.Application.DTOs;

namespace GestionCanchas.Application.Interfaces
{
    public interface ICanchaService
    {
        Task<List<CanchaDto>> ObtenerTodasAsync();
        Task<CanchaDto?> ObtenerPorIdAsync(int id);
        Task<ServiceResult> CrearAsync(CanchaDto dto);
        Task<ServiceResult> ActualizarAsync(CanchaDto dto);
        Task<ServiceResult> EliminarAsync(int id);
        Task<ServiceResult> AlternarDisponibilidadAsync(int id);
    }
}
