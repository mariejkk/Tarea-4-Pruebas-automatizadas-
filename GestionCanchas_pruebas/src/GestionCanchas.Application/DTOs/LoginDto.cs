using System.ComponentModel.DataAnnotations;

namespace GestionCanchas.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
