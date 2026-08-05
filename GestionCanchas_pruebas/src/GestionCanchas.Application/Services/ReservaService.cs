using GestionCanchas.Application.Common;
using GestionCanchas.Application.DTOs;
using GestionCanchas.Application.Interfaces;
using GestionCanchas.Domain.Entities;

namespace GestionCanchas.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepositorio;
        private readonly ICanchaRepository _canchaRepositorio;

        public ReservaService(IReservaRepository reservaRepositorio, ICanchaRepository canchaRepositorio)
        {
            _reservaRepositorio = reservaRepositorio;
            _canchaRepositorio = canchaRepositorio;
        }

        public async Task<List<ReservaDto>> ObtenerTodasAsync()
        {
            var reservas = await _reservaRepositorio.ObtenerTodasAsync();
            return reservas.Select(MapearADto).ToList();
        }

        public async Task<ReservaDto?> ObtenerPorIdAsync(int id)
        {
            var reserva = await _reservaRepositorio.ObtenerPorIdAsync(id);
            return reserva is null ? null : MapearADto(reserva);
        }

        public async Task<ServiceResult> CrearAsync(ReservaDto dto)
        {
            var validacion = await ValidarReglasDeNegocioAsync(dto);
            if (!validacion.Exitoso) return validacion;

            var reserva = new Reserva
            {
                CanchaId = dto.CanchaId,
                NombreCliente = dto.NombreCliente.Trim(),
                FechaReserva = dto.FechaReserva.Date,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin
            };

            await _reservaRepositorio.AgregarAsync(reserva);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> ActualizarAsync(ReservaDto dto)
        {
            var validacion = await ValidarReglasDeNegocioAsync(dto);
            if (!validacion.Exitoso) return validacion;

            var reserva = await _reservaRepositorio.ObtenerPorIdAsync(dto.Id);
            if (reserva is null)
                return ServiceResult.Fallo("La reserva no existe.");

            reserva.CanchaId = dto.CanchaId;
            reserva.NombreCliente = dto.NombreCliente.Trim();
            reserva.FechaReserva = dto.FechaReserva.Date;
            reserva.HoraInicio = dto.HoraInicio;
            reserva.HoraFin = dto.HoraFin;

            await _reservaRepositorio.ActualizarAsync(reserva);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> EliminarAsync(int id)
        {
            var reserva = await _reservaRepositorio.ObtenerPorIdAsync(id);
            if (reserva is null)
                return ServiceResult.Fallo("La reserva no existe.");

            await _reservaRepositorio.EliminarAsync(reserva);
            return ServiceResult.Ok();
        }

        private async Task<ServiceResult> ValidarReglasDeNegocioAsync(ReservaDto dto)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.NombreCliente))
                errores.Add("El nombre del cliente es obligatorio.");

            var cancha = await _canchaRepositorio.ObtenerPorIdAsync(dto.CanchaId);
            if (cancha is null)
                errores.Add("La cancha especificada no existe.");
            else if (!cancha.Disponible)
                errores.Add("La cancha seleccionada no esta disponible.");

            if (dto.FechaReserva.Date < DateTime.Today)
                errores.Add("No se puede reservar en una fecha pasada.");

            if (dto.HoraFin <= dto.HoraInicio)
                errores.Add("La hora de fin debe ser posterior a la hora de inicio.");

            return errores.Count == 0 ? ServiceResult.Ok() : ServiceResult.Fallo(errores.ToArray());
        }

        private static ReservaDto MapearADto(Reserva r) => new()
        {
            Id = r.Id,
            CanchaId = r.CanchaId,
            NombreCancha = r.Cancha?.Nombre,
            NombreCliente = r.NombreCliente,
            FechaReserva = r.FechaReserva,
            HoraInicio = r.HoraInicio,
            HoraFin = r.HoraFin
        };
    }
}
