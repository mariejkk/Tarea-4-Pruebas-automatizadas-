namespace GestionCanchas.Domain.Entities
{
    public class Cancha
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoDeporte { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal PrecioPorHora { get; set; }
        public bool Disponible { get; set; } = true;

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
