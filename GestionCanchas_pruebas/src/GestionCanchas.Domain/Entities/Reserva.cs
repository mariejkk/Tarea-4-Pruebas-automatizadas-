namespace GestionCanchas.Domain.Entities
{
    public class Reserva
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public DateTime FechaReserva { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public int CanchaId { get; set; }
        public Cancha? Cancha { get; set; }
    }
}
