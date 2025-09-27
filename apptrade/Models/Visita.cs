using System.ComponentModel.DataAnnotations;


namespace apptrade.Models
{
public class Visita
{
public int Id { get; set; }
[Required]
public int InmuebleId { get; set; }
[Required]
public string UsuarioId { get; set; } = string.Empty; // IdentityUser.Id


[Display(Name = "Inicio"), DataType(DataType.DateTime)]
public DateTime FechaInicio { get; set; }


[Display(Name = "Fin"), DataType(DataType.DateTime)]
public DateTime FechaFin { get; set; }


public EstadoVisita Estado { get; set; } = EstadoVisita.Solicitada;


[StringLength(200)]
public string? Notas { get; set; }


public Inmueble? Inmueble { get; set; }
}
}