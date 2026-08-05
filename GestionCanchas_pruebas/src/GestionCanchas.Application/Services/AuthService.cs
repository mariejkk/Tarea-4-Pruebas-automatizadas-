using GestionCanchas.Application.Common;
using GestionCanchas.Application.DTOs;
using GestionCanchas.Application.Interfaces;
using GestionCanchas.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestionCanchas.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepositorio;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public AuthService(IUsuarioRepository usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<ServiceResult<Usuario>> ValidarCredencialesAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NombreUsuario) || string.IsNullOrWhiteSpace(dto.Password))
                return ServiceResult<Usuario>.Fallo("Usuario y contraseña son obligatorios.");

            var usuario = await _usuarioRepositorio.ObtenerPorNombreUsuarioAsync(dto.NombreUsuario.Trim());

            if (usuario is null || !usuario.Activo)
                return ServiceResult<Usuario>.Fallo("Usuario o contraseña incorrectos.");

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password);
            if (resultado == PasswordVerificationResult.Failed)
                return ServiceResult<Usuario>.Fallo("Usuario o contraseña incorrectos.");

            return ServiceResult<Usuario>.Ok(usuario);
        }
    }
}
