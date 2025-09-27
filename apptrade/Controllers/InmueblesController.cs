using System;                 // Math
using System.Linq;            // Where, OrderBy, Skip, Take, Any
using apptrade.Data;
using apptrade.Dto;
using apptrade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apptrade.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<InmueblesController> _log;

        public InmueblesController(ApplicationDbContext ctx, ILogger<InmueblesController> log)
        {
            _ctx = ctx;
            _log = log;
        }

        // GET: /Inmuebles/Catalogo
        [HttpGet]
        public async Task<IActionResult> Catalogo([FromQuery] CatalogoFiltroDto f)
        {
            f ??= new CatalogoFiltroDto();

            // Validaciones server-side
            if (f.PrecioMin.HasValue && f.PrecioMax.HasValue && f.PrecioMin > f.PrecioMax)
                ModelState.AddModelError(string.Empty, "El precio mínimo no puede ser mayor que el máximo");
            if (f.Page < 1) f.Page = 1;
            if (f.PageSize < 1) f.PageSize = 9;

            if (!ModelState.IsValid)
            {
                var baseItems = await _ctx.Inmuebles.AsNoTracking()
                                    .Where(i => i.Activo)
                                    .OrderBy(i => i.Titulo)
                                    .Take(f.PageSize)
                                    .ToListAsync();

                ViewBag.Total = await _ctx.Inmuebles.CountAsync(i => i.Activo);
                ViewBag.Page = 1; ViewBag.PageSize = f.PageSize;
                ViewBag.Filtro = f;
                return View(baseItems);
            }

            // Query base
            IQueryable<Inmueble> q = _ctx.Inmuebles.AsNoTracking().Where(i => i.Activo);

            // Filtros
            if (!string.IsNullOrWhiteSpace(f.Ciudad))
                q = q.Where(i => i.Ciudad == f.Ciudad);

            if (f.Tipo.HasValue)
                q = q.Where(i => i.Tipo == f.Tipo.Value);

            if (f.PrecioMin.HasValue)
                q = q.Where(i => i.Precio >= f.PrecioMin.Value);

            if (f.PrecioMax.HasValue)
                q = q.Where(i => i.Precio <= f.PrecioMax.Value);

            if (f.DormitoriosMin.HasValue)
                q = q.Where(i => i.Dormitorios >= f.DormitoriosMin.Value);

            var total = await q.CountAsync();

            // Paginación
            var page = Math.Max(1, f.Page);
            var size = Math.Clamp(f.PageSize, 1, 100);

            var items = await q.OrderBy(i => i.Titulo)
                               .Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.PageSize = size;
            ViewBag.Filtro = f;

            return View(items); // -> Views/Inmuebles/Catalogo.cshtml
        }

        // GET: /Inmuebles/Detalle/5
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var i = await _ctx.Inmuebles.AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == id && x.Activo);
            if (i == null) return NotFound();
            return View(i); // -> Views/Inmuebles/Detalle.cshtml
        }
    }
}
