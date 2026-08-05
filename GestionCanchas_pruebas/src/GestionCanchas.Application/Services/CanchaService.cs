using GestionCanchas.Application.Common;
using GestionCanchas.Application.DTOs;
using GestionCanchas.Application.Interfaces;
using GestionCanchas.Domain.Entities;

namespace GestionCanchas.Application.Services
{
    public class CanchaService : ICanchaService
    {
        private readonly ICanchaRepository _repositorio;

        public CanchaService(ICanchaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<CanchaDto>> ObtenerTodasAsync()
        {
            var canchas = await _repositorio.ObtenerTodasAsync();
            return canchas.Select(MapearADto).ToList();
        }

        public async Task<CanchaDto?> ObtenerPorIdAsync(int id)
        {
            var cancha = await _repositorio.ObtenerPorIdAsync(id);
            return cancha is null ? null : MapearADto(cancha);
        }

        public async Task<ServiceResult> CrearAsync(CanchaDto dto)
        {
            var validacion = ValidarReglasDeNegocio(dto);
            if (!validacion.Exitoso) return validacion;

            var cancha = new Cancha
            {
                Nombre = dto.Nombre.Trim(),
                TipoDeporte = dto.TipoDeporte.Trim(),
                Ubicacion = dto.Ubicacion.Trim(),
                PrecioPorHora = dto.PrecioPorHora,
                Disponible = dto.Disponible
            };

            await _repositorio.AgregarAsync(cancha);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> ActualizarAsync(CanchaDto dto)
        {
            var validacion = ValidarReglasDeNegocio(dto);
            if (!validacion.Exitoso) return validacion;

            var cancha = await _repositorio.ObtenerPorIdAsync(dto.Id);
            if (cancha is null)
                return ServiceResult.Fallo("La cancha no existe.");

            cancha.Nombre = dto.Nombre.Trim();
            cancha.TipoDeporte = dto.TipoDeporte.Trim();
            cancha.Ubicacion = dto.Ubicacion.Trim();
            cancha.PrecioPorHora = dto.PrecioPorHora;
            cancha.Disponible = dto.Disponible;

            await _repositorio.ActualizarAsync(cancha);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> EliminarAsync(int id)
        {
            var cancha = await _repositorio.ObtenerPorIdAsync(id);
            if (cancha is null)
                return ServiceResult.Fallo("La cancha no existe.");

            if (cancha.Reservas.Any())
                return ServiceResult.Fallo("No se puede eliminar una cancha que tiene reservas asociadas.");

            await _repositorio.EliminarAsync(cancha);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> AlternarDisponibilidadAsync(int id)
        {
            var cancha = await _repositorio.ObtenerPorIdAsync(id);
            if (cancha is null)
                return ServiceResult.Fallo("La cancha no existe.");

            cancha.Disponible = !cancha.Disponible;
            await _repositorio.ActualizarAsync(cancha);
            return ServiceResult.Ok();
        }

        private static ServiceResult ValidarReglasDeNegocio(CanchaDto dto)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                errores.Add("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.TipoDeporte))
                errores.Add("El tipo de deporte es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Ubicacion))
                errores.Add("La ubicacion es obligatoria.");

            if (dto.PrecioPorHora <= 0)
                errores.Add("El precio por hora debe ser mayor a 0.");

            if (dto.PrecioPorHora > 10000)
                errores.Add("El precio por hora no puede superar 10,000.");

            return errores.Count == 0 ? ServiceResult.Ok() : ServiceResult.Fallo(errores.ToArray());
        }

        private static CanchaDto MapearADto(Cancha c) => new()
        {
            Id = c.Id,
            Nombre = c.Nombre,
            TipoDeporte = c.TipoDeporte,
            Ubicacion = c.Ubicacion,
            PrecioPorHora = c.PrecioPorHora,
            Disponible = c.Disponible
        };
    }
}
