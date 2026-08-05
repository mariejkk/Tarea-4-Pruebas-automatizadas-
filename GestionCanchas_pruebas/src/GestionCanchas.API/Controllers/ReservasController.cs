using GestionCanchas.Application.DTOs;
using GestionCanchas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionCanchas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservaDto>>> ObtenerTodas() =>
            Ok(await _reservaService.ObtenerTodasAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservaDto>> ObtenerPorId(int id)
        {
            var reserva = await _reservaService.ObtenerPorIdAsync(id);
            return reserva is null ? NotFound() : Ok(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ReservaDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var resultado = await _reservaService.CrearAsync(dto);
            return resultado.Exitoso
                ? Created(string.Empty, dto)
                : BadRequest(new { errores = resultado.Errores });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ReservaDto dto)
        {
            if (id != dto.Id) return BadRequest(new { errores = new[] { "El id de la ruta no coincide con el del cuerpo." } });
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var resultado = await _reservaService.ActualizarAsync(dto);
            return resultado.Exitoso ? NoContent() : BadRequest(new { errores = resultado.Errores });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _reservaService.EliminarAsync(id);
            return resultado.Exitoso ? NoContent() : BadRequest(new { errores = resultado.Errores });
        }
    }
}
