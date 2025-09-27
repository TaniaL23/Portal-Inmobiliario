using System.ComponentModel.DataAnnotations;

namespace apptrade.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required] public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; } = default!;

        [Required] public string UsuarioId { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaExpiracion { get; set; } = DateTime.UtcNow.AddHours(48);
    }
}
