using GestionCanchas.Application.Common;
using GestionCanchas.Application.DTOs;
using GestionCanchas.Domain.Entities;

namespace GestionCanchas.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<Usuario>> ValidarCredencialesAsync(LoginDto dto);
    }
}
