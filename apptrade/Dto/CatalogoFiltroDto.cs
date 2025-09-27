using System.ComponentModel.DataAnnotations;
using apptrade.Models;

namespace apptrade.Dto
{
    public class CatalogoFiltroDto
    {
        public string? Ciudad { get; set; }
        public TipoInmueble? Tipo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Precio mínimo no puede ser negativo")]
        public decimal? PrecioMin { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Precio máximo no puede ser negativo")]
        public decimal? PrecioMax { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Dormitorios debe ser ≥ 0")]
        public int? DormitoriosMin { get; set; }

        // Paginación
        [Range(1, int.MaxValue)] public int Page { get; set; } = 1;
        [Range(1, 100)] public int PageSize { get; set; } = 9;
    }
}
