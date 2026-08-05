using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionCanchas.Application.DTOs;
using GestionCanchas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GestionCanchas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var resultado = await _authService.ValidarCredencialesAsync(dto);

            if (!resultado.Exitoso)
                return Unauthorized(new { errores = resultado.Errores });

            var usuario = resultado.Data!;
            var token = GenerarToken(usuario.NombreUsuario, usuario.Rol);

            return Ok(new
            {
                token,
                usuario = new { usuario.NombreUsuario, usuario.NombreCompleto, usuario.Rol }
            });
        }

        private string GenerarToken(string nombreUsuario, string rol)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, nombreUsuario),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
