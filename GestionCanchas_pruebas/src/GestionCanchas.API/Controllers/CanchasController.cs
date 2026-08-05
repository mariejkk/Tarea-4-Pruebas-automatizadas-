using GestionCanchas.Application.DTOs;
using GestionCanchas.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionCanchas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CanchasController : ControllerBase
    {
        private readonly ICanchaService _canchaService;

        public CanchasController(ICanchaService canchaService)
        {
            _canchaService = canchaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CanchaDto>>> ObtenerTodas() =>
            Ok(await _canchaService.ObtenerTodasAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CanchaDto>> ObtenerPorId(int id)
        {
            var cancha = await _canchaService.ObtenerPorIdAsync(id);
            return cancha is null ? NotFound() : Ok(cancha);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CanchaDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var resultado = await _canchaService.CrearAsync(dto);
            return resultado.Exitoso
                ? Created(string.Empty, dto)
                : BadRequest(new { errores = resultado.Errores });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CanchaDto dto)
        {
            if (id != dto.Id) return BadRequest(new { errores = new[] { "El id de la ruta no coincide con el del cuerpo." } });
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var resultado = await _canchaService.ActualizarAsync(dto);
            return resultado.Exitoso ? NoContent() : BadRequest(new { errores = resultado.Errores });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _canchaService.EliminarAsync(id);
            return resultado.Exitoso ? NoContent() : BadRequest(new { errores = resultado.Errores });
        }

        [HttpPatch("{id:int}/disponibilidad")]
        public async Task<IActionResult> AlternarDisponibilidad(int id)
        {
            var resultado = await _canchaService.AlternarDisponibilidadAsync(id);
            return resultado.Exitoso ? NoContent() : BadRequest(new { errores = resultado.Errores });
        }
    }
}
