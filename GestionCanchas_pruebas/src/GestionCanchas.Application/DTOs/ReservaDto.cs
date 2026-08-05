using System.ComponentModel.DataAnnotations;

namespace GestionCanchas.Application.DTOs
{
    public class ReservaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una cancha.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cancha valida.")]
        public int CanchaId { get; set; }

        public string? NombreCancha { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del cliente no puede superar los 100 caracteres.")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de reserva es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaReserva { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        [DataType(DataType.Time)]
        public TimeSpan HoraFin { get; set; }
    }
}
