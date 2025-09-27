using System;
using System.Linq;
using apptrade.Data;
using apptrade.Dto;
using apptrade.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apptrade.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<InmueblesController> _log;
        private readonly UserManager<IdentityUser> _userMgr;

        public InmueblesController(ApplicationDbContext ctx, ILogger<InmueblesController> log, UserManager<IdentityUser> userMgr)
        {
            _ctx = ctx; _log = log; _userMgr = userMgr;
        }

        // ... (TU acción Catalogo queda igual)

        // GET: /Inmuebles/Detalle/5  → muestra si hay reserva activa
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var i = await _ctx.Inmuebles.AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == id && x.Activo);
            if (i == null) return NotFound();

            var hayReservaActiva = await _ctx.Reservas
                .AnyAsync(r => r.InmuebleId == id && r.FechaExpiracion > DateTime.UtcNow);

            ViewBag.ReservaActiva = hayReservaActiva;
            return View(i);
        }

        // POST: /Inmuebles/AgendarVisita
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgendarVisita(int inmuebleId, DateTime fechaInicio, DateTime fechaFin, string? notas)
        {
            // Validaciones server-side
            if (fechaInicio >= fechaFin)
            {
                TempData["Error"] = "La fecha de inicio debe ser menor a la de fin.";
                return RedirectToAction("Detalle", new { id = inmuebleId });
            }

            // Horario laboral 08:00–19:00
            if (fechaInicio.Hour < 8 || fechaFin.Hour > 19)
            {
                TempData["Error"] = "Las visitas se permiten entre 08:00 y 19:00.";
                return RedirectToAction("Detalle", new { id = inmuebleId });
            }

            // No permitir solapamiento con otra visita del mismo inmueble
            bool solapa = await _ctx.Visitas.AnyAsync(v =>
                v.InmuebleId == inmuebleId &&
                ((fechaInicio < v.FechaFin) && (fechaFin > v.FechaInicio)));

            if (solapa)
            {
                TempData["Error"] = "Ya existe una visita en ese intervalo.";
                return RedirectToAction("Detalle", new { id = inmuebleId });
            }

            var userId = _userMgr.GetUserId(User)!;

            _ctx.Visitas.Add(new Visita
            {
                InmuebleId = inmuebleId,
                UsuarioId = userId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Notas = notas,
                Estado = EstadoVisita.Solicitada
            });

            await _ctx.SaveChangesAsync();
            TempData["Ok"] = "Visita agendada correctamente.";
            return RedirectToAction("Detalle", new { id = inmuebleId });
        }

        // POST: /Inmuebles/Reservar
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(int inmuebleId)
        {
            // Rechazar si ya existe reserva activa
            bool existe = await _ctx.Reservas.AnyAsync(r =>
                r.InmuebleId == inmuebleId && r.FechaExpiracion > DateTime.UtcNow);

            if (existe)
            {
                TempData["Error"] = "Ya existe una reserva activa para este inmueble.";
                return RedirectToAction("Detalle", new { id = inmuebleId });
            }

            var userId = _userMgr.GetUserId(User)!;

            _ctx.Reservas.Add(new Reserva
            {
                InmuebleId = inmuebleId,
                UsuarioId = userId,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddHours(48)
            });

            await _ctx.SaveChangesAsync();
            TempData["Ok"] = "Reserva creada por 48 horas.";
            return RedirectToAction("Detalle", new { id = inmuebleId });
        }
    }
}
