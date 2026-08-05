using System.ComponentModel.DataAnnotations;

namespace GestionCanchas.Application.DTOs
{
    public class CanchaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de deporte es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de deporte no puede superar los 50 caracteres.")]
        public string TipoDeporte { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ubicacion es obligatoria.")]
        [StringLength(150, ErrorMessage = "La ubicacion no puede superar los 150 caracteres.")]
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio por hora es obligatorio.")]
        [Range(0.01, 10000, ErrorMessage = "El precio por hora debe estar entre 0.01 y 10,000.")]
        public decimal PrecioPorHora { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
