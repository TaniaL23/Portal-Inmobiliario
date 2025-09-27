using System.ComponentModel.DataAnnotations;


namespace apptrade.Models
{
public class Reserva
{
public int Id { get; set; }
[Required]
public int InmuebleId { get; set; }
[Required]
public string UsuarioId { get; set; } = string.Empty; // IdentityUser.Id


[Display(Name = "Expira"), DataType(DataType.DateTime)]
public DateTime FechaExpiracion { get; set; }


[Display(Name = "Creada"), DataType(DataType.DateTime)]
public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


public Inmueble? Inmueble { get; set; }
}
}