using System.ComponentModel.DataAnnotations;


namespace apptrade.Models
{
public class Inmueble
{
public int Id { get; set; }


[Required, StringLength(30)]
public string Codigo { get; set; } = string.Empty; // único


[Required, StringLength(100)]
public string Titulo { get; set; } = string.Empty;


[Url]
public string? Imagen { get; set; }


[Required]
public TipoInmueble Tipo { get; set; }


[Required, StringLength(60)]
public string Ciudad { get; set; } = string.Empty;


[Required, StringLength(120)]
public string Direccion { get; set; } = string.Empty;


[Range(0, 20)]
public int Dormitorios { get; set; }


[Range(0, 20)]
public int Banos { get; set; }


[Range(1, int.MaxValue, ErrorMessage = "MetrosCuadrados debe ser > 0")]
public int MetrosCuadrados { get; set; }


[Range(1, double.MaxValue, ErrorMessage = "Precio debe ser > 0")]
public decimal Precio { get; set; }


public bool Activo { get; set; } = true;


public ICollection<Visita> Visitas { get; set; } = new List<Visita>();
public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
}